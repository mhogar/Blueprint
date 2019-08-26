using Blueprint.Logic;
using Blueprint.Logic.LangFactory;
using Blueprint.Logic.LangClassBuilder;
using Blueprint.Logic.LangFileBuilder;
using Blueprint.Interpreter.ContextEvaluator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

namespace Blueprint.Interpreter
{
    public class BlueprintInterpreter
    {
        private LangFactoryBase _langFactory;
        private string _outDir;

        public class InterpreterParseException : Exception
        {
            public InterpreterParseException(string message)
                : base(message)
            {
            }

            public InterpreterParseException(string message, Exception innerException)
                : base(message, innerException)
            {
            }
        }

        public class BlueprintSchemaValidationException : Exception
        {
            public BlueprintSchemaValidationException(ValidationEventArgs args)
                : base(args.Message, args.Exception)
            {
            }
        }

        public BlueprintInterpreter(LangFactoryBase langFactory)
        {
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
            _outDir = outDir;

            try
            {
                var doc = new XmlDocument();
                doc.Load(stream);

                doc.Schemas.Add(null, 
                    "C:/Users/mhoga/Documents/Projects/Blueprint/Blueprint.Interpreter/BlueprintXMLSchema.xsd");
                doc.Validate(new ValidationEventHandler((object sender, ValidationEventArgs args) =>
                {
                    if (args.Severity == XmlSeverityType.Error)
                    {
                        throw new BlueprintSchemaValidationException(args);
                    }
                    else
                    {
                        Console.WriteLine("Blueprint Validation Warning: " + args.Message);
                    }
                }));

                foreach (XmlNode node in doc.ChildNodes)
                {
                    if (node.Name == "Blueprint")
                    {
                        InterpretContent(node, new BlueprintContextEvaluator());
                        break;
                    }
                }
            }
            catch (XmlException e)
            {
                throw new InterpreterParseException(e.Message);
            }
        }

        private void InterpretContent(XmlNode contentNode, ContextEvaluatorBase contextEvaluator)
        {
            foreach (XmlNode parentNode in contentNode.ChildNodes)
            {
                Dictionary<string, string> extraParams;
                switch (parentNode.Name)
                {
                    case "Files":
                        foreach (XmlNode node in parentNode.ChildNodes)
                        {
                            LangFileBuilderBase fileBuilder = _langFactory.TryCast<ICreateFileBuilder>().CreateFileBuilder();
                            fileBuilder.CreateFile(_outDir + GetAttributeOrDefault(node, "name", ""));

                            extraParams = InterpretIdentifier(node, () =>
                            {
                                return new FileContextEvaluator(fileBuilder);
                            });

                            contextEvaluator.TryCast<IEvaluateFile>().EvaluateFile(fileBuilder, extraParams);
                        }
                        break;
                    case "Variables":
                        foreach (XmlNode node in parentNode.ChildNodes)
                        {
                            var variableObj = new VariableObj(
                                ParseDataType(GetAttributeOrDefault(node, "type", "")),
                                GetAttributeOrDefault(node, "name", "")
                            );

                            extraParams = InterpretIdentifier(node, null);

                            contextEvaluator.TryCast<IEvaluateVariable>().EvaluateVariable(variableObj, extraParams);
                        }
                        break;
                    case "Functions":
                        foreach (XmlNode node in parentNode.ChildNodes)
                        {
                            var functionObj = new FunctionObj(
                                ParseDataType(GetAttributeOrDefault(node, "returnType", "")),
                                GetAttributeOrDefault(node, "name", "")
                            );
                            functionObj.FuncParams = ParseFuncParams(GetAttributeOrDefault(node, "params", ""));

                            extraParams = InterpretIdentifier(node, null);

                            contextEvaluator.TryCast<IEvaluateFunction>().EvaluateFunction(functionObj, extraParams);
                        }
                        break;
                    case "Properties":
                        foreach (XmlNode node in parentNode.ChildNodes)
                        {
                            var propertyObj = new VariableObj(
                                ParseDataType(GetAttributeOrDefault(node, "type", "")),
                                GetAttributeOrDefault(node, "name", "")
                            );

                            extraParams = InterpretIdentifier(node, null);

                            contextEvaluator.TryCast<IEvaluateProperty>().EvaluateProperty(propertyObj, extraParams);
                        }
                        break;
                    case "Constructor":
                        List<VariableObj> constructorParams = ParseFuncParams(GetAttributeOrDefault(parentNode, "params", ""));

                        extraParams = InterpretIdentifier(parentNode, null);

                        contextEvaluator.TryCast<IEvaluateConstructor>().EvaluateConstructor(constructorParams, extraParams);
                        break;
                    case "Classes":
                        foreach (XmlNode node in parentNode.ChildNodes)
                        {
                            LangClassBuilderBase classBuilder =
                            _langFactory.TryCast<ICreateClassBuilder>().CreateClassBuilder();

                            classBuilder.CreateClass(GetAttributeOrDefault(node, "name", ""));

                            extraParams = InterpretIdentifier(node, () =>
                            {
                                return new ClassContextEvaluator(classBuilder);
                            });

                            contextEvaluator.TryCast<IEvaluateClass>().EvaluateClass(classBuilder, extraParams);
                        }
                        break;
                    default:
                        /* Note: this should be impossible since an invalid identifier would be caught by the 
                         * schema validation, but we should still throw an exception just in case */
                        throw new InterpreterParseException($"Identifier \"{parentNode.Name}\" not recognized.");
                }
            }
        }

