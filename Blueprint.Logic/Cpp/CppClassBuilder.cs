using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blueprint.Logic
{
    public class CppClassBuilder : ILangClassBuilder
    {
        private Tuple<string, AccessModifier> _classObj;
        private List<Tuple<VariableObj, AccessModifier>> _members;
        private List<Tuple<FunctionObj, AccessModifier, bool>> _functions;
        private List<Tuple<CppClassBuilder, AccessModifier>> _subClasses;

        public CppClassBuilder()
        {
            _members = new List<Tuple<VariableObj, AccessModifier>>();
            _functions = new List<Tuple<FunctionObj, AccessModifier, bool>>();
        }

        public void CreateClass(string name, AccessModifier accessModifier)
        {
            _classObj = new Tuple<string, AccessModifier>(name, accessModifier);
        }

        public void CreateClassFunction(FunctionObj functionObj, AccessModifier accessModifier, bool isOverridable)
        {
            _functions.Add(new Tuple<FunctionObj, AccessModifier, bool>(functionObj, accessModifier, isOverridable));
        }

        public void CreateClassMemeber(VariableObj variableObj, AccessModifier accessModifier)
        {
            _members.Add(new Tuple<VariableObj, AccessModifier>(variableObj, accessModifier));
        }

        public void CreateClassProperty(VariableObj variableObj, AccessModifier accessModifier)
        {
            if (accessModifier != AccessModifier.PRIVATE)
            {
                accessModifier = AccessModifier.PROTECTED;
            }

            CreateClassMemeber(variableObj, accessModifier);

            var getFunc = new FunctionObj(variableObj.Type, "get_" + variableObj.Name);
            CreateClassFunction(getFunc, AccessModifier.PUBLIC, false);

            var setFunc = new FunctionObj("void", "set_" + variableObj.Name);
            setFunc.FuncParams.Add(new VariableObj(variableObj.Type, variableObj.Name));
            CreateClassFunction(setFunc, AccessModifier.PUBLIC, false);
        }

        public void CreateConstructor(List<VariableObj> constructorParams, AccessModifier accessModifier)
        {
            var constructor = new FunctionObj("", _classObj.Item1);
            constructor.FuncParams = constructorParams;
            CreateClassFunction(constructor, accessModifier, false);

            var deconstructor = new FunctionObj("", "~" + _classObj.Item2);
            CreateClassFunction(deconstructor, accessModifier, true);
        }

        public void CreateSubClass(ILangClassBuilder classBuilder, AccessModifier accessModifier)
        {
            var cppClassBuilder = classBuilder as CppClassBuilder;
            if (cppClassBuilder == null)
            {
                throw new InvalidCastException("ILangClassBuilder was not a CppClassBuilder.");
            }

            _subClasses.Add(new Tuple<CppClassBuilder, AccessModifier>(cppClassBuilder, accessModifier));
        }

        public void WriteClass(ILangWriter langWriter)
        {
            var cppWriter = langWriter as CppWriter;
            if (cppWriter == null)
            {
                throw new InvalidCastException("ILangWriter was not a CppWriter.");
            }
        }
    }
}
