using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R5_Reloaded_Downloader_Library.IO
{
    public static class PathExpansion
    {
        public static string Combine(params string[] paths)
        {
            string path = string.Empty;
            foreach (var str in paths) path = Path.Combine(path, str);
            return path.Replace(@"\", "/");
        }
    }
}
