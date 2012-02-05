using System.Collections.Generic;
using System.IO;
using NSprockets.Abstract;

namespace NSprockets
{
    public class DirectiveParser
    {        
        private readonly string _lineStart;

        public static DirectiveParser CssParser { get; private set; }
        public static DirectiveParser JsParser { get; private set; }
        public static List<DirectiveParser> DefaultParsers { get; private set; }

        public static DirectiveParser ForType(AssetType assetType)
        {
            if (assetType == AssetType.Css)
                return CssParser;
            if (assetType == AssetType.Js)
                return JsParser;
            return null;
        }

        static DirectiveParser()
        {
            CssParser = new DirectiveParser("*=");
            JsParser = new DirectiveParser("//=");
            DefaultParsers = new List<DirectiveParser> { CssParser, JsParser };
        }

        public DirectiveParser(string lineStart)
        {
            _lineStart = lineStart;
        }

        public void Parse(TextReader reader, IParserContext context)
        {
            var line = reader.ReadLine();
            while (line != null)
            {
                var text = line.Trim();
                if (text.StartsWith(_lineStart))
                {
                    var directive = text.Remove(0, _lineStart.Length);
                    context.AddDirective(directive);
                }
                else
                {
                    context.FilteredContent.WriteLine(line);
                }
                line = reader.ReadLine();
            }
        }
    }
}
