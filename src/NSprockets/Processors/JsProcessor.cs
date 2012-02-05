using System.IO;
using NSprockets.Abstract;
using Yahoo.Yui.Compressor;

namespace NSprockets.Processors
{
    public class JsProcessor: ProcessorBase
    {
        public JsProcessor() : base("js") { }

        public override void Parse(TextReader reader, IProcessorContext context)
        {
            if (AssetPipeline.MinifyJs)
            {
                var compressor = new JavaScriptCompressor(reader.ReadToEnd());
                context.Output.Write(compressor.Compress());
            }
            else
            {
                context.Output.Write(reader.ReadToEnd());               
            }            
        }

        public override DirectiveParser Parser
        {
            get { return new DirectiveParser("//=");}
        }
    }
}