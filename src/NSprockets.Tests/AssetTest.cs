using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public void JsAsset1Test()
        {
            Asset result1 = new Asset(null, "C:/app1/assets/jquery.ui/jquery.1.5.js", "C:/app1/assets/");
            Assert.AreEqual("C:/app1/assets/jquery.ui/jquery.1.5.js", result1.FullFileName);
            Assert.AreEqual("jquery.ui", result1.Directory);
            Assert.AreEqual(true, result1.IsInTree("jquery.ui"));
            Assert.AreEqual(false, result1.IsInTree("jquery"));
            Assert.AreEqual(false, result1.IsInTree("jquery.ui.styles"));
            Assert.AreEqual("jquery.1.5", result1.Name);
            Assert.AreEqual(".js", result1.Extension);
            Assert.AreEqual(AssetType.Js, result1.Type);

        }
        
        [Test]
        public void JsAsset2Test()
        {
            Asset result2 = new Asset(null, "C:/app1/assets/jquery.ui/jquery.1.5.js", "C:/app1/assets");
            Assert.AreEqual("jquery.ui", result2.Directory);

            Asset result3 = new Asset(null, "C:/app1/assets/jquery.ui/jquery.1.5.js.coffee", "C:/app1/assets/");
            Assert.AreEqual("C:/app1/assets/jquery.ui/jquery.1.5.js.coffee", result3.FullFileName);
            Assert.AreEqual("jquery.ui", result3.Directory);
            Assert.AreEqual("jquery.1.5", result3.Name);
            Assert.AreEqual(".js.coffee", result3.Extension);
            Assert.AreEqual(AssetType.Js, result3.Type);
        }
        
        [Test]
        public void CssAsset1Test()
        {
            Asset result4 = new Asset(null, "C:/app1/assets/jquery.ui/styles/jquery.1.5.css", "C:/app1/assets");
            Assert.AreEqual("jquery.ui/styles", result4.Directory);
            Assert.AreEqual(AssetType.Css, result4.Type);
            Assert.AreEqual(true, result4.IsInTree("jquery.ui"));
            Assert.AreEqual(true, result4.IsInTree("styles"));
            Assert.AreEqual(true, result4.IsInTree("jquery.ui/styles"));

            Assert.AreEqual(true, result4.IsInDirectory("jquery.ui/styles"));
            Assert.AreEqual(true, result4.IsInDirectory("styles"));
            Assert.AreEqual(false, result4.IsInDirectory("jquery.ui"));

            Assert.AreEqual(true, result4.HasName("jquery.1.5.css"));
            Assert.AreEqual(true, result4.HasName("jquery.1.5"));
        }

        [Test]
        public void LoadTest1()
        {
            Asset target = new Asset(null, Path.Combine(_assetsRootDir, "scripts1/scripts1.2/test1.2.b.js"), _assetsRootDir);
            target.Load();
            Assert.AreEqual("var test1_2_b = \"\";", target.Content.Trim());
        }

        [Test]
        public void LoadTest2()
        {
            // this doesn't work, I have no idea why
            //var processorMock = new Mock<IAssetProcessor>();
            //processorMock.Setup(i => i.IsForExtension(".coffee")).Returns(true);
            //processorMock.Setup(i => i.Parse(null, null))
            //    .Callback(delegate (TextReader input, IProcessorContext context){
            //        context.Output.Write("coffee");
            //    });
            var processorMock = new MockProcessor();

            var assetLoaderMock = new Mock<IAssetLoader>();
            assetLoaderMock.Setup(i => i.FindProcessor(".coffee")).Returns(processorMock);
            Asset target = new Asset(assetLoaderMock.Object, Path.Combine(_assetsRootDir, "scripts2/test2.a.js.coffee"), _assetsRootDir);
            target.Load();
            Assert.AreEqual("coffee", target.Content.Trim());
 
        }

        private class MockProcessor: IAssetProcessor
        {
            public bool IsForExtension(string extension)
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
