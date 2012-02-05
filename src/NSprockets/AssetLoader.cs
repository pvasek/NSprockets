using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using NSprockets.Abstract;

namespace NSprockets
{
    public class AssetLoader : IAssetLoader
    {
        public AssetLoader(IEnumerable<string> lookupDirectories)
        {
            Assets = new List<Asset>();
            _lookupDirectories = new List<string>(lookupDirectories);
            foreach (var dir in _lookupDirectories)
            {
                WalkThrough(dir, dir);
            }
        }

        private readonly List<string> _lookupDirectories;

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
            return Assets.Where(i => i.File.IsInTree(dir));
        }

        public IEnumerable<Asset> FromDirectory(string dir)
        {
            return Assets.Where(i => i.File.IsInDirectory(dir));
        }

        public Asset Load(string name)
        {
            Asset asset = Assets.FirstOrDefault(i => i.File.HasName(name));
            if (asset == null)
            {
                return null;
            }
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
            foreach (var asset in files.Select(Load).Where(i => i != null))
            {
                ForAssetTree(asset, i => result.Add(i.File.OriginalFile));
            }
            return result.Distinct().ToList();
        }

        public string GetContent(string file)
        {
            var result = new StringWriter();
            ForAssetTree(Load(file), i => result.Write(i.Content));
            return result.ToString();
        }

        public List<IAssetProcessor> FindProcessors(AssetFile file)
        {
            return AssetPipeline.Processors.Where(i => i.IsForFile(file)).ToList();
        }
    }
}
