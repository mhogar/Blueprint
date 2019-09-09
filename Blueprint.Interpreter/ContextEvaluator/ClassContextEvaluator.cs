using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blueprint.Logic;
using Blueprint.Logic.LangClassBuilder;

namespace Blueprint.Interpreter.ContextEvaluator
{
    public class ClassContextEvaluator : ContextEvaluatorBase, 
        IEvaluateVariable, IEvaluateProperty, IEvaluateFunction, IEvaluateConstructor, IEvaluateClass
    {
        private LangClassBuilderBase _classBuilder;

        public ClassContextEvaluator(LangClassBuilderBase classBuilder) : base("Class")
        {
            _classBuilder = classBuilder;
        }

        public void EvaluateVariable(VariableObj variableObj, Dictionary<string, string> extraParams)
        {
            TryCastUtil.TryCast<ICreateClassMemeber>(_classBuilder).CreateClassMemeber(
                variableObj, GetAccessModifierFromExtraParams(extraParams, AccessModifier.PRIVATE));
        }

        public void EvaluateProperty(VariableObj variableObj, Dictionary<string, string> extraParams)
        {
            TryCastUtil.TryCast<ICreateClassProperty>(_classBuilder).CreateClassProperty(
                variableObj, GetAccessModifierFromExtraParams(extraParams, AccessModifier.PRIVATE));
        }

        public void EvaluateFunction(FunctionObj functionObj, Dictionary<string, string> extraParams)
        {
            TryCastUtil.TryCast<ICreateClassFunction>(_classBuilder).CreateClassFunction(
                functionObj, GetAccessModifierFromExtraParams(extraParams, AccessModifier.PUBLIC));
        }

        public void EvaluateConstructor(List<VariableObj> constructorParams, Dictionary<string, string> extraParams)
        {
            TryCastUtil.TryCast<ICreateClassConstructor>(_classBuilder).CreateClassConstructor(
                constructorParams, GetAccessModifierFromExtraParams(extraParams, AccessModifier.PUBLIC));
        }

        public void EvaluateClass(LangClassBuilderBase classBuilder, Dictionary<string, string> extraParams)
        {
            TryCastUtil.TryCast<ICreateInnerClass>(_classBuilder).CreateInnerClass(
                classBuilder, GetAccessModifierFromExtraParams(extraParams, AccessModifier.PRIVATE));
        }
    }
}
