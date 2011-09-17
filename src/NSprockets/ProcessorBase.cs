using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSprockets
{
    public class ProcessorBase
    {
        private string _extension;

        public ProcessorBase(string extension)
        {
            _extension = extension;
        }

        public bool IsForExtension(string extension)
        {
            return String.Compare(_extension, extension, true) == 0;
        }

    }
}
