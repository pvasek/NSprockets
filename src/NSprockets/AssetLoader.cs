using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NSprockets
{
    public class AssetLoader
    {
        public AssetLoader(IEnumerable<string> lookupDirectories)
        {
            Files = new List<string>();
            Directories = new List<string>();
            WalkThrough(lookupDirectories, Directories, Files);
        }

        private void WalkThrough(IEnumerable<string> throughDirectories, List<string> directories, List<string> files)
        {
            foreach (var item in throughDirectories)
            {
                directories.Add(item);
                files.AddRange(Directory.GetFiles(item));
                WalkThrough(Directory.GetDirectories(item).ToList(), directories, files);
            }
        }

        public List<string> Files { get; private set; }
        public List<string> Directories { get; private set; }

        public List<string> GetTree(string dir)
        {
            throw new NotImplementedException();
        }

        public List<string> GetDirectory(string dir)
        {
            throw new NotImplementedException();
        }

    }
}
