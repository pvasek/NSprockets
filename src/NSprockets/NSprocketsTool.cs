using System.Collections.Generic;
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

        public NSprocketsTool()
        {
            LookupDirectories = new List<string>();
            Processors = new List<IAssetProcessor>();
            Processors.Add(new CoffeeScriptProcessor());
        }

        #region Private Members 

        private readonly object _syncRoot = new object();
        private readonly Dictionary<string, string> _fileMapping = new Dictionary<string, string>(); 

        #endregion

        #region Private Methods

        private AssetLoader GetLoader()
        {
            return new AssetLoader(LookupDirectories, GetAllProcessors());
        }

        #endregion

        static NSprocketsTool()
        {
            Current = new NSprocketsTool();
            if (HttpContext.Current != null)
            {
                Current.AddServerLookupDirectory("~/scripts");
                Current.ApplicationRootDirectory = HttpContext.Current.Server.MapPath("~/");
                Current.OutputDirectory = HttpContext.Current.Server.MapPath("~/scripts");
            }
        }

        private IEnumerable<IAssetProcessor> GetAllProcessors()
        {
            if (Minify)
            {
                yield return new JsMinifierProcessor();
            }
            foreach (var processor in Processors)
            {
                yield return processor;
            }
        }

        public static NSprocketsTool Current { get; set; }

        public List<string> LookupDirectories { get; private set; }
        public string OutputDirectory { get; set; }        
        public List<IAssetProcessor> Processors { get; private set; }
        public bool ConcatToSingleFile { get; set; }
        public bool Minify { get; set; }       

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
            LookupDirectories.Add(HttpContext.Current.Server.MapPath(virtualPath));
        }

        public string GetScriptFileDeclaration(params string[] files)
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
            return sb.ToString();
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
