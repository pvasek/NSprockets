using System.Collections.Generic;
using System.Linq;
using NSprockets.Abstract;

namespace NSprockets
{
    public class AssetFile
    {
        public AssetFile(string file, string assetDir)
        {
            OriginalFile = file;
            File = file.NormalizePath();
            assetDir = assetDir.NormalizePath();
            Name = File.Remove(0, assetDir.Length).NormalizePath();
            int nameIndex = Name.LastIndexOf("/", System.StringComparison.Ordinal);
            Path = nameIndex == -1 ? "" : Name.Substring(0, nameIndex);
            Extensions = new List<string>();
            foreach (string extension in Name.Split('.').Reverse())
            {
                if (!AssetPipeline.KnownExtensions.Any(i => i.NormalizePath() == extension))
                    break;
                Extensions.Add(extension);
            }
            Extension = "." + string.Join(".", ((IEnumerable<string>)Extensions).Reverse());
            if (Extension.Length > 1)
            {
                Name = Name.Remove(Name.Length - Extension.Length, Extension.Length);
            }

            if (Extensions.Any(i => AssetPipeline.JavascriptExtensions.Contains(i)))
            {
                Type = AssetType.Js;
            }
            else if (Extensions.Any(i => AssetPipeline.CssExtensions.Contains(i)))
            {
                Type = AssetType.Css;
            }
        }

        public string OriginalFile { get; set; }
        public string File { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public List<string> Extensions { get; set; }
        public AssetType Type { get; set; }

        public bool IsInTree(string path)
        {
            path = path.NormalizePath();
            return Path.StartsWith(path);
        }

        public bool IsInDirectory(string path)
        {
            path = path.NormalizePath();
            return Path == path;
        }

        public bool HasName(string name)
        {
            name = name.NormalizePath();
            return Name == name || (Name + Extension) == name;
        }
    }
}