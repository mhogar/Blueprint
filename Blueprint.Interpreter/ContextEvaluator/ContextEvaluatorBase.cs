using Blueprint.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blueprint.Interpreter
{
    public abstract class ContextEvaluatorBase : ISupportedFlags
    {
        public static readonly uint EVALUATE_VARIABLE = 0X1;
        public static readonly uint EVALUATE_FUNCTION = 0X2;
        public static readonly uint EVALUATE_PROPERTY = 0X4;
        public static readonly uint EVALUATE_CONSTRUCTOR = 0X8;
        public static readonly uint EVALUATE_CLASS = 0X10;

        public string Name
        {
            get;
        }

        public ContextEvaluatorBase(string name)
        {
            Name = name;
        }

        public abstract uint GetSupportedFlags();
        public abstract void EvaluateVariable(VariableObj variableObj, Dictionary<string, string> extraParams);
        public abstract void EvaluateFunction(FunctionObj functionObj, Dictionary<string, string> extraParams);
        public abstract void EvaluateProperty(VariableObj variableObj, Dictionary<string, string> extraParams);
        public abstract void EvaluateConstructor(List<VariableObj> constructorParams, Dictionary<string, string> extraParams);
        public abstract void EvaluateClass(LangClassBuilderBase classBuilder, Dictionary<string, string> extraParams);
    }
}
