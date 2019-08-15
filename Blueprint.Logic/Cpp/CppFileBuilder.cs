using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blueprint.Logic.LangFileBuilder;
using Blueprint.Logic.LangClassBuilder;

namespace Blueprint.Logic.Cpp
{
    public class CppFileBuilder : LangFileBuilderBase, 
        ICreateFileVariable, ICreateFileFunction, ICreateFileClass
    {
        private List<CppClassBuilder> _classes;

        public CppFileBuilder()
        {
            _classes = new List<CppClassBuilder>();
        }

        public void CreateFileVariable(VariableObj variableObj)
        {
            throw new NotImplementedException();
        }

        public void CreateFileFunction(FunctionObj functionObj)
        {
            throw new NotImplementedException();
        }

        public void CreateFileClass(LangClassBuilderBase classBuilder, AccessModifier accessModifier)
        {
            CppClassBuilder cppClassBuilder = classBuilder.TryCast<CppClassBuilder>();

            _classes.Add(cppClassBuilder);
        }

        public override void WriteFile(LangWriterBase langWriter)
        {
            var cppWriter = langWriter as CppWriter;
            if (cppWriter == null)
            {
                throw new ArgumentException("LangWriterBase was not a CppWriter.");
            }

            //write the beginning of the files
            cppWriter.HeaderStream.WriteLine("#pragma once");
            cppWriter.SourceStream.WriteLine($"#include \"{new FilenameInfo(langWriter.Filename).Basename}.h\"");

            //write any classes
            foreach (CppClassBuilder classBuilder in _classes)
            {
                cppWriter.HeaderStream.NewLine();
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
