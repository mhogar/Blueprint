using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blueprint.Logic
{
    public class CppClassBuilder : ILangClassBuilder
    {
        private struct ClassObj
        {
            public string className;
            public AccessModifier accessModifier;
        }

        private struct ClassMemeber
        {
            public VariableObj variableObj;
            public AccessModifier accessModifier;
        }

        private struct ClassFunction
        {
            public FunctionObj functionObj;
            public AccessModifier accessModifier;
            public bool isOverridable;
        }

        private ClassObj _classObj;
        private List<ClassMemeber> _members;
        private List<ClassFunction> _functions;
        private List<CppClassBuilder> _subClasses;

        public CppClassBuilder()
        {
            _members = new List<ClassMemeber>();
            _functions = new List<ClassFunction>();
            _subClasses = new List<CppClassBuilder>();
        }

        public void CreateClass(string className, AccessModifier accessModifier)
        {
            _classObj.className = className;
            _classObj.accessModifier = accessModifier;
        }

        public void CreateClassFunction(FunctionObj functionObj, AccessModifier accessModifier, 
            bool isOverridable=false)
        {
            ClassFunction classFunction;
            classFunction.functionObj = functionObj;
            classFunction.accessModifier = accessModifier;
            classFunction.isOverridable = isOverridable;

            _functions.Add(classFunction);
        }

        public void CreateClassMemeber(VariableObj variableObj, AccessModifier accessModifier)
        {
            ClassMemeber classMemeber;
            classMemeber.variableObj = variableObj;
            classMemeber.accessModifier = accessModifier;

            _members.Add(classMemeber);
        }

        public void CreateClassProperty(VariableObj variableObj, AccessModifier accessModifier)
        {
            if (accessModifier != AccessModifier.PRIVATE)
            {
                accessModifier = AccessModifier.PROTECTED;
            }

            CreateClassMemeber(variableObj, accessModifier);

            var getFunc = new FunctionObj(variableObj.Type, "get_" + variableObj.Name, (stream) =>
            {
                stream.WriteLine("return this->" + variableObj.Name);
            });
            CreateClassFunction(getFunc, AccessModifier.PUBLIC, false);

            var setFunc = new FunctionObj("void", "set_" + variableObj.Name, (stream) =>
            {
                stream.WriteLine("this->" + variableObj.Name + " = " + variableObj.Name + ";");
            });
            setFunc.FuncParams.Add(new VariableObj(variableObj.Type, variableObj.Name));
            CreateClassFunction(setFunc, AccessModifier.PUBLIC, false);
        }

        public void CreateConstructor(List<VariableObj> constructorParams, AccessModifier accessModifier)
        {
            var constructor = new FunctionObj("", _classObj.className);
            constructor.FuncParams = constructorParams;
            CreateClassFunction(constructor, accessModifier);

            var deconstructor = new FunctionObj("", "~" + _classObj.className);
            CreateClassFunction(deconstructor, accessModifier, true);
        }

        public void CreateSubClass(ILangClassBuilder classBuilder, AccessModifier accessModifier)
        {
            var cppClassBuilder = classBuilder as CppClassBuilder;
            if (cppClassBuilder == null)
            {
                throw new InvalidCastException("ILangClassBuilder was not a CppClassBuilder.");
            }

            _subClasses.Add(cppClassBuilder);
        }

        public void WriteClass(ILangWriter langWriter)
        {
            var cppWriter = langWriter as CppWriter;
            if (cppWriter == null)
            {
                throw new InvalidCastException("ILangWriter was not a CppWriter.");
            }

            WriterHeaderFile(cppWriter.HeaderStream);
            WriteSourceFile(cppWriter.SourceStream);
        }

        private void WriterHeaderFile(LangStreamWrapper stream)
        {
            var publicMembers = new List<string>();
            var protectedMembers = new List<string>();
            var privateMembers = new List<string>();

            //init member string
            {
                foreach (ClassFunction classFunc in _functions)
                {
                    string funcStr = CppWriter.CreateFunctionString(classFunc.functionObj);
                    switch (classFunc.accessModifier)
                    {
                        case AccessModifier.PRIVATE:
                            privateMembers.Add(funcStr);
                            break;
                        case AccessModifier.PROTECTED:
                            protectedMembers.Add(funcStr);
                            break;
                        default: //ACCESS_PUBLIC
                            publicMembers.Add(funcStr);
                            break;
                    }
                }

                foreach (ClassMemeber classMemeber in _members)
                {
                    string memberStr = CppWriter.CreateVariableString(classMemeber.variableObj);
                    switch (classMemeber.accessModifier)
                    {
                        case AccessModifier.PRIVATE:
                            privateMembers.Add(memberStr);
                            break;
                        case AccessModifier.PROTECTED:
                            protectedMembers.Add(memberStr);
                            break;
                        default: //ACCESS_PUBLIC
                            publicMembers.Add(memberStr);
                            break;
                    }
                }
            }

            //write the header file
            {
                stream.WriteLine("class " + _classObj.className);
                stream.WriteLine("{");
                {
                    var memberStringTuples = new List<Tuple<string, List<string>>>();
                    memberStringTuples.Add(new Tuple<string, List<string>>("public", publicMembers));
                    memberStringTuples.Add(new Tuple<string, List<string>>("protected", protectedMembers));
                    memberStringTuples.Add(new Tuple<string, List<string>>("private", privateMembers));

                    foreach (Tuple<string, List<string>> memberStringTuple in memberStringTuples)
                    {
                        List<string> memberStrings = memberStringTuple.Item2;
                        if (memberStrings.Count > 0)
                        {
                            stream.WriteLine(memberStringTuple.Item1 + ":");

                            stream.IncreaseTab();
                            foreach (string memberStr in memberStrings)
                            {
                                stream.WriteLine(memberStr + ";");
                            }
                            stream.DecreaseTab();

                            stream.NewLine();
                        }
                    }
                }
                stream.WriteLine("};");
            }
        }

        private void WriteSourceFile(LangStreamWrapper stream)
        {
            foreach (ClassFunction classFunc in _functions)
            {
                stream.NewLine();
                stream.WriteLine(CppWriter.CreateFunctionString(classFunc.functionObj, _classObj.className));
                stream.WriteLine("{");

                stream.IncreaseTab();
                classFunc.functionObj.ContentDelegate?.Invoke(stream);
                stream.DecreaseTab();

                stream.WriteLine("}");
            }
        }
    }
}
