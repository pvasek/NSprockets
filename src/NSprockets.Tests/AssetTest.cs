using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace NSprockets.Tests
{
    [TestFixture]
    public class AssetTest
    {
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
            Assert.AreEqual(false, result4.IsInDirectory("styles"));
            Assert.AreEqual(false, result4.IsInDirectory("jquery.ui"));

            Assert.AreEqual(true, result4.HasName("jquery.1.5.css"));
            Assert.AreEqual(true, result4.HasName("jquery.1.5"));
        }
    }
}
