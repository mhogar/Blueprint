using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blueprint.Logic.Cpp
{
    public class CppFileBuilder : LangFileBuilderBase
    {
        private List<CppClassBuilder> _classes;

        public CppFileBuilder()
        {
            _classes = new List<CppClassBuilder>();
        }

        public override uint GetSupportedFlags()
        {
            uint flags = 0;
            flags &= FILE_VARAIBLE;
            flags &= FILE_FUNCTION;
            flags &= FILE_CLASS;

            return flags;
        }

        public override void CreateFileVariable(VariableObj variableObj)
        {
            throw new NotImplementedException();
        }

        public override void CreateFileFunction(FunctionObj functionObj)
        {
            throw new NotImplementedException();
        }

        public override void CreateFileClass(LangClassBuilderBase classBuilder)
        {
            var cppClassBuilder = classBuilder as CppClassBuilder;
            if (cppClassBuilder == null)
            {
                throw new InvalidCastException("LangClassBuilderBase was not a CppClassBuilder.");
            }

            _classes.Add(cppClassBuilder);
        }

        public override void WriteFile(ILangWriter langWriter)
        {
            var cppWriter = langWriter as CppWriter;
            if (cppWriter == null)
            {
                throw new InvalidCastException("ILangWriter was not a CppWriter.");
            }

            foreach (CppClassBuilder classBuilder in _classes)
            {
                cppWriter.HeaderStream.NewLine();
                cppWriter.SourceStream.NewLine();
                classBuilder.WriteClass(cppWriter);
            }

            WriterHeaderFile(cppWriter.HeaderStream);
            WriteSourceFile(cppWriter.SourceStream);
        }

        private void WriterHeaderFile(LangStreamWrapper stream)
        {
        }

        private void WriteSourceFile(LangStreamWrapper stream)
        {
        }
    }
}
