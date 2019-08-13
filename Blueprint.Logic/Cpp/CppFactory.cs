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

        public override LangWriterBase CreateLangWriter(string filename)
        {
            return new CppWriter(filename);
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
