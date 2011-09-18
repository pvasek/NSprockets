using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.IO;

namespace NSprockets.Tests
{
    [TestFixture]
    public class AssetLoaderTest
    {
        [Test]
        public void InitializeTest()
        {            
            Uri uri = new Uri(Path.GetDirectoryName(typeof(AssetLoaderTest).Assembly.CodeBase));
            var dir = Path.Combine(uri.AbsolutePath, "assets");
            var lookupDirectories = new List<string>();
            lookupDirectories.Add(dir);

            var target = new AssetLoader(lookupDirectories);
            Assert.AreEqual(5, target.Directories.Count);
            Assert.AreEqual(6, target.Files.Count);
        }
    }
}
