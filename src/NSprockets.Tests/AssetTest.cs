using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.IO;
using Moq;
using NSprockets.Abstract;

namespace NSprockets.Tests
{
    [TestFixture]
    public class AssetTest
    {
        [SetUp]
        public void SetUp()
        {
            Uri uri = new Uri(Path.GetDirectoryName(typeof(AssetTest).Assembly.CodeBase));
            _assetsRootDir = Path.Combine(uri.AbsolutePath, "assets");
        }

        private string _assetsRootDir; 
        
        [Test]
        public void LoadTest1()
        {
            var assetLoaderMock = new Mock<IAssetLoader>();
            assetLoaderMock.Setup(i => i.FindProcessors(It.IsAny<AssetFile>()))
                .Returns(new List<IAssetProcessor>());
            
            var target = new Asset(assetLoaderMock.Object, Path.Combine(_assetsRootDir, "scripts1/scripts1.2/test1.2.b.js"), _assetsRootDir);
            target.Load();
            Assert.AreEqual("var test1_2_b = \"\";", target.Content.Trim());
        }

        [Test]
        public void LoadTest2()
        {
            var processorMock = new MockProcessor();
            var assetLoaderMock = new Mock<IAssetLoader>();
            assetLoaderMock.Setup(i => i.FindProcessors(It.IsAny<AssetFile>()))
                .Returns(new List<IAssetProcessor>{processorMock});
            Asset target = new Asset(assetLoaderMock.Object, Path.Combine(_assetsRootDir, "scripts2/test2.a.js.coffee"), _assetsRootDir);
            target.Load();
            Assert.AreEqual("coffee", target.Content.Trim());
 
        }

        private class MockProcessor: IAssetProcessor
        {
            public bool IsForFile(AssetFile file)
            {
                return true;
            }

            public void Parse(TextReader reader, IProcessorContext context)
            {
                context.Output.Write("coffee");
            }
        }
    }
}
