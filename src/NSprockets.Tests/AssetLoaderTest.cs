using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using System.IO;
using NSprockets.Abstract;

namespace NSprockets.Tests
{
    // ReSharper disable InconsistentNaming

    [TestFixture]
    public class AssetLoaderTest
    {
        private List<string> _lookupDirectories;

        [SetUp]
        public void SetUp()
        {
            var uri = new Uri(Path.GetDirectoryName(typeof(AssetLoaderTest).Assembly.CodeBase));
            var dir1 = Path.Combine(uri.AbsolutePath, Path.Combine("assets", "scripts1"));
            var dir2 = Path.Combine(uri.AbsolutePath, Path.Combine("assets", "scripts2"));
            _lookupDirectories = new List<string>();
            _lookupDirectories.Add(dir1);
            _lookupDirectories.Add(dir2);            
        }

        [Test]
        public void Can_load_assets_from_directories()
        {
            var target = new AssetLoader(_lookupDirectories);
            Assert.AreEqual(10, target.Assets.Count);
            Assert.AreEqual(1, target.FromDirectory("scripts1.1").Count());
            Assert.AreEqual(4, target.FromTree("scripts1.2").Count());
        }

        [Test]
        public void Can_load_assest_from_single_file()
        {
            var target = new AssetLoader(_lookupDirectories);
            Asset result1 = target.Load("test1.a.js");
            Assert.AreEqual(2, result1.Children.Count()); // one real + one from require_self

            Asset result2 = target.Load("test1.b.js");
            Assert.AreEqual(5, result2.Children.Count());
        }

        [Test]
        public void Get_all_file_names_for_asset()
        {
            var target = new AssetLoader(_lookupDirectories);
            var files = target.GetFiles(new []{ "test1.b.js" });
            Assert.AreEqual(6, files.Count);
            Assert.AreEqual("test1.b.js", Path.GetFileName(files[0]));
            Assert.AreEqual("test1.1.a.js", Path.GetFileName(files[1]));
            Assert.AreEqual("test1.1.1.a.js", Path.GetFileName(files[2]));
            Assert.AreEqual("test1.1.1.b.js", Path.GetFileName(files[3]));
            Assert.AreEqual("test1.2.a.js", Path.GetFileName(files[4]));
            Assert.AreEqual("test1.2.b.js", Path.GetFileName(files[5]));
        }

        [Test]
        public void Doesnt_get_files_for_asset_which_is_not_on_lookup_path()
        {
            var target = new AssetLoader(_lookupDirectories);
            var files = target.GetFiles(new [] { "test1.1.a.js" });
            Assert.AreEqual(0, files.Count);
        }

        [Test]
        public void Get_all_content_for_asset()
        {
            var target = new AssetLoader(_lookupDirectories);
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
        [Explicit]
        public void Get_all_content_for_asset_which_contains_coffeescript()
        {
            var target = new AssetLoader(_lookupDirectories);
            var content = target.GetContent("test2.a.js");
            Assert.AreEqual("var test;\ntest = 5;", content.Trim());
        }

        [Test]
        public void Get_all_content_for_asset_containing_require_self()
        {
            var target = new AssetLoader(_lookupDirectories);
            var content = target.GetContent("test1.a.js");
            Assert.AreEqual("var test1_1_1_a = \"\";var test1_a = \"\";", content.Trim().Replace("\n", "").Replace("\r", ""));
        }

        [Test]
        public void Get_all_files_for_asset_containing_require_self()
        {
            var target = new AssetLoader(_lookupDirectories);
            var files = target.GetFiles("test1.a.js");
            Assert.AreEqual(2, files.Count);
            Assert.AreEqual("test1.1.1.a.js", Path.GetFileName(files[0]));
            Assert.AreEqual("test1.a.js", Path.GetFileName(files[1]));
        }

        [Test]
        public void Get_all_content_for_asset_using_a_test_processor()
        {
            AssetPipeline.Processors.Add(new TestAssetProcessor());

            var target = new AssetLoader(_lookupDirectories);
            var content = target.GetContent("test1.a.js");
            Assert.AreEqual("AA", content);
        }

        private class TestAssetProcessor : IAssetProcessor
        {
            public bool IsForFile(AssetFile file)
            {
                return file.Extension == ".js";
            }

            public void Parse(TextReader reader, IProcessorContext context)
            {
                context.Output.Write("A");
            }

            public DirectiveParser Parser
            {
                get { return DirectiveParser.JsParser; }
            }
        }
    }
    // ReSharper restore InconsistentNaming
}