        private Dictionary<string, string> InterpretIdentifier(
            XmlNode identifierNode, Func<ContextEvaluatorBase> createContextEvaluatorDelegate)
        {
            var extraParams = new Dictionary<string, string>();

            foreach (XmlNode node in identifierNode.ChildNodes)
            {
                if (node.Name == "content")
                {
                    InterpretContent(node, createContextEvaluatorDelegate());
                }
                else
                {
                    extraParams.Add(node.Name, node.InnerText);
                }
            }

            return extraParams;
        }

        private string GetAttributeOrDefault(XmlNode node, string name, string defaultValue)
        {
            XmlAttribute attribute = node.Attributes[name];
            if (attribute == null)
            {
                return defaultValue;
            }

            return attribute.Value;
        }

        public static List<VariableObj> ParseFuncParams(string paramsString)
        {
            var funcParams = new List<VariableObj>();
            if (paramsString == "")
            {
                return funcParams;
            }

            string[] paramTokens = paramsString.Split(' ');
            foreach (string paramToken in paramTokens)
            {
                string[] tokens = paramToken.Split(':');
                if (tokens.Length != 2)
                {
                    throw new InterpreterParseException($"Invalid function param string \"{paramToken}\"");
                }

                funcParams.Add(new VariableObj(ParseDataType(tokens[1]), tokens[0]));
            }

            return funcParams;
        }

        public static AccessModifier ParseAccessModifier(string accessModifier)
        {
            switch (accessModifier)
            {
                case "public":
                    return AccessModifier.PUBLIC;
                case "protected":
                    return AccessModifier.PROTECTED;
                case "private":
                    return AccessModifier.PRIVATE;
                default:
                    throw new InterpreterParseException($"Invalid access modifier string \"{accessModifier}\"");
            }
        }

        public static DataType ParseDataType(string dataType)
        {
            switch (dataType)
            {
                case "":
                    return DataType.NONE;
                case "void":
                    return DataType.VOID;
                case "boolean":
                    return DataType.BOOLEAN;
                case "char":
                    return DataType.CHAR;
                case "string":
                    return DataType.STRING;
                case "int8":
                    return DataType.INT_8;
                case "int16":
                    return DataType.INT_16;
                case "int32":
                    return DataType.INT_32;
                case "int64":
                    return DataType.INT_64;
                case "uint8":
                    return DataType.UINT_8;
                case "uint16":
                    return DataType.UINT_16;
                case "uint32":
                    return DataType.UINT_32;
                case "uint64":
                    return DataType.UINT_64;
                case "float32":
                    return DataType.FLOAT_32;
                case "float64":
                    return DataType.FLOAT_64;
                default:
                    throw new InterpreterParseException($"Invalid data type string \"{dataType}\"");
            }
        }
    }
}
