using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NSprockets
{
    public class Asset
    {
        public Asset(string fileName, string rootDir)
        {
            FileName = fileName;
            if (!fileName.StartsWith(rootDir))
            {
                throw new ArgumentException("The file name is not in the root directory");
            }
            var tmp = fileName.Remove(0, rootDir.Length);
            CheckType(AssetType.Css, tmp);
            CheckType(AssetType.Js, tmp);
        }

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
                var path = Path.GetDirectoryName(tmp).Replace(@"\", "/");
                if (path.StartsWith("/"))
                {
                    path = path.Substring(1);
                }
                DirectoryPath = path;
                Directories = path.Split('/').ToList();
            }
        }

        public string Name { get; private set; }
        public string Extension { get; private set; }
        public string DirectoryPath { get; private set; }
        public string FileName { get; private set; }
        public AssetType Type { get; private set; }
        public List<string> Directories { get; private set; }

        public bool IsIn(string dir)
        {
            dir = dir.ToLower();
            return dir == DirectoryPath || Directories.Contains(dir);
        }
    }
}
