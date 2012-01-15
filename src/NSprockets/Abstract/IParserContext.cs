using System.IO;

namespace NSprockets.Abstract
{
    public interface IParserContext
    {
        void AddDirective(string directiveText);
        StringWriter FilteredContent { get; }
    }
}
