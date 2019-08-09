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
        public static readonly uint EVALUATE_CLASS = 0X8;

        protected LangFactoryBase _langFactory;

        public ContextEvaluatorBase(LangFactoryBase langFactory)
        {
            _langFactory = langFactory;
        }

        public abstract uint GetSupportedFlags();
        public abstract void EvaluateVariable(VariableObj variableObj, List<string> extraParams);
        public abstract void EvaluateFunction(FunctionObj functionObj, List<string> extraParams);
        public abstract void EvaluateProperty(VariableObj variableObj, List<string> extraParams);
        public abstract LangClassBuilderBase EvaluateClass(string className, List<string> extraParams);
    }
}
