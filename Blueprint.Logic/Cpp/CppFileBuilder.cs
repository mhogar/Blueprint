using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blueprint.Logic.LangFileBuilder;
using Blueprint.Logic.LangClassBuilder;

namespace Blueprint.Logic.Cpp
{
    public class CppFileBuilder : LangFileBuilderBase, ICreateFileVariable, ICreateFileFunction, ICreateFileClass
    {
        private string _filename;
        private List<VariableObj> _variables;
        private List<FunctionObj> _functions;
        private List<CppClassBuilder> _classes;

        public CppFileBuilder()
        {
            _variables = new List<VariableObj>();
            _functions = new List<FunctionObj>();
            _classes = new List<CppClassBuilder>();
        }

        public override void CreateFile(string filename)
        {
            _filename = filename;
        }

        public void CreateFileVariable(VariableObj variableObj)
        {
            _variables.Add(variableObj);
        }

        public void CreateFileFunction(FunctionObj functionObj)
        {
            _functions.Add(functionObj);
        }

        public void CreateFileClass(LangClassBuilderBase classBuilder, AccessModifier accessModifier)
        {
            CppClassBuilder cppClassBuilder = classBuilder.TryCast<CppClassBuilder>();

            _classes.Add(cppClassBuilder);
        }

        public override void WriteFile()
        {
            var cppWriter = new CppWriter();
            cppWriter.BeginWriter(_filename);
            {
                //write the beginning of the files
                cppWriter.HeaderStream.WriteLine("#pragma once");
                cppWriter.SourceStream.WriteLine($"#include \"{new FilenameInfo(_filename).Basename}.h\"");

                WriterHeaderFile(cppWriter.HeaderStream);
                WriteSourceFile(cppWriter.SourceStream);

                //write any classes
                foreach (CppClassBuilder classBuilder in _classes)
                {
                    cppWriter.HeaderStream.NewLine();
                    classBuilder.WriteClass(cppWriter);
                }
            }
            cppWriter.EndWriter();
        }

        private void WriterHeaderFile(LangStreamWrapper stream)
        {
            stream.NewLine();
            foreach (VariableObj variableObj in _variables)
            {
                CppWriter.WriteVariableString(stream, variableObj);
                stream.WriteLine(";");
            }

            stream.NewLine();
            foreach (FunctionObj functionObj in _functions)
            {
                CppWriter.WriteFunctionString(stream, functionObj);
                stream.WriteLine(";");
            }
        }

        private void WriteSourceFile(LangStreamWrapper stream)
        {
            foreach (FunctionObj functionObj in _functions)
            {
                stream.NewLine();
                CppWriter.WriteFunctionString(stream, functionObj);
                stream.WriteLine("{");
                stream.WriteLine("}");
            }
        }
    }
}
