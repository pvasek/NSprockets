using System;
using System.IO;
using NSprockets.Abstract;

namespace NSprockets.Processors
{
    public abstract class ProcessorBase : IAssetProcessor
    {
        private readonly string _extension;

        protected ProcessorBase(string extension)
        {
            _extension = extension;
        }

        public bool IsForExtension(string extension)
        {
            return String.Compare(_extension, extension, StringComparison.OrdinalIgnoreCase) == 0;
        }

        public abstract void Parse(TextReader reader, IProcessorContext context);
    }
}
