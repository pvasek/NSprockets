using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NSprockets.Abstract
{
    public interface IAssetProcessor
    {
        bool IsForExtension(string extension);
        void Parse(TextReader reader, IProcessorContext context);
    }
}
