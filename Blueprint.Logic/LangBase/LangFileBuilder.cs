using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blueprint.Logic.LangClassBuilder;

namespace Blueprint.Logic.LangFileBuilder
{
    public abstract class LangFileBuilderBase : Foo
    {
        public abstract void WriteFile(LangWriterBase langWriter);
    }

    public interface ICreateFileVariable
    {
        void CreateFileVariable(VariableObj variableObj);
    }

    public interface ICreateFileFunction
    {
        void CreateFileFunction(FunctionObj functionObj);
    }

    public interface ICreateFileClass
    {
        void CreateFileClass(LangClassBuilderBase classBuilder, AccessModifier accessModifier = AccessModifier.PUBLIC);
    }
}
