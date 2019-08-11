﻿using System;
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

        public ClassContextEvaluator(LangFactoryBase langFactory, LangClassBuilderBase classBuilder)
            : base(langFactory, "Class")
        {
            _classBuilder = classBuilder;
        }

        public override uint GetSupportedFlags()
        {
            uint flags = 0;
            flags &= EVALUATE_VARIABLE;
            flags &= EVALUATE_PROPERTY;
            flags &= EVALUATE_FUNCTION;
            flags &= EVALUATE_CLASS;

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

        public override LangClassBuilderBase EvaluateClass(string className, Dictionary<string, string> extraParams)
        {
            LangClassBuilderBase classBuilder = _langFactory.CreateClassBuilder();
            classBuilder.CreateClass(className);

            _classBuilder.CreateInnerClass(classBuilder,
                GetAccessModifierFromExtraParams(extraParams, AccessModifier.PRIVATE));

            return classBuilder;
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
