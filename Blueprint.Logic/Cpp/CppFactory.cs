using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blueprint.Logic.Cpp
{
    public class CppFactory : LangFactoryBase
    {
        public override uint GetSupportedFlags()
        {
            uint flags = 0;
            flags &= CLASS_BUILDER;

            return flags;
        }

        public override ILangWriter CreateLangWriter(string outDir)
        {
            return new CppWriter(outDir);
        }

        public override LangFileBuilderBase CreateFileBuilder()
        {
            return new CppFileBuilder();
        }

        public override LangClassBuilderBase CreateClassBuilder()
        {
            return new CppClassBuilder();
        }
    }
}
