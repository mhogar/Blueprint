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
            get
            {
                return TypeAndName.Type;
            }
            set
            {
                TypeAndName.Type = value;
            }
        }

        public string Name
        {
            get
            {
                return TypeAndName.Name;
            }
            set
            {
                TypeAndName.Name = value;
            }
        }

        public VariableObj TypeAndName
        {
            get;
            set;
        }

        public List<VariableObj> FuncParams
        {
            get;
            set;
        }

        public Action<LangStreamWrapper> ContentDelegate
        {
            get;
            set;
        }

        public FunctionObj(VariableObj typeAndName, Action<LangStreamWrapper> contentDelegate = null)
        {
            TypeAndName = typeAndName;
            FuncParams = new List<VariableObj>();
            ContentDelegate = contentDelegate;
        }

        public FunctionObj(string type = "", string name = "", Action<LangStreamWrapper> contentDelegate = null)
            : this(new VariableObj(type, name), contentDelegate)
        {
        }
    }
}
