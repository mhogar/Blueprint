using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blueprint.Logic.LangFactory;
using Blueprint.Logic.LangFileBuilder;
using Blueprint.Logic.LangClassBuilder;

namespace Blueprint.Logic.Cpp
{
    public class CppFactory : LangFactoryBase, ICreateFileBuilder, ICreateClassBuilder
    {
        public CppFactory() : base("Cpp")
        {
        }

        public override LangWriterBase CreateLangWriter(string filename)
        {
            return new CppWriter(filename);
        }

        public LangFileBuilderBase CreateFileBuilder()
        {
            return new CppFileBuilder();
        }

        public LangClassBuilderBase CreateClassBuilder()
        {
            return new CppClassBuilder();
        }
    }
}
