using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace R5_Reloaded_Downloader_Library.External
{
    public class ResourceProcess : IDisposable
    {
        public string GetPath { get; private set; }

        public ResourceProcess(string ResourceName)
        {
            GetPath = ExportingFile(Path.Combine(Path.GetTempPath(), ResourceName), ResourceName);
        }

        public void Dispose()
        {
            File.Delete(GetPath);
        }

        private static string ExportingFile(string path, string resource)
        {
            var name = "R5_Reloaded_Downloader_Library.External.Resources." + resource;
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
            if (stream == null) throw new("The assembly does not have the specified file.");
            using var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            File.WriteAllBytes(path, memoryStream.ToArray());
            return path;
        }
    }
}
