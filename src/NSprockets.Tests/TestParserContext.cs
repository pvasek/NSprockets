using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NSprockets.Abstract;

namespace NSprockets.Tests
{
    public class TestParserContext: IParserContext
    {
        public TestParserContext()
        {
            Directives = new List<string>();
            FilteredContent = new StringWriter();
        }

        public List<string> Directives { get; private set; }
        public StringWriter FilteredContent { get; private set; }

        public void AddDirective(string directiveText)
        {
            Directives.Add(directiveText);
        }
    }
}
