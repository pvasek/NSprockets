using System.Collections.Generic;

namespace NSprockets.Abstract
{
    public interface IAssetLoader
    {
        Asset Load(string name);
        IEnumerable<Asset> FromDirectory(string dir);
        IEnumerable<Asset> FromTree(string dir);
        List<IAssetProcessor> FindProcessors(AssetFile file);
    }
}
