using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NSprockets
{
    public class Asset
    {
        public Asset(AssetLoader loader, string fileName, string rootDir)
        {
            _loader = loader;
            FullFileName = fileName;            
            if (!fileName.StartsWith(rootDir))
            {
                throw new ArgumentException("The file name is not in the root directory");
            }
            
            _children = new List<Asset>();

            var tmp = fileName.Remove(0, rootDir.Length);
            CheckType(AssetType.Css, tmp);
            CheckType(AssetType.Js, tmp);
        }

        private bool _loaded = false;
        private object _syncRoot = new Object();
        private readonly AssetLoader _loader;
        private readonly List<Asset> _children;

        private void CheckType(AssetType type, string file)
        {
            var tmp = file.ToLower();
            var rootExtension = "." + type.ToString().ToLower();
            if (tmp.EndsWith(rootExtension) || tmp.Contains(rootExtension + "."))
            {
                Type = type;
                int extensionStart = tmp.IndexOf(rootExtension);
                Extension = tmp.Substring(extensionStart, tmp.Length - extensionStart);
                var name = Path.GetFileName(tmp); 
                Name = name.Remove(name.Length - Extension.Length, Extension.Length);
                FileName = Name + rootExtension;
                var path = Path.GetDirectoryName(tmp).Replace(@"\", "/");
                if (path.StartsWith("/"))
                {
                    path = path.Substring(1);
                }
                Directory = path;
                DirectoryParts = path.Split('/').ToList();
            }
        }

        public string Name { get; private set; }
        public string FileName { get; private set; }
        public string Extension { get; private set; }
        public string Directory { get; private set; }
        public string FullFileName { get; private set; }
        public AssetType Type { get; private set; }
        public List<string> DirectoryParts { get; private set; }
        public string Content { get; private set; }
        public IEnumerable<Asset> Children { get { return _children; } }

        public bool IsInDirectory(string dir)
        {
            dir = dir.ToLower();
            return dir == Directory || Directory.EndsWith("/" + dir);
        }

        public bool IsInTree(string dir)
        {
            dir = dir.ToLower();
            return IsInDirectory(dir) || DirectoryParts.Contains(dir);
        }

        public bool HasName(string name)
        {
            name = name.ToLower();
            return name == Name || name == FileName;
        }

        public Asset Load()
        {
            lock (_syncRoot)
            {
                if (_loaded) return this;

                Content = File.ReadAllText(FullFileName);
                var parser = DirectiveParser.ForType(Type);
                if (parser != null)
                {
                    Parse(parser);
                }
                return this;
            }
        }

        private void Parse(DirectiveParser parser)
        {
            var context = new ParserContext(Name);
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
            }
        }
    }
}
