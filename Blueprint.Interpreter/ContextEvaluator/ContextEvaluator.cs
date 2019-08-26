using Blueprint.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blueprint.Logic.LangClassBuilder;
using Blueprint.Logic.LangFileBuilder;

namespace Blueprint.Interpreter.ContextEvaluator
{
    public abstract class ContextEvaluatorBase : TryCastBase
    {
        public string Name
        {
            get;
        }

        public ContextEvaluatorBase(string name)
        {
            Name = name;
        }

        protected AccessModifier GetAccessModifierFromExtraParams(
            Dictionary<string, string> extraParams, AccessModifier defaultValue)
        {
            string accessModifierStr;
            if (!extraParams.TryGetValue("accessModifier", out accessModifierStr))
            {
                return defaultValue;
            }

            return BlueprintInterpreter.ParseAccessModifier(accessModifierStr);
        }
    }

    public interface IEvaluateVariable
    {
        void EvaluateVariable(VariableObj variableObj, Dictionary<string, string> extraParams);
    }

    public interface IEvaluateFunction
    {
        void EvaluateFunction(FunctionObj functionObj, Dictionary<string, string> extraParams);
    }

    public interface IEvaluateProperty
    {
        void EvaluateProperty(VariableObj variableObj, Dictionary<string, string> extraParams);
    }

    public interface IEvaluateConstructor
    {
        void EvaluateConstructor(List<VariableObj> constructorParams, Dictionary<string, string> extraParams);
    }

    public interface IEvaluateClass
    {
        void EvaluateClass(LangClassBuilderBase classBuilder, Dictionary<string, string> extraParams);
    }

    public interface IEvaluateFile
    {
        void EvaluateFile(LangFileBuilderBase fileBuilder, Dictionary<string, string> extraParams);
    }
}
