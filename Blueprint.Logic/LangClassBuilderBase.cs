using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blueprint.Logic
{
    public abstract class LangClassBuilderBase : ISupportedFlags
    {
        public static readonly UInt32 CLASS_CONSTRUCTOR = 0X1;
        public static readonly UInt32 CLASS_MEMBER = 0X2;
        public static readonly UInt32 CLASS_FUNCTION = 0X4;
        public static readonly UInt32 CLASS_PROPERTY = 0X8;
        public static readonly UInt32 CLASS_SUB_CLASS = 0X10;

        public abstract void CreateClass(string name, AccessModifier accessModifier);
        public abstract void CreateConstructor(List<VariableObj> args, AccessModifier accessModifier);
        public abstract void CreateClassMemeber(VariableObj variableObj, AccessModifier accessModifier);
        public abstract void CreateClassFunction(FunctionObj functionObj, AccessModifier accessModifier, bool isOverridable);
        public abstract void CreateClassProperty(VariableObj variableObj, AccessModifier accessModifier);
        public abstract void CreateSubClass(LangClassBuilderBase classWriter, AccessModifier accessModifier);
        public abstract void WriteClass(ILangWriter langWriter);
        public abstract UInt32 GetSupportedFlags();
    }
}
