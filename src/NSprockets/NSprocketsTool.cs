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
            Files = new List<string>();
            Directories = new List<string>();
        }

        public List<string> Files { get; private set; }
        public List<string> Directories { get; private set; }
        public List<string> LookupDirectories { get; private set; }
        public string DefaultOutputDirectory { get; set; }
        public List<IAssetProcessor> Processors { get; private set; }

      
        private void WalkThrough(List<string> from, List<string> directories, List<string> files)
        {
            foreach (var item in from)
            {
                directories.Add(item);
                files.AddRange(Directory.GetFiles(item));
                WalkThrough(Directory.GetDirectories(item).ToList(), directories, files);
            }
        }

        public void Initialize()
        {
            WalkThrough(LookupDirectories, Directories, Files);
        }

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
