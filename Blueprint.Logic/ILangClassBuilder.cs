using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blueprint.Logic
{
    public interface ILangClassBuilder
    {
        void CreateClass(string name, AccessModifier accessModifier);
        void CreateConstructor(List<VariableObj> args, AccessModifier accessModifier);
        void CreateClassMemeber(VariableObj variableObj, AccessModifier accessModifier);
        void CreateClassFunction(FunctionObj functionObj, AccessModifier accessModifier, bool isOverridable);
        void CreateClassProperty(VariableObj variableObj, AccessModifier accessModifier);
        void CreateSubClass(ILangClassBuilder classWriter, AccessModifier accessModifier);
        void WriteClass(ILangWriter langWriter);
    }
}
