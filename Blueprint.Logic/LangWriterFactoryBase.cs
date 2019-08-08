using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blueprint.Logic
{
    public abstract class LangWriterFactoryBase : ISupportedFlags
    {
        public static readonly UInt32 CLASS_BUILDER = 0x1;

        public LangWriterFactoryBase()
        {
        }

        public abstract ILangWriter CreateLangWriter(string outDir);
        public abstract LangClassBuilderBase CreateClassBuilder();
        public abstract UInt32 GetSupportedFlags();
    }
}
