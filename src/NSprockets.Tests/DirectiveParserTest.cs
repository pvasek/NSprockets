using System;
using System.Text;
using NUnit.Framework;
using System.IO;
using NSprockets;

namespace NSprockets.Tests
{
    [TestFixture]
    public class DirectiveParserTest
    {
        [Test]
        public void ProcessTest()
        {
            var target = new DirectiveParser("*=");
            
            string content = @"
*= require test1.css
 *= require test2
body {}
h1 {}
*= require test3  ";
            string result = @"body {}
h1 {}";
            var context = new TestParserContext();
            target.Parse(new StringReader(content), context);

            Assert.AreEqual(3, context.Directives.Count);            
        }
    }

}
