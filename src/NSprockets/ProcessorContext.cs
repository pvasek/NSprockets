using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NSprockets.Abstract;

namespace NSprockets
{
    public class ProcessorContext: IProcessorContext
    {
        private StringWriter _output = new StringWriter();
        public TextWriter Output { get { return _output; } }
    }
}
