using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.Utils
{
    public static class FileHelper
    {
        public static DirectoryInfo EnsureDirectory(string path)
        {
            DirectoryInfo di = null;

            // Try to create the directory.
            di = Directory.CreateDirectory(path);

            return di;
        }

    }
}
