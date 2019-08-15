using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blueprint.Logic.LangClassBuilder;

namespace Blueprint.Logic
{
    public class CppClassBuilder : LangClassBuilderBase, 
        ICreateClassConstructor, ICreateClassMemeber, ICreateClassFunction, ICreateClassProperty, ICreateInnerClass
    {
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

        private struct InnerClass
        {
            public CppClassBuilder classBuilder;
            public AccessModifier accessModifier;
        }

        private string _className;
        private List<ClassMemeber> _members;
        private List<ClassFunction> _functions;
        private List<InnerClass> _subClasses;

        public CppClassBuilder()
        {
            _members = new List<ClassMemeber>();
            _functions = new List<ClassFunction>();
            _subClasses = new List<InnerClass>();
        }

        public override void CreateClass(string className)
        {
            _className = className;
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

            var setFunc = new FunctionObj(DataType.VOID, "set_" + variableObj.Name, (stream) =>
            {
                stream.WriteLine("this->" + variableObj.Name + " = " + variableObj.Name + ";");
            });
            setFunc.FuncParams.Add(new VariableObj(variableObj.Type, variableObj.Name));
            CreateClassFunction(setFunc, AccessModifier.PUBLIC, false);
        }

        public void CreateClassConstructor(List<VariableObj> constructorParams, AccessModifier accessModifier)
        {
            var constructor = new FunctionObj(DataType.NONE, _className);
            constructor.FuncParams = constructorParams;
            CreateClassFunction(constructor, accessModifier);

            var deconstructor = new FunctionObj(DataType.NONE, "~" + _className);
            CreateClassFunction(deconstructor, accessModifier, true);
        }

        public void CreateInnerClass(LangClassBuilderBase classBuilder, AccessModifier accessModifier)
        {
            var cppClassBuilder = classBuilder as CppClassBuilder;
            if (cppClassBuilder == null)
            {
                throw new ArgumentException("LangClassBuilderBase was not a CppClassBuilder.");
            }

            InnerClass innerClass;
            innerClass.classBuilder = cppClassBuilder;
            innerClass.accessModifier = accessModifier;

            _subClasses.Add(innerClass);
        }

        public override void WriteClass(LangWriterBase langWriter)
        {
            var cppWriter = langWriter.TryCast<CppWriter>();

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

            //write the file
            {
                stream.WriteLine("class " + _className);
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

                stream.WriteLine(CppWriter.CreateFunctionString(classFunc.functionObj, _className));
                stream.WriteLine("{");

                stream.IncreaseTab();
                classFunc.functionObj.ContentDelegate?.Invoke(stream);
                stream.DecreaseTab();

                stream.WriteLine("}");
            }
        }
    }
}
