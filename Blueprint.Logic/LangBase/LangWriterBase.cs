using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blueprint.Logic
{
    public abstract class LangWriterBase
    {
        public LangWriterBase(string filename)
        {
            Filename = filename;
        }

        public string Filename
        {
            get;
        }

        public abstract void BeginWriter();
        public abstract void EndWriter();
    }
}
