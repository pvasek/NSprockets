using System.IO;
using NSprockets.Abstract;
using Yahoo.Yui.Compressor;

namespace NSprockets.Processors
{
    public class JsMinifierProcessor: ProcessorBase
    {
        public JsMinifierProcessor() : base(".js") { }

        public override void Parse(TextReader reader, IProcessorContext context)
        {
            var compressor = new JavaScriptCompressor(reader.ReadToEnd()); 
            context.Output.Write(compressor.Compress());
            
        }
    }
}