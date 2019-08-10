using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blueprint.Logic
{
    public abstract class LangClassBuilderBase : ISupportedFlags
    {
        public static readonly uint CLASS_CONSTRUCTOR = 0X1;
        public static readonly uint CLASS_MEMBER = 0X2;
        public static readonly uint CLASS_FUNCTION = 0X4;
        public static readonly uint CLASS_PROPERTY = 0X8;
        public static readonly uint CLASS_SUB_CLASS = 0X10;

        public abstract uint GetSupportedFlags();
        public abstract void CreateClass(string name);
        public abstract void CreateConstructor(
            List<VariableObj> args, AccessModifier accessModifier = AccessModifier.PUBLIC);
        public abstract void CreateClassMemeber(
            VariableObj variableObj, AccessModifier accessModifier = AccessModifier.PRIVATE);
        public abstract void CreateClassFunction(
            FunctionObj functionObj, AccessModifier accessModifier = AccessModifier.PUBLIC, bool isOverridable = false);
        public abstract void CreateClassProperty(
            VariableObj variableObj, AccessModifier accessModifier = AccessModifier.PRIVATE);
        public abstract void CreateInnerClass(
            LangClassBuilderBase classWriter, AccessModifier accessModifier = AccessModifier.PRIVATE);
        public abstract void WriteClass(LangWriterBase langWriter);
    }
}
