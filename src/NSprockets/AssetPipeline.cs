using System.Collections.Generic;
using System.Linq;
using System.Web;
using NSprockets.Abstract;
using NSprockets.Processors;

namespace NSprockets
{
    public static class AssetPipeline
    {
        static AssetPipeline()
        {
            JavascriptExtensions = new List<string>{"js", "coffee"};
            CssExtensions = new List<string> {"css", "less", "sass"};
            Processors = new List<IAssetProcessor>
                             {
                                 new JsProcessor(),
                                 new CoffeeScriptProcessor()
                             };
            LookupDirectories = new List<string>();
        }

        public static List<string> JavascriptExtensions { get; private set; }
        public static List<string> CssExtensions { get; private set; } 
        public static List<string> LookupDirectories { get; private set; }

        public static List<string> KnownExtensions
        {
            get { return JavascriptExtensions.Concat(CssExtensions).ToList(); }
        }

        public static List<IAssetProcessor> Processors { get; private set; }

        public static bool MinifyJs { get; set; }

        public static void AddServerLookupDirectory(string virtualPath)
        {
            LookupDirectories.Add(HttpContext.Current.Server.MapPath(virtualPath));
        }


    }
}