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
            Asset result1 = new Asset("C:/app1/assets/jquery.ui/jquery.1.5.js", "C:/app1/assets/");
            Assert.AreEqual("C:/app1/assets/jquery.ui/jquery.1.5.js", result1.FileName);
            Assert.AreEqual("jquery.ui", result1.DirectoryPath);
            Assert.AreEqual(true, result1.IsIn("jquery.ui"));
            Assert.AreEqual(false, result1.IsIn("jquery"));
            Assert.AreEqual(false, result1.IsIn("jquery.ui.styles"));
            Assert.AreEqual("jquery.1.5", result1.Name);
            Assert.AreEqual(".js", result1.Extension);
            Assert.AreEqual(AssetType.Js, result1.Type);

        }
        
        [Test]
        public void JsAsset2Test()
        {
            Asset result2 = new Asset("C:/app1/assets/jquery.ui/jquery.1.5.js", "C:/app1/assets");
            Assert.AreEqual("jquery.ui", result2.DirectoryPath);

            Asset result3 = new Asset("C:/app1/assets/jquery.ui/jquery.1.5.js.coffee", "C:/app1/assets/");
            Assert.AreEqual("C:/app1/assets/jquery.ui/jquery.1.5.js.coffee", result3.FileName);
            Assert.AreEqual("jquery.ui", result3.DirectoryPath);
            Assert.AreEqual("jquery.1.5", result3.Name);
            Assert.AreEqual(".js.coffee", result3.Extension);
            Assert.AreEqual(AssetType.Js, result3.Type);
        }
        
        [Test]
        public void CssAsset1Test()
        {
            Asset result4 = new Asset("C:/app1/assets/jquery.ui/styles/jquery.1.5.css", "C:/app1/assets");
            Assert.AreEqual("jquery.ui/styles", result4.DirectoryPath);
            Assert.AreEqual(AssetType.Css, result4.Type);
            Assert.AreEqual(true, result4.IsIn("jquery.ui"));
            Assert.AreEqual(true, result4.IsIn("styles"));
            Assert.AreEqual(true, result4.IsIn("jquery.ui/styles"));
        }
    }
}
