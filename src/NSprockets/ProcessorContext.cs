using System.IO;
using NSprockets.Abstract;

namespace NSprockets
{
    public class ProcessorContext: IProcessorContext
    {
        private readonly StringWriter _output = new StringWriter();
        public TextWriter Output { get { return _output; } }
    }
}
