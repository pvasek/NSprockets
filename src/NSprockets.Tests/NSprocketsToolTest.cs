using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.IO;

namespace NSprockets.Tests
{
    [TestFixture]
    public class NSprocketsToolTest
    {
        [Test]
        public void InitializeTest()
        {
            var target = new NSprocketsTool();
            Uri uri = new Uri(Path.GetDirectoryName(typeof(NSprocketsToolTest).Assembly.CodeBase));
            var dir = Path.Combine(uri.AbsolutePath, "assets");
            target.LookupDirectories.Add(dir);
            target.Initialize();
            Assert.AreEqual(5, target.Directories.Count);
            Assert.AreEqual(6, target.Files.Count);
        }
    }
}
