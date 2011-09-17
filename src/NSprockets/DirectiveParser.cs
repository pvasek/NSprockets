using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NSprockets
{
    public class DirectiveParser
    {        
        private string _lineStart;

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
                line = reader.ReadLine();
            }
        }
    }
}
