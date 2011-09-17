using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSprockets.Tests
{
    public class TestParserContext: IParserContext
    {
        public TestParserContext()
        {
            Directives = new List<string>();
        }

        public List<string> Directives { get; private set; }

        public void AddDirective(string directiveText)
        {
            Directives.Add(directiveText);
        }
    }
}
