using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace NSprockets.Tests
{
    public static class TestRunner
    {
        [STAThread]
        public static void Main()
        {
            var args = new string[] { Assembly.GetExecutingAssembly().Location, "/run" };
            NUnit.Gui.AppEntry.Main(args);
        }
    }
}
