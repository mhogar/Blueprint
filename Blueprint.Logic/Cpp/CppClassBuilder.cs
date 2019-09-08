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
        private string _namespaceName;
        private List<ClassMemeber> _members;
        private List<ClassFunction> _functions;
        private List<InnerClass> _innerClasses;

        public CppClassBuilder()
        {
            _members = new List<ClassMemeber>();
            _functions = new List<ClassFunction>();
            _innerClasses = new List<InnerClass>();
        }

        public override void CreateClass(string className, string namespaceName = "")
        {
            _className = className;
            _namespaceName = namespaceName;
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
                stream.WriteLine("return this->" + variableObj.Name + ";");
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
            var cppClassBuilder = classBuilder.TryCast<CppClassBuilder>();
            cppClassBuilder._namespaceName = CppWriter.CreateNamespaceString(cppClassBuilder._namespaceName, _className);

            InnerClass innerClass;
            innerClass.classBuilder = cppClassBuilder;
            innerClass.accessModifier = accessModifier;

            _innerClasses.Add(innerClass);
        }

        public override void WriteClass(LangWriterBase langWriter)
        {
            var cppWriter = langWriter.TryCast<CppWriter>();

            WriteHeaderFile(cppWriter);
            WriteSourceFile(cppWriter);
        }

        private delegate void WriteToStream();

        private void WriteHeaderFile(CppWriter cppWriter)
        {
            LangStreamWrapper stream = cppWriter.HeaderStream;

            var publicMembers = new List<WriteToStream>();
            var protectedMembers = new List<WriteToStream>();
            var privateMembers = new List<WriteToStream>();

            Action<AccessModifier, WriteToStream> EvaluateAccessModifier = (accessModifier, writeDelegate) =>
            {
                switch (accessModifier)
                {
                    case AccessModifier.PRIVATE:
                        privateMembers.Add(writeDelegate);
                        break;
                    case AccessModifier.PROTECTED:
                        protectedMembers.Add(writeDelegate);
                        break;
                    default: //ACCESS_PUBLIC
                        publicMembers.Add(writeDelegate);
                        break;
                }
            };

            //init member string
            {
                foreach (InnerClass innerClass in _innerClasses)
                {
                    EvaluateAccessModifier(innerClass.accessModifier, () => 
                    {
                        innerClass.classBuilder.WriteClass(cppWriter);
                        stream.NewLine();
                    });
                }

                foreach (ClassFunction classFunc in _functions)
                {
                    EvaluateAccessModifier(classFunc.accessModifier, () => 
                    {
                        CppWriter.WriteFunctionString(stream, classFunc.functionObj);
                        stream.WriteLine(";");
                    });
                }

                foreach (ClassMemeber classMemeber in _members)
                {
                    EvaluateAccessModifier(classMemeber.accessModifier, () => 
                    {
                        CppWriter.WriteVariableString(stream, classMemeber.variableObj);
                        stream.WriteLine(";");
                    });
                }
            }

            //write the file
            {
                stream.WriteLine("class " + _className);
                stream.WriteLine("{");
                {
                    var memberTuples = new List<Tuple<string, List<WriteToStream>>>();
                    memberTuples.Add(new Tuple<string, List<WriteToStream>>("public", publicMembers));
                    memberTuples.Add(new Tuple<string, List<WriteToStream>>("protected", protectedMembers));
                    memberTuples.Add(new Tuple<string, List<WriteToStream>>("private", privateMembers));

                    foreach (Tuple<string, List<WriteToStream>> memberTuple in memberTuples)
                    {
                        List<WriteToStream> writeMemberDelegates = memberTuple.Item2;
                        if (writeMemberDelegates.Count > 0)
                        {
                            stream.WriteLine(memberTuple.Item1 + ":");

                            stream.IncreaseTab();
                            foreach (WriteToStream writeMemberDelegate in writeMemberDelegates)
                            {
                                writeMemberDelegate.Invoke();
                            }
                            stream.DecreaseTab();

                            stream.NewLine();
                        }
                    }
                }
                stream.WriteLine("};");
            }
        }

        private void WriteSourceFile(CppWriter cppWriter)
        {
            LangStreamWrapper stream = cppWriter.SourceStream;

            foreach (ClassFunction classFunc in _functions)
            {
                stream.NewLine();

                CppWriter.WriteFunctionString(stream, classFunc.functionObj, CppWriter.CreateNamespaceString(_className, _namespaceName));
                stream.NewLine();
                stream.WriteLine("{");

                stream.IncreaseTab();
                classFunc.functionObj.ContentDelegate?.Invoke(stream);
                stream.DecreaseTab();

                stream.WriteLine("}");
            }
        }
    }
}
