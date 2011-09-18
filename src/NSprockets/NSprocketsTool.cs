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
        private List<FilePointer> _requiredFiles = new List<FilePointer>();

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
      
        private void Require(string file) 
        { 
        }

        private void RequireTree(string rootDir)
        {
        }

        private void RequireDirectory(string dir)
        {
        }
    }
    
}
