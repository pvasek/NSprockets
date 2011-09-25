using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NSprockets.Abstract
{
    public interface IParserContext
    {
        void AddDirective(string directiveText);
        StringWriter FilteredContent { get; }
    }
}
