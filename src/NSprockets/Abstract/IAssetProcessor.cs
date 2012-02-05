using System.IO;

namespace NSprockets.Abstract
{
    public interface IAssetProcessor
    {
        bool IsForFile(AssetFile file);
        void Parse(TextReader reader, IProcessorContext context);
        DirectiveParser Parser { get; }
    }
}
