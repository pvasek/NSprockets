using System;
using System.IO;
using System.Linq;
using NSprockets.Abstract;

namespace NSprockets.Processors
{
    public abstract class ProcessorBase : IAssetProcessor
    {
        private readonly string _extension;

        protected ProcessorBase(string extension)
        {
            _extension = extension.NormalizePath();
        }

        public bool IsForFile(AssetFile file)
        {
            return file.Extensions.Any(i => i == _extension);
        }

        public abstract void Parse(TextReader reader, IProcessorContext context);
    }
}
