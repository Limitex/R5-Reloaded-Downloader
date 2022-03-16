using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R5_Reloaded_Downloader_Library.Text
{
    public class StringProcessing
    {
        public static string GetExtension(string address) => Path.GetExtension(address).Replace(".", "").ToLower();

        public static string ByteToStringWithUnits(float data)
        {
            if (data == 0) return "0.000".PadLeft(7) + " Byte";
            var count = 0;
            var text = new string[] { "Byte", "KB", "MB", "GB", "TB" };
            while (data >= 1024f) { data /= 1024f; count++; }
            return data.ToString(".000").PadLeft(7) + " " + text[count];
        }
    }
}
