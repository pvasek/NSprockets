using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace NSprockets.Tests
{
    [TestFixture]
    public class ParserContextTest
    {
        [Test]
        public void AddDirecitveTest()
        {
            var target = new ParserContext("current.js");
            target.AddDirective("require_self");
            target.AddDirective("require test1.js");
            target.AddDirective("require test2.js test3.js");
            target.AddDirective("require_tree tree1");
            target.AddDirective("require_tree tree2 tree3");
            Assert.AreEqual(7, target.Directives.Count);
            Assert.AreEqual("current.js", target.Directives[0].Parameter);
            Assert.AreEqual(DirectiveType.Require, target.Directives[0].Type);
            Assert.AreEqual("test1.js", target.Directives[1].Parameter);
            Assert.AreEqual("test2.js", target.Directives[2].Parameter);
            Assert.AreEqual("test3.js", target.Directives[3].Parameter);
            Assert.AreEqual("tree1", target.Directives[4].Parameter);
            Assert.AreEqual("tree2", target.Directives[5].Parameter);
            Assert.AreEqual("tree3", target.Directives[6].Parameter);
        }
    }
}
