﻿using Blueprint.Logic;
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
        private LangFactoryBase _langFactory;
        private string _outDir;

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
            XmlReader reader = XmlReader.Create(stream);

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "Blueprint")
                {
                    InterpretContent(reader, null);
                    break;
                }
            }
        }

        private void InterpretContent(XmlReader reader, ContextEvaluatorBase contextEvaluator)
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    string identifier = reader.Name;
                    Dictionary<string, string> extraParams;
                    switch (identifier)
                    {
                        case "File":
                            string filename = GetAttributeOrDefault(reader, "name", "");
                            LangFileBuilderBase fileBuilder = _langFactory.CreateFileBuilder();

                            InterpretIdentifier(reader, identifier, () =>
                            {
                                return new FileContextEvaluator(fileBuilder);
                            });

                            //write the file
                            LangWriterBase langWriter = _langFactory.CreateLangWriter(_outDir + filename);
                            langWriter.BeginWriter();
                            {
                                fileBuilder.WriteFile(langWriter);
                            }
                            langWriter.EndWriter();
                            break;
                        case "Variable":
                            var variableObj = new VariableObj(
                                ParseDataType(GetAttributeOrDefault(reader, "type", "")),
                                GetAttributeOrDefault(reader, "name", "")
                            );

                            extraParams = InterpretIdentifier(reader, identifier, null);

                            contextEvaluator.EvaluateVariable(variableObj, extraParams);
                            break;
                        case "Function":
                            var functionObj = new FunctionObj(
                                ParseDataType(GetAttributeOrDefault(reader, "returnType", "")),
                                GetAttributeOrDefault(reader, "name", "")
                            );

                            //parse the function params
                            string[] funcParams = GetAttributeOrDefault(reader, "params", "").Split(' ');
                            foreach (string funcParam in funcParams)
                            {
                                string[] tokens = funcParam.Split(':');
                                functionObj.FuncParams.Add(new VariableObj(
                                    ParseDataType(tokens[1]), tokens[0]));
                            }

                            extraParams = InterpretIdentifier(reader, identifier, null);

                            contextEvaluator.EvaluateFunction(functionObj, extraParams);
                            break;
                        case "Property":
                            var propertyObj = new VariableObj(
                                ParseDataType(GetAttributeOrDefault(reader, "type", "")),
                                GetAttributeOrDefault(reader, "name", "")
                            );

                            extraParams = InterpretIdentifier(reader, identifier, null);

                            contextEvaluator.EvaluateProperty(propertyObj, extraParams);
                            break;
                        case "Class":
                            LangClassBuilderBase classBuilder = _langFactory.CreateClassBuilder();
                            classBuilder.CreateClass(GetAttributeOrDefault(reader, "name", ""));

                            extraParams = InterpretIdentifier(reader, identifier, () =>
                            {
                                return new ClassContextEvaluator(classBuilder);
                            });

                            contextEvaluator.EvaluateClass(classBuilder, extraParams);
                            break;
                        default:
                            throw new InvalidOperationException(
                                $"Identifier \"{identifier}\" not valid in context {contextEvaluator.Name}");
                    }
                }
            }
        }

        private Dictionary<string, string> InterpretIdentifier(
            XmlReader reader, string identifier, Func<ContextEvaluatorBase> createContextEvaluatorDelegate)
        {
            var extraParams = new Dictionary<string, string>();

            while(reader.Read()
                && !(reader.NodeType == XmlNodeType.EndElement && reader.Name == identifier))
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    string tagName = reader.Name;
                    if (tagName == "content")
                    {
                        InterpretContent(reader, createContextEvaluatorDelegate());
                    }
                    else
                    {
                        reader.Read();
                        extraParams.Add(tagName, reader.Value);
                    }
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
                    throw new ArgumentException("Invalid access modifier string: " + accessModifier);
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
                case "float":
                    return DataType.FLOAT_32;
                case "double":
                    return DataType.FLOAT_64;
                default:
                    throw new ArgumentException("Invalid data type string: " + dataType);
            }
        }
    }
}
