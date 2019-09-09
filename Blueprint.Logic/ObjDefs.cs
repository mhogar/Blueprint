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

    public enum DataType
    {
        NONE,
        VOID,
        BOOLEAN,
        CHAR,
        STRING,
        INT_8, INT_16, INT_32, INT_64,
        UINT_8, UINT_16, UINT_32, UINT_64,
        FLOAT_32, FLOAT_64
    }

    public class VariableObj
    {
        public DataType Type
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

        public VariableObj(DataType type = DataType.NONE, string name = "")
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

        public DataType Type
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

        public FunctionObj(DataType type = DataType.NONE, string name = "", Action<LangStreamWrapper> contentDelegate = null)
            : this(new VariableObj(type, name), contentDelegate)
        {
        }
    }
}
