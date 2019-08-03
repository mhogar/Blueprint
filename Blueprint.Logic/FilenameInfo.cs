using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blueprint.Logic
{
    public class FilenameInfo
    {
        public FilenameInfo(string filename)
        {
            Dirname = Path.GetDirectoryName(filename);
            Basename = Path.GetFileNameWithoutExtension(filename);
            Extname = Path.GetExtension(filename);
        }

        public string Dirname
        {
            get;
            set;
        }

        public string Basename
        {
            get;
            set;
        }

        public string Extname
        {
            get;
            set;
        }

        public string DirAndBasename
        {
            get
            {
                return Path.Combine(Dirname, Basename);
            }
        }

        public string BaseAndExtname
        {
            get
            {
                return Basename + Extname;
            }
        }

        public string FullFilename
        {
            get
            {
                return Path.Combine(Dirname, BaseAndExtname);
            }
        }
    }
}
