using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using NSprockets.Abstract;

namespace NSprockets
{
    public class AssetLoader : IAssetLoader
    {
        public AssetLoader(IEnumerable<string> lookupDirectories, IEnumerable<IAssetProcessor> processors)
        {
            Assets = new List<Asset>();
            foreach (var dir in lookupDirectories)
            {
                WalkThrough(dir, dir);
            }
            _processors = new List<IAssetProcessor>(processors);
        }

        private readonly List<IAssetProcessor> _processors;

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
            Asset asset = Assets.First(i => i.HasName(name));
            return asset.Load();
        }

        private void ForAssetTree(Asset root, Action<Asset> action)
        {
            if (!root.IsManifestOnly)
            {
                action(root);
            }
            foreach (var item in root.Children)
            {
                ForAssetTree(item, action);
            }
        }

        public List<string> GetFiles(string file)
        {
            return GetFiles(new[] { file });
        }

        public List<string> GetFiles(IEnumerable<string> files)
        {
            var result = new List<string>();
            foreach (var asset in files.Select(Load))
            {
                ForAssetTree(asset, i => result.Add(i.FullFileName));
            }
            return result.Distinct().ToList();
        }

        public string GetContent(string file)
        {
            var result = new StringWriter();
            ForAssetTree(Load(file), i => result.Write(i.Content));
            return result.ToString();
        }

        public IAssetProcessor FindProcessor(string extension)
        {
            return _processors.FirstOrDefault(i => i.IsForExtension(extension));
        }
    }
}
