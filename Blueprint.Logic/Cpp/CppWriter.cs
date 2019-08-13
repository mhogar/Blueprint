using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blueprint.Logic
{
    public class CppWriter : LangWriterBase
    {
        public CppWriter(string filename) : base(filename)
        {
            _headerStream = null;
            _sourceStream = null;
        }

        private LangStreamWrapper _headerStream;
        public LangStreamWrapper HeaderStream
        {
            get
            {
                return _headerStream;
            }
            private set
            {
                if (_headerStream != null)
                    _headerStream.Close();

                _headerStream = value;
            }
        }

        private LangStreamWrapper _sourceStream;
        public LangStreamWrapper SourceStream
        {
            get
            {
                return _sourceStream;
            }
            private set
            {
                if (_sourceStream != null)
                    _sourceStream.Close();

                _sourceStream = value;
            }
        }

        public override void BeginWriter()
        {
            var headerFilenameInfo = new FilenameInfo(Filename);
            headerFilenameInfo.Extname = ".h";
            HeaderStream = new LangStreamWrapper(
                new StreamWriter(
                    new FileStream(headerFilenameInfo.FullFilename, FileMode.Create, FileAccess.Write)
                )
            );

            var sourceFilenameInfo = new FilenameInfo(Filename);
            sourceFilenameInfo.Extname = ".cpp";
            SourceStream = new LangStreamWrapper(
               new StreamWriter(
                   new FileStream(sourceFilenameInfo.FullFilename, FileMode.Create, FileAccess.Write)
               )
           );
        }

        public override void EndWriter()
        {
            HeaderStream.Close();
            SourceStream.Close();
        }

        public static string ConvertDataType(DataType dataType)
        {
            switch (dataType)
            {
                case DataType.NONE:
                    return "";
                case DataType.VOID:
                    return "void";
                case DataType.BOOLEAN:
                    return "bool";
                case DataType.CHAR:
                    return "char";
                case DataType.STRING:
                    return "const char*";
                case DataType.INT_8:
                    return "char";
                case DataType.INT_16:
                    return "short";
                case DataType.INT_32:
                    return "int";
                case DataType.INT_64:
                    return "long long";
                case DataType.UINT_8:
                    return "unsigned char";
                case DataType.UINT_16:
                    return "unsigned short";
                case DataType.UINT_32:
                    return "unsigned int";
                case DataType.UINT_64:
                    return "unsigned long long";
                case DataType.FLOAT_32:
                    return "float";
                case DataType.FLOAT_64:
                    return "double";
                default:
                    throw new ArgumentException("Invalid data type.");
            }
        }

        public static string CreateVariableString(VariableObj variableObj, string namespaceName="")
        {
            string namespaceStr = "";
            if (namespaceName != "")
            {
                namespaceStr = namespaceName + "::";
            }

            return ConvertDataType(variableObj.Type) + " " + namespaceStr + variableObj.Name;
        }

        public static string CreateFunctionString(FunctionObj functionObj, string namespaceName = "")
        {
            string funcStr = CreateVariableString(functionObj.TypeAndName, namespaceName) + "(";
            foreach (VariableObj funcParam in functionObj.FuncParams)
            {
                funcStr += CreateVariableString(funcParam, "") + ", ";
            }
            funcStr = funcStr.TrimEnd(", ".ToCharArray());
            funcStr += ")";

            return funcStr;
        }
    }
}
