using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blueprint.Logic
{
    public class CppWriter : LangWriterBase
    {
        public CppWriter(string className) : base(className)
        {
        }

        public override void AddProperty(MemberObj propertyObj)
        {
            //properties can only be private or protected, so if the access modifier is not private, make it protected
            if (propertyObj.Access != AccessModifier.PRIVATE)
            {
                propertyObj.Access = AccessModifier.PROTECTED;
            }

            //-- add a member to the member list and get and set functions to the functions list --
            AddMember(propertyObj);

            VariableObj variable = propertyObj.Variable;
            var getFunc = new FunctionObj(AccessModifier.PUBLIC, variable.Type, "get_" + variable.Name);
            AddFunction(getFunc);

            var setFunc = new FunctionObj(AccessModifier.PUBLIC, "void", "set_" + variable.Name);
            setFunc.FuncParams.Add(new VariableObj(variable));
		    AddFunction(setFunc);
        }

        public override void Write(string outDir)
        {
            string sourceFile = outDir + _className + ".cpp";

            var publicMembers = new List<string>();
            var protectedMembers = new List<string>();
            var privateMembers = new List<string>();

            //init member string
            {
                foreach (FunctionObj func in _functions)
			    {
                    string funcStr = CreateFuncProtoString(func);
                    switch (func.Access)
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

                foreach (MemberObj member in _members)
			    {
                    string memberStr = CreateVariableString(member.Variable);
                    switch (member.Access)
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
                string headerFile = outDir + _className + ".h";
                StreamWriter writer = new StreamWriter(
                    new FileStream(headerFile, FileMode.OpenOrCreate, FileAccess.Write));

                writer.Write("class " + _className + "\n{\n");

                //public members
                if (publicMembers.Count > 0)
                {
                    writer.Write("public:\n");
                    WriteMembers(writer, publicMembers);
                }

                //protected members
                if (publicMembers.Count > 0)
                {
                    writer.Write("public:\n");
                    WriteMembers(writer, protectedMembers);
                }

                //private members
                if (privateMembers.Count > 0)
                {
                    writer.Write("public:\n");
                    WriteMembers(writer, privateMembers);
                }

                writer.Write("};\n");

                writer.Close();
                Console.WriteLine("Created: " + headerFile);
            }
        }

        private string CreateVariableString(VariableObj variableObj)
        {
            return variableObj.Type + " " + variableObj.Name;
        }

        private string CreateFuncProtoString(FunctionObj functionObj, bool addClassScope = false)
        {
            string funcName = functionObj.TypeAndName.Name;
            if (addClassScope)
            {
                funcName = _className + "::" + funcName;
            }

            string funcStr = CreateVariableString(functionObj.TypeAndName);

            funcStr += "(";
            foreach (VariableObj param in functionObj.FuncParams)
            {
                funcStr += CreateVariableString(param);
            }
            funcStr.TrimEnd(", ".ToCharArray());
            funcStr += ")";

		    return funcStr;
        }

        private string CreateFuncImplString(FunctionObj function, string content)
        {
            return CreateFuncProtoString(function, true) + "\n{" + content + "\n}\n";
        }

        private void WriteMembers(StreamWriter writer, List<string> memberStrs)
	    {
		    foreach (string memberStr in memberStrs)
		    {
                writer.Write("\t" + memberStr + ";\n");
            }
        }
    }
}
