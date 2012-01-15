using NUnit.Framework;

namespace NSprockets.Tests
{
    [TestFixture]
    public class UtilsTest
    {
        [Test]
        public void GetHashTest()
        {
            const string content = "The quick brown fox jumps over the lazy dog";
            Assert.AreEqual("9e107d9d372bb6826bd81d3542a419d6", Utils.GetHash(content));
        }

        [Test]
        public void GetOutputFileNameTest()
        {
            const string outputPath = @"C:\test";
            const string inputFile = @"C:\test\application.js";
            Assert.AreEqual(@"C:\test\application_ffff.js", Utils.GetOutputFileName(inputFile, outputPath, "ffff"));
        }
    }
}