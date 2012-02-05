using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using NSprockets.Abstract;
using NSprockets.Processors;

namespace NSprockets
{
    public class NSprocketsTool
    {

        #region Private Members 

        private readonly object _syncRoot = new object();
        private readonly Dictionary<string, string> _fileMapping = new Dictionary<string, string>(); 

        #endregion

        #region Private Methods

        private AssetLoader GetLoader()
        {
            return new AssetLoader(AssetPipeline.LookupDirectories);
        }

        #endregion

        private static bool GetConfigurationAsBool(string key, bool defaultValue)
        {
            string value = ConfigurationManager.AppSettings[key];
            if (value == null)
            {
                return defaultValue;
            }
            return value == "1" || String.Compare(value, "true", StringComparison.CurrentCultureIgnoreCase) == 0;
        }

        private static string GetConfigurationAsString(string key, string defaultValue)
        {
            return ConfigurationManager.AppSettings[key] ?? defaultValue;
        }

        static NSprocketsTool()
        {
            Current = new NSprocketsTool();
            if (HttpContext.Current != null)
            {                
                Current.ApplicationRootDirectory = HttpContext.Current.Server.MapPath("~/");
                Current.ConcatToSingleFile = GetConfigurationAsBool("NSprockets.ConcatToSingleFile", true);
                AssetPipeline.MinifyJs = GetConfigurationAsBool("NSprockets.Minify", true);
                Current.SetWebOutputDirectory(GetConfigurationAsString("NSprockets.OutputDirectory", "~/scripts"));
                foreach (string dir in GetConfigurationAsString("NSprockets.LookupDirectories", "~/scripts").Split(','))
                {
                    Current.AddServerLookupDirectory(dir);
                }                
            }
        }

        public static NSprocketsTool Current { get; set; }

        public string OutputDirectory { get; set; }        
        public bool ConcatToSingleFile { get; set; }

        private string _applicationRootDirectory;
        public string ApplicationRootDirectory
        {
            get { return _applicationRootDirectory; }
            set
            {
                _applicationRootDirectory = value.Replace("\\", "/");
            }
        }

        public void AddServerLookupDirectory(string virtualPath)
        {
            AssetPipeline.AddServerLookupDirectory(virtualPath);
        }

        public void SetWebOutputDirectory(string virtualPath)
        {
            OutputDirectory = HttpContext.Current.Server.MapPath(GetConfigurationAsString("NSprockets.OutputDirectory", "~/scripts"));
        }

        public IHtmlString GetScriptFileDeclaration(params string[] files)
        {
            var scriptFiles = ConcatToSingleFile
                                  ? files.Select(GetPreprocessedFileName)
                                  : GetLoader().GetFiles(files).Select(MapToApplicationPath);

            var sb = new StringBuilder();
            foreach (var file in scriptFiles)
            {
                sb.Append("<script type=\"text/javascript\" src=\"");
                sb.Append(file);
                sb.Append("\"></script>");
            }            
            return new HtmlString(sb.ToString());
        }

        private string MapToApplicationPath(string fileName)
        {
            return fileName.Replace(ApplicationRootDirectory, "/");
        }

        private string GetPreprocessedFileName(string fileName)
        {
            lock (_syncRoot)
            {
                string result;
                if (!_fileMapping.TryGetValue(fileName, out result))
                {
                    string content = GetLoader().GetContent(fileName);
                    result = Utils.GetOutputFileName(fileName, OutputDirectory, Utils.GetHash(content));
                    result = result.Replace("\\", "/").Replace(ApplicationRootDirectory, "/");
                    _fileMapping[fileName] = result;
                }
                return result;
            }
        }

        public void Generate(string inputFile)
        {
            string content = GetLoader().GetContent(inputFile);
            string hash = Utils.GetHash(content);
            var outputFile = Utils.GetOutputFileName(inputFile, OutputDirectory, hash);
            File.WriteAllText(outputFile, content);
        }
    }

}
