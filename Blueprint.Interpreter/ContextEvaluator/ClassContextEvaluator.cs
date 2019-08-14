using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blueprint.Logic;

namespace Blueprint.Interpreter
{
    public class ClassContextEvaluator : ContextEvaluatorBase
    {
        private LangClassBuilderBase _classBuilder;

        public ClassContextEvaluator(LangClassBuilderBase classBuilder)
            : base("Class")
        {
            _classBuilder = classBuilder;
        }

        public override uint GetSupportedFlags()
        {
            uint flags = 0;
            flags |= EVALUATE_VARIABLE;
            flags |= EVALUATE_PROPERTY;
            flags |= EVALUATE_FUNCTION;
            flags |= EVALUATE_CONSTRUCTOR;
            flags |= EVALUATE_CLASS;

            return flags;
        }

        public override void EvaluateVariable(VariableObj variableObj, Dictionary<string, string> extraParams)
        {
            _classBuilder.CreateClassMemeber(variableObj, 
                GetAccessModifierFromExtraParams(extraParams, AccessModifier.PRIVATE));
        }

        public override void EvaluateProperty(VariableObj variableObj, Dictionary<string, string> extraParams)
        {
            _classBuilder.CreateClassProperty(variableObj,
                GetAccessModifierFromExtraParams(extraParams, AccessModifier.PRIVATE));
        }

        public override void EvaluateFunction(FunctionObj functionObj, Dictionary<string, string> extraParams)
        {
            _classBuilder.CreateClassFunction(functionObj,
                GetAccessModifierFromExtraParams(extraParams, AccessModifier.PUBLIC));
        }

        public override void EvaluateConstructor(List<VariableObj> constructorParams, Dictionary<string, string> extraParams)
        {
            _classBuilder.CreateConstructor(constructorParams,
                GetAccessModifierFromExtraParams(extraParams, AccessModifier.PUBLIC));
        }

        public override void EvaluateClass(LangClassBuilderBase classBuilder, Dictionary<string, string> extraParams)
        {
            _classBuilder.CreateInnerClass(classBuilder,
                GetAccessModifierFromExtraParams(extraParams, AccessModifier.PRIVATE));
        }

        private AccessModifier GetAccessModifierFromExtraParams(
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
}
