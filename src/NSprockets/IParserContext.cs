using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSprockets
{
    public interface IParserContext
    {
        void AddDirective(string directiveText);
    }
}
