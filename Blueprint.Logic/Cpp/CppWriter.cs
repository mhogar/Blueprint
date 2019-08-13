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

        public static string CreateVariableString(VariableObj variableObj, string namespaceName="")
        {
            string namespaceStr = "";
            if (namespaceName != "")
            {
                namespaceStr = namespaceName + "::";
            }

            return variableObj.Type + " " + namespaceStr + variableObj.Name;
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
