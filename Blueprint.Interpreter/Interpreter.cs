using Blueprint.Logic;
using Blueprint.Logic.Cpp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Blueprint.Interpreter
{
    public class Interpreter
    {
        private Stack<ContextEvaluatorBase> _contextStack;
        private LangFactoryBase _langFactory;

        public struct Result
        {
            public bool success;
            public string message;
            public uint lineNum;
        }

        public Interpreter(LangFactoryBase langFactory)
        {
            _contextStack = new Stack<ContextEvaluatorBase>();
            _langFactory = langFactory;

            //the interpreter starts in file context
            _contextStack.Push(new FileContextEvaluator(langFactory, langFactory.CreateFileBuilder()));
        }

        public void InterpretStream(Stream stream)
        {
            XmlReader reader = XmlReader.Create(stream);
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    ContextEvaluatorBase currentContext = _contextStack.Peek();
                    string identifier = reader.Name;
                    switch (identifier)
                    {
                        case "Variable":
                            var variableObj = new VariableObj(
                                GetAttributeOrDefault(reader, "type", ""),
                                GetAttributeOrDefault(reader, "name", "")
                            );
                            currentContext.EvaluateVariable(variableObj, ReadExtraParams(reader, identifier));
                            break;
                        case "Function":
                            var functionObj = new FunctionObj(
                                GetAttributeOrDefault(reader, "type", ""),
                                GetAttributeOrDefault(reader, "name", "")
                            );

                            //TODO: read function params

                            currentContext.EvaluateFunction(functionObj, ReadExtraParams(reader, identifier));
                            break;
                        case "Property":
                            var propertyObj = new VariableObj(
                                GetAttributeOrDefault(reader, "type", ""),
                                GetAttributeOrDefault(reader, "name", "")
                            );
                            currentContext.EvaluateProperty(propertyObj, ReadExtraParams(reader, identifier));
                            break;
                        case "Class":
                            string className = GetAttributeOrDefault(reader, "name", "");
                            LangClassBuilderBase classBuilder = 
                                currentContext.EvaluateClass(className, ReadExtraParams(reader, identifier));

                            if (IsContentBeginTag(reader))
                            {
                                _contextStack.Push(new ClassContextEvaluator(_langFactory, classBuilder));
                            }
                            break;
                    }
                }
                else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "content")
                {
                    _contextStack.Pop();
                }
            }
        }

        private Dictionary<string, string> ReadExtraParams(XmlReader reader, string identifier)
        {
            var extraParams = new Dictionary<string, string>();

            while(reader.Read()
                && !(reader.NodeType == XmlNodeType.EndElement && reader.Name == identifier)
                && !(IsContentBeginTag(reader)))
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    string key = reader.Name;
                    reader.Read();
                    extraParams.Add(key, reader.Value);
                }
            }

            return extraParams;
        }

        private string GetAttributeOrDefault(XmlReader reader, string name, string defaultValue)
        {
            string value = reader.GetAttribute(name);
            if (value == null)
            {
                return defaultValue;
            }

            return value;
        }

        private bool IsContentBeginTag(XmlReader reader)
        {
            return reader.NodeType == XmlNodeType.Element && reader.Name == "content";
        }

        /*public Result InterpretLine(string line)
        {
            Result result;
            result.success = true;
            result.message = "";
            result.lineNum = 0;

            return result;
        }

        public Result InterpretLine(string identifier, List<string> parameters, List<string> extraParams, bool createNewContext)
        {
            Result result;
            result.success = true;
            result.message = "";
            result.lineNum = 0;

            //interpret the identifier
            uint identifierFlag = 0;
            switch(identifier)
            {
                case "variable":
                    identifierFlag = ContextEvaluatorBase.EVALUATE_VARIABLE;
                    break;
                case "function":
                    identifierFlag = ContextEvaluatorBase.EVALUATE_FUNCTION;
                    break;
                case "property":
                    identifierFlag = ContextEvaluatorBase.EVALUATE_PROPERTY;
                    break;
                case "class":
                    identifierFlag = ContextEvaluatorBase.EVALUATE_CLASS;
                    break;
                default:
                    result.success = false;
                    result.message = $"Identifier \"{identifier}\" is not recognized.";
                    return result;
            }

            //if trying to create a new context, check if the identifier supports new context creation
            bool supportsNewContext = (identifierFlag == ContextEvaluatorBase.EVALUATE_CLASS);
            if (createNewContext && !supportsNewContext)
            {
                result.success = false;
                result.message = $"Identifier \"{identifier}\" does not support new context creation.";
                return result;
            }

            //verify the current context supports the identifier
            ContextEvaluatorBase currentContext = _contextStack.Peek();
            if ((currentContext.GetSupportedFlags() & identifierFlag) == 0)
            {
                result.success = false;
                result.message = $"Identifier \"{identifier}\" is not supported in context \"{currentContext.Name}\".";
                return result;
            }

            //use the identifier, params, and current context to evaluate the line
            if (identifierFlag == ContextEvaluatorBase.EVALUATE_VARIABLE)
            {
                var variableObj = new VariableObj();
                currentContext.EvaluateVariable(variableObj, extraParams);
            }
            else if (identifierFlag == ContextEvaluatorBase.EVALUATE_FUNCTION)
            {
                var functionObj = new FunctionObj();
                currentContext.EvaluateFunction(functionObj, extraParams);
            }
            else if (identifierFlag == ContextEvaluatorBase.EVALUATE_PROPERTY)
            {
                var variableObj = new VariableObj();
                currentContext.EvaluateProperty(variableObj, extraParams);
            }
            else if (identifierFlag == ContextEvaluatorBase.EVALUATE_CLASS)
            {
                var className = parameters[0];
                LangClassBuilderBase classBuilder = currentContext.EvaluateClass(className, extraParams);
                if (createNewContext)
                {
                    _contextStack.Push(new ClassContextEvaluator(_langFactory, classBuilder));
                }
            }

            return result;
        }*/
    }
}
