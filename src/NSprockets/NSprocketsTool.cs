using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NSprockets
{
    public class NSprocketsTool
    {
        private DirectiveParser _cssParser;
        private DirectiveParser _jsParser;

        public NSprocketsTool()
        {
            LookupDirectories = new List<string>();
            _cssParser = new DirectiveParser("*=");
            _jsParser = new DirectiveParser("//=");
            Processors = new List<IAssetProcessor>();
        }

        public List<string> LookupDirectories { get; private set; }
        public string DefaultOutputDirectory { get; set; }
        public List<IAssetProcessor> Processors { get; private set; }
      
    }
    
}
