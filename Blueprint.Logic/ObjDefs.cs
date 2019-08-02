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

    public class FunctionObj
    {
        public AccessModifier Access
        {
            get;
            set;
        }

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

        public List<VariableObj> FuncParams
        {
            get;
            set;
        }

        public FunctionObj(VariableObj typeAndName)
        {
            Type = typeAndName.Type;
            Name = typeAndName.Name;
            FuncParams = new List<VariableObj>();
        }

        public FunctionObj(string type = "", string name = "")
            : this(new VariableObj(type, name))
        {
        }
    }
}
