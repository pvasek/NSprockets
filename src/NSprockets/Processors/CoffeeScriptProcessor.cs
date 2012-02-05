using JurassicCoffee.Core;
using NSprockets.Abstract;

namespace NSprockets.Processors
{
    public class CoffeeScriptProcessor : ProcessorBase
    {
        private readonly CoffeeCompiler _compiler = new CoffeeCompiler();
        private readonly object _syncRoot = new object();

        public CoffeeScriptProcessor() : base("coffee"){}
        
        public override void Parse(System.IO.TextReader reader, IProcessorContext context)
        {
            lock (_syncRoot)
            {
                context.Output.Write(_compiler.CompileString(reader.ReadToEnd()));
            }
        }

        public override DirectiveParser Parser
        {
            get { return new DirectiveParser("#=="); }
        }
    }
}
