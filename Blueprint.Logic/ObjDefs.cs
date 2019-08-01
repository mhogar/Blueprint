using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blueprint.Logic
{
    public enum AccessModifier
    {
        PUBLIC,
        PROTECTED,
        PRIVATE
    }

    public class VariableObj
    {
        public string Type
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public VariableObj(VariableObj variableObj)
            : this(variableObj.Type, variableObj.Name)
        {
        }

        public VariableObj(string type = "", string name = "")
        {
            Type = type;
            Name = name;
        }
    }

    public class MemberObj
    {
        public AccessModifier Access
        {
            get;
            set;
        }

        public VariableObj Variable
        {
            get;
            set;
        }

        public MemberObj(AccessModifier access, VariableObj variable)
        {
            Access = access;
            Variable = variable;
        }

        public MemberObj(AccessModifier access = AccessModifier.PUBLIC, string type = "", string name = "")
            : this(access, new VariableObj(type, name))
        {
        }
    }

    public class FunctionObj
    {
        public AccessModifier Access
        {
            get;
            set;
        }

        public VariableObj TypeAndName
        {
            get;
            set;
        }

        public List<VariableObj> FuncParams
        {
            get;
        }

        public FunctionObj(AccessModifier access, VariableObj typeAndName)
        {
            Access = access;
            TypeAndName = typeAndName;
            FuncParams = new List<VariableObj>();
        }

        public FunctionObj(AccessModifier access = AccessModifier.PUBLIC, string type = "", string name = "")
            : this(access, new VariableObj(type, name))
        {
        }
    }
}
