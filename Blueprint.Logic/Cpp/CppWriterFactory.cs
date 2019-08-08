using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blueprint.Logic
{
    public class CppWriterFactory : LangWriterFactoryBase
    {
        public CppWriterFactory()
        {
        }

        public override UInt32 GetSupportedFlags()
        {
            UInt32 flags = 0;
            flags &= CLASS_BUILDER;

            return flags;
        }

        public override ILangWriter CreateLangWriter(string outDir)
        {
            return new CppWriter(outDir);
        }

        public override LangClassBuilderBase CreateClassBuilder()
        {
            return new CppClassBuilder();
        }
    }
}
