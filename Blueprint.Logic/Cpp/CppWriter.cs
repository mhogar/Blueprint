using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blueprint.Logic
{
    public class CppWriter : ILangWriter
    {
        private FilenameInfo _headerFilenameInfo;
        private FilenameInfo _sourceFilenameInfo;

        public CppWriter(string outFile)
        {
            _headerFilenameInfo = new FilenameInfo(outFile + ".h");
            _headerStream = null;

            _sourceFilenameInfo = new FilenameInfo(outFile + ".cpp");
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

        public void BeginWriter()
        {
            HeaderStream = new LangStreamWrapper(
                new StreamWriter(
                    new FileStream(_headerFilenameInfo.FullFilename, FileMode.Create, FileAccess.Write)
                )
            );
            HeaderStream.WriteLine("#pragma once");
            HeaderStream.NewLine();

            SourceStream = new LangStreamWrapper(
               new StreamWriter(
                   new FileStream(_sourceFilenameInfo.FullFilename, FileMode.Create, FileAccess.Write)
               )
           );
            SourceStream.WriteLine($"#include \"{_headerFilenameInfo.BaseAndExtname}\"");
            SourceStream.NewLine();
        }

        public void EndWriter()
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
            funcStr.TrimEnd(", ".ToCharArray());
            funcStr += ")";

            return funcStr;
        }
    }
}
