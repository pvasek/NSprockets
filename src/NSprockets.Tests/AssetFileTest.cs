using NSprockets.Abstract;
using NUnit.Framework;

namespace NSprockets.Tests
{
    // ReSharper disable InconsistentNaming

    [TestFixture]
    public class AssetFileTest
    {
        [Test]
        public void Can_be_created_from_js_file_name()
        {
            var target = new AssetFile(@"C:\Project\path1\Asset1.js", @"C:\project");
            Assert.AreEqual(@"C:\Project\path1\Asset1.js", target.OriginalFile);
            Assert.AreEqual(@"c:/project/path1/asset1.js", target.File);
            Assert.AreEqual(@"path1", target.Path);
            Assert.AreEqual("path1/asset1", target.Name);
            Assert.AreEqual(".js", target.Extension);
            Assert.AreEqual(true, target.HasName("path1/asset1"));
            Assert.AreEqual(true, target.HasName("path1/asset1.js"));
            Assert.AreEqual(false, target.HasName("path1/asset1.js.coffee"));
            Assert.AreEqual(AssetType.Js, target.Type);            
        }

        [Test]
        public void Can_be_created_from_coffee_file_name()
        {
            var target = new AssetFile(@"C:\Project\path1\Path1_a\Asset1.coffee", @"C:\project");
            Assert.AreEqual(@"C:\Project\path1\Path1_a\Asset1.coffee", target.OriginalFile);
            Assert.AreEqual(@"c:/project/path1/path1_a/asset1.coffee", target.File);
            Assert.AreEqual("path1/path1_a", target.Path);
            Assert.AreEqual("path1/path1_a/asset1", target.Name);
            Assert.AreEqual(".coffee", target.Extension);
            Assert.AreEqual(AssetType.Js, target.Type);
        }

        [Test]
        public void Can_tell_if_it_is_in_the_given_tree()
        {
            var target = new AssetFile(@"C:\Project\path1\a\a1\asset1.js", @"C:\project");
            // ideal paths
            Assert.AreEqual(true, target.IsInTree("path1"));
            Assert.AreEqual(true, target.IsInTree("path1/a"));
            Assert.AreEqual(true, target.IsInTree("path1/a/a1"));
            // not-ideal paths
            Assert.AreEqual(true, target.IsInTree(@"Path1\"));
            Assert.AreEqual(true, target.IsInTree(@"path1\A"));
            Assert.AreEqual(true, target.IsInTree(@"path1/A\A1"));
            // wrong paths
            Assert.AreEqual(false, target.IsInTree("a\a1"));
            Assert.AreEqual(false, target.IsInTree(@"path2"));
            Assert.AreEqual(false, target.IsInTree(@"Projects"));
            Assert.AreEqual(false, target.IsInTree("path1/a/a1/asset1"));
        }
    }

    // ReSharper restore InconsistentNaming
}