using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blueprint.Logic
{
    public abstract class LangFactoryBase : ISupportedFlags
    {
        public static readonly uint CLASS_BUILDER = 0x1;

        public abstract LangWriterBase CreateLangWriter(string filename);
        public abstract LangFileBuilderBase CreateFileBuilder();
        public abstract LangClassBuilderBase CreateClassBuilder();
        public abstract uint GetSupportedFlags();
    }
}
