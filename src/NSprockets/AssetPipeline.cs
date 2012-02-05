using System.Collections.Generic;
using System.Linq;

namespace NSprockets
{
    public static class AssetPipeline
    {
        static AssetPipeline()
        {
            JavascriptExtensions = new List<string>{"js", "coffee"};
            CssExtensions = new List<string> {"css", "less", "sass"};
        }

        public static List<string> JavascriptExtensions { get; private set; }
        public static List<string> CssExtensions { get; private set; } 

        public static List<string> KnownExtensions
        {
            get { return JavascriptExtensions.Concat(CssExtensions).ToList(); }
        } 
    }
}