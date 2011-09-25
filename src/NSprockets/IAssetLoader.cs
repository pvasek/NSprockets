using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSprockets
{
    public interface IAssetLoader
    {
        Asset Load(string name);
        IEnumerable<Asset> FromDirectory(string dir);
        IEnumerable<Asset> FromTree(string dir);
        IAssetProcessor FindProcessor(string extension);
    }
}
