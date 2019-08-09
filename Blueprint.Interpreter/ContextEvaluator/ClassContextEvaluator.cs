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

        public override void EvaluateVariable(VariableObj variableObj, List<string> extraParams)
        {
            _classBuilder.CreateClassMemeber(variableObj);
        }

        public override void EvaluateProperty(VariableObj variableObj, List<string> extraParams)
        {
            _classBuilder.CreateClassProperty(variableObj);
        }

        public override void EvaluateFunction(FunctionObj functionObj, List<string> extraParams)
        {
            _classBuilder.CreateClassFunction(functionObj);
        }

        public override LangClassBuilderBase EvaluateClass(string className, List<string> extraParams)
        {
            LangClassBuilderBase classBuilder = _langFactory.CreateClassBuilder();
            classBuilder.CreateClass(className);

            _classBuilder.CreateInnerClass(classBuilder);

            return classBuilder;
        }
    }
}
