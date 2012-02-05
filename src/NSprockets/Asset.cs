using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using NSprockets.Abstract;

namespace NSprockets
{
    public class Asset
    {
        public AssetFile File { get; private set; }        

        public Asset(IAssetLoader loader, string fileName, string rootDir): 
            this(loader, new AssetFile(fileName, rootDir))
        {
        }

        public Asset(IAssetLoader loader, AssetFile file)
        {
            _loader = loader;
            File = file;
            _children = new List<Asset>();
        }

        private bool _loaded;
        private readonly object _syncRoot = new Object();
        private readonly IAssetLoader _loader;
        private readonly List<Asset> _children;
        public string Content { get; private set; }
        public IEnumerable<Asset> Children { get { return _children; } }
        public bool IsManifestOnly { get; set; }

        private Asset CreateManifestContentAsset()
        {
            IsManifestOnly = true;
            var result = new Asset(_loader, File);
            result.Content = Content;
            result.ProcessContent();
            return result;
        }

        public Asset Load()
        {
            lock (_syncRoot)
            {
                if (_loaded) return this;

                Content = System.IO.File.ReadAllText(File.OriginalFile);
                var parser = DirectiveParser.ForType(File.Type);
                if (parser != null)
                {
                    Parse(parser);
                }
                ProcessContent();
                _loaded = true;
                return this;
            }
        }

        private void ProcessContent()
        {
            var processors = _loader.FindProcessors(File);
            foreach (var processor in processors)
            {
                var processorContext = new ProcessorContext();
                processor.Parse(new StringReader(Content), processorContext);
                Content = processorContext.Output.ToString();
            }
        }

        private void Parse(DirectiveParser parser)
        {
            var context = new ParserContext(File);
            parser.Parse(new StringReader(Content), context);
            var newContent = context.FilteredContent.ToString();
            if (newContent != String.Empty)
            {
                Content = newContent;
            }

            foreach (var directive in context.Directives)
            {
                if (directive.Type == DirectiveType.RequireDirectory)
                {
                    _children.AddRange(_loader
                        .FromDirectory(directive.Parameter)
                        .Select(i => i.Load()));
                }
                else if (directive.Type == DirectiveType.RequireTree)
                {
                    _children.AddRange(_loader
                        .FromTree(directive.Parameter)
                        .Select(i => i.Load()));
                }
                else if (directive.Type == DirectiveType.Require)
                {
                    _children.Add(_loader.Load(directive.Parameter));
                }
                else if (directive.Type == DirectiveType.RequireSelf)
                {
                    _children.Add(CreateManifestContentAsset());
                }
            }
        }
    }
}
