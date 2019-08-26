using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blueprint.Logic.LangClassBuilder
{
    public abstract class LangClassBuilderBase : TryCastBase
    {
        public abstract void CreateClass(string name);
        public abstract void WriteClass(LangWriterBase langWriter);
    }

    public interface ICreateClassConstructor
    {
        void CreateClassConstructor(
            List<VariableObj> constructorParams, AccessModifier accessModifier = AccessModifier.PUBLIC);
    }

    public interface ICreateClassMemeber
    {
        void CreateClassMemeber(
            VariableObj variableObj, AccessModifier accessModifier = AccessModifier.PRIVATE);
    }

    public interface ICreateClassFunction
    {
        void CreateClassFunction(
            FunctionObj functionObj, AccessModifier accessModifier = AccessModifier.PUBLIC, bool isOverridable = false);
    }

    public interface ICreateClassProperty
    {
        void CreateClassProperty(
            VariableObj variableObj, AccessModifier accessModifier = AccessModifier.PRIVATE);
    }

    public interface ICreateInnerClass
    {
        void CreateInnerClass(
            LangClassBuilderBase classBuilder, AccessModifier accessModifier = AccessModifier.PRIVATE);
    }
}
