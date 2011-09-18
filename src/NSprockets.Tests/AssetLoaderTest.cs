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
        private List<string> _lookupDirectories = new List<string>();

        [SetUp]
        public void SetUp()
        {
            Uri uri = new Uri(Path.GetDirectoryName(typeof(AssetLoaderTest).Assembly.CodeBase));
            var dir = Path.Combine(uri.AbsolutePath, "assets");            
            _lookupDirectories.Add(dir);
        }

        [Test]
        public void BasicTest()
        {            
            var target = new AssetLoader(_lookupDirectories);
            Assert.AreEqual(6, target.Assets.Count);
            Assert.AreEqual(2, target.FromDirectory("scripts1").Count());
            Assert.AreEqual(6, target.FromTree("scripts1").Count());
        }

        [Test]
        public void LoadTest()
        {
            var target = new AssetLoader(_lookupDirectories);
            Asset result = target.Load("test1.a.js");
            Assert.AreEqual(1, result.Children.Count());
        }
    }
}
