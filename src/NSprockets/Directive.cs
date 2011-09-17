using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSprockets
{
    public class Directive
    {
        public Directive(DirectiveType type, string parameter)
        {
            Type = type;
            Parameter = parameter;
        }

        public DirectiveType Type { get; private set; }
        public string Parameter { get; private set; }
    }
}
