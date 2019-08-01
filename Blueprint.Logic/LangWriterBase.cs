using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blueprint.Logic
{
    public abstract class LangWriterBase
    {
        protected string _className;
        protected List<MemberObj> _members;
        protected List<MemberObj> _properties;
        protected List<FunctionObj> _functions;

        public abstract void Write(string outDir);

        public LangWriterBase(string className)
        {
            _className = className;
            _members = new List<MemberObj>();
            _properties = new List<MemberObj>();
            _functions = new List<FunctionObj>();
        }

        public virtual void AddMember(MemberObj memberObj)
        {
            _members.Add(memberObj);
        }

        public virtual void AddProperty(MemberObj propertyObj)
        {
            _properties.Add(propertyObj);
        }

        public virtual void AddFunction(FunctionObj functionObj)
        {
            _functions.Add(functionObj);
        } 
    }
}
