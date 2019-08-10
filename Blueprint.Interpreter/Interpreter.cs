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

        public void ReadFile(Stream stream)
        {
            XmlReader xmlReader = XmlReader.Create(stream);
            //read tag
            //if identifier open tag
                //create object using attributes
                //read tags and add them to dictionary until reached identifier end tag or content start tag
                //Call current evaluator with object and dictionary
                //if open content tag, create new context
            //else if end content tag
                //pop context off stack
        }

        public Result InterpretLine(string line)
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
        }
    }
}
