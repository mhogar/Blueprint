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
    public class BlueprintInterpreter
    {
        private Stack<ContextEvaluatorBase> _contextStack;
        private LangFactoryBase _langFactory;

        public struct Result
        {
            public bool success;
            public string message;
            public uint lineNum;
        }

        public BlueprintInterpreter(LangFactoryBase langFactory)
        {
            _contextStack = new Stack<ContextEvaluatorBase>();
            _langFactory = langFactory;
        }

        public void InterpretBlueprint(string filename, string outDir)
        {
            var stream = new FileStream(filename, FileMode.Open, FileAccess.Read);
            InterpretBlueprint(stream, outDir);
            stream.Close();
        }

        public void InterpretBlueprint(Stream stream, string outDir)
        {
            LangFileBuilderBase currentFileBuilder = null;

            XmlReader reader = XmlReader.Create(stream);
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    ContextEvaluatorBase currentContext = null;
                    if (_contextStack.Count > 0)
                    {
                        currentContext = _contextStack.Peek();
                    }

                    string identifier = reader.Name;
                    switch (identifier)
                    {
                        case "File":
                            string filename = GetAttributeOrDefault(reader, "name", "");
                            currentFileBuilder = _langFactory.CreateFileBuilder(filename);

                            ReadExtraParams(reader, identifier);
                            if (IsContentBeginTag(reader))
                            {
                                _contextStack.Push(new FileContextEvaluator(_langFactory, currentFileBuilder));
                            }
                            break;
                        case "Variable":
                            var variableObj = new VariableObj(
                                GetAttributeOrDefault(reader, "type", ""),
                                GetAttributeOrDefault(reader, "name", "")
                            );
                            currentContext.EvaluateVariable(variableObj, ReadExtraParams(reader, identifier));
                            break;
                        case "Function":
                            var functionObj = new FunctionObj(
                                GetAttributeOrDefault(reader, "returnType", ""),
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
                else if (reader.NodeType == XmlNodeType.EndElement)
                {
                    switch(reader.Name)
                    {
                        case "File":
                            //write the file
                            LangWriterBase langWriter = _langFactory.CreateLangWriter();
                            langWriter.BeginWriter(outDir + currentFileBuilder.Filename);
                            {
                                currentFileBuilder.WriteFile(langWriter);
                                currentFileBuilder = null;
                            }
                            langWriter.EndWriter();
                            break;
                        case "content":
                            _contextStack.Pop();
                            break;
                    }
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

        public static AccessModifier ParseAccessModifier(string accessModifierStr)
        {
            switch (accessModifierStr)
            {
                case "public":
                    return AccessModifier.PUBLIC;
                case "protected":
                    return AccessModifier.PROTECTED;
                case "private":
                    return AccessModifier.PRIVATE;
                default:
                    throw new ArgumentException("Invalid access modifier string.");
            }
        }
    }
}
