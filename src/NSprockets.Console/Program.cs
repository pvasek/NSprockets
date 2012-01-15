using System;
using System.IO;
using System.Linq;

namespace NSprockets.Console
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                System.Console.WriteLine("Usage nsproc.exe <input file> [<output directory>] [-minify]");
                return;
            }
            string inputFile = args[0];
            bool minify = args.Any(i => string.Compare(i, "-minify", StringComparison.InvariantCultureIgnoreCase) == 0);

            var tool = new NSprocketsTool();            
            tool.OutputDirectory = args.Length > 1 ? args[1] : Environment.CurrentDirectory;
            tool.LookupDirectories.Add(Path.GetDirectoryName(inputFile));
            tool.Minify = minify;
            tool.Generate(Path.GetFileName(inputFile));
        }
    }
}