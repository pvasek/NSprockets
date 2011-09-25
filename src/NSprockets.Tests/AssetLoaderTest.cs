using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.IO;
using NSprockets.Abstract;
using NSprockets.Processors;

namespace NSprockets.Tests
{
    [TestFixture]
    public class AssetLoaderTest
    {
        private List<string> _lookupDirectories;

        [SetUp]
        public void SetUp()
        {
            Uri uri = new Uri(Path.GetDirectoryName(typeof(AssetLoaderTest).Assembly.CodeBase));
            var dir = Path.Combine(uri.AbsolutePath, "assets");
            _lookupDirectories = new List<string>();
            _lookupDirectories.Add(dir);
        }

        [Test]
        public void BasicTest()
        {
            var target = new AssetLoader(_lookupDirectories, new IAssetProcessor[0]);
            Assert.AreEqual(10, target.Assets.Count);
            Assert.AreEqual(1, target.FromDirectory("scripts1.1").Count());
            Assert.AreEqual(4, target.FromTree("scripts1.2").Count());
        }

        [Test]
        public void LoadTest()
        {
            var target = new AssetLoader(_lookupDirectories, new IAssetProcessor[0]);
            Asset result1 = target.Load("test1.a.js");
            Assert.AreEqual(1, result1.Children.Count());

            Asset result2 = target.Load("test1.b.js");
            Assert.AreEqual(5, result2.Children.Count());
        }

        [Test]
        public void GetFilesTest()
        {
            var target = new AssetLoader(_lookupDirectories, new IAssetProcessor[0]);
            var files = target.GetFiles(new string[]{ "test1.b.js" });
            Assert.AreEqual(6, files.Count);
            Assert.AreEqual("test1.b.js", files[0]);
            Assert.AreEqual("test1.1.a.js", files[1]);
            Assert.AreEqual("test1.1.1.a.js", files[2]);
            Assert.AreEqual("test1.1.1.b.js", files[3]);
            Assert.AreEqual("test1.2.a.js", files[4]);
            Assert.AreEqual("test1.2.b.js", files[5]);
        }

        [Test]
        public void GetContentTest()
        {
            var target = new AssetLoader(_lookupDirectories, new IAssetProcessor[0]);
            var content = target.GetContent("test1.b.js");
            var expectedResult = 
@"var test1_b = """";
var test1_1_a = """";
var test1_1_1_a = """";
var test1_1_1_b = """";
var test1_2_a = """";
var test1_2_b = """";";

            Assert.AreEqual(expectedResult, content.Trim());
        }

        [Test]
        public void GetContentTest2()
        {
            var target = new AssetLoader(_lookupDirectories, new[] { new CoffeeScriptProcessor() });
            var content = target.GetContent("test2.a.js");
            Assert.AreEqual("var test;\ntest = 5;", content.Trim());
        }
    }
}
