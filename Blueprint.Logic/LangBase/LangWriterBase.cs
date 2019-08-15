using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blueprint.Logic
{
    public abstract class LangWriterBase : Foo
    {
        public abstract void BeginWriter(string filename);
        public abstract void EndWriter();
    }
}
