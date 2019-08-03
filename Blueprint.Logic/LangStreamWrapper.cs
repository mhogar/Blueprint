using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blueprint.Logic
{
    public class LangStreamWrapper
    {
        private byte _tabLevel;
        private StreamWriter _stream;

        public LangStreamWrapper(StreamWriter stream)
        {
            _tabLevel = 0;
            _stream = stream;
        }

        public void Close()
        {
            _stream.Close();
        }

        public void IncreaseTab(byte amount = 1)
        {
            //TODO: check for overflow
            _tabLevel += amount;
        }

        public void DecreaseTab(byte amount = 1)
        {
            if (_tabLevel < amount)
            {
                _tabLevel = 0;
                return;
            }

            _tabLevel -= amount;
        }

        public void Write(string text)
        {
            for (byte i = 0; i < _tabLevel; i++)
            {
                _stream.Write('\t');
            }

            _stream.Write(text);
        }

        public void WriteLine(string text)
        {
            Write(text);
            _stream.WriteLine();
        }

        public void NewLine()
        {
            _stream.WriteLine();
        }
    }
}
