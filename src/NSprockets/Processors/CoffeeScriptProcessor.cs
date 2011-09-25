using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JurassicCoffee.Core;
using NSprockets.Abstract;

namespace NSprockets.Processors
{
    public class CoffeeScriptProcessor: IAssetProcessor
    {
        private CoffeeCompiler _compiler = new CoffeeCompiler();
        private object _syncRoot = new object();

        public bool IsForExtension(string extension)
        {
            return String.Compare(".coffee", extension, true) == 0;
        }

        public void Parse(System.IO.TextReader reader, IProcessorContext context)
        {
            lock (_syncRoot)
            {
                context.Output.Write(_compiler.CompileString(reader.ReadToEnd()));
            }
        }
    }
}
