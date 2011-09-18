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
            Assets = new List<Asset>();
            foreach (var dir in lookupDirectories)
            {
                WalkThrough(dir, dir);
            }
        }

        private void WalkThrough(string dir, string rootDir)
        {
            Assets.AddRange(Directory.GetFiles(dir).Select(i => new Asset(this, i, rootDir)));
            foreach (var subdir in Directory.GetDirectories(dir))
            {
                WalkThrough(subdir, rootDir);
            }
        }

        public List<Asset> Assets { get; private set; }

        public IEnumerable<Asset> FromTree(string dir)
        {
            return Assets.Where(i => i.IsInTree(dir));
        }

        public IEnumerable<Asset> FromDirectory(string dir)
        {
            return Assets.Where(i => i.IsInDirectory(dir));
        }

        public Asset Load(string name)
        {
            return Assets.First(i => i.HasName(name)).Load();
        }

        private void ForAssetTree(Asset root, Action<Asset> action)
        {
            action(root);
            foreach (var item in root.Children)
            {
                ForAssetTree(item, action);
            }
        }

        public List<string> GetFiles(IEnumerable<string> files)
        {
            var result = new List<string>();
            foreach (var asset in files.Select(i => Load(i)))
            {
                ForAssetTree(asset, i => {
                    result.Add(i.FileName);
                });
            }
            return result.Distinct().ToList();
        }
    }
}
