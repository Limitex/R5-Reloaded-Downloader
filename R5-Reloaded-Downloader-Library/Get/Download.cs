using R5_Reloaded_Downloader_Library.IO;
using R5_Reloaded_Downloader_Library.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R5_Reloaded_Downloader_Library.Get
{
    public class Download : IDisposable
    {
        public event ProgressChangedHandler? ProgressEventReceives;

        private string SaveingDirectoryPath;

        private HttpClientProgress? httpClient;

        public Download(string saveingDirectoryPath)
        {
            SaveingDirectoryPath = saveingDirectoryPath;
            DirectoryExpansion.CreateIfNotFound(SaveingDirectoryPath);
        }

        public void Dispose()
        {
            if (httpClient != null) httpClient.Dispose();
        }

        public string Run(string address)
        {
            var path = Path.Combine(SaveingDirectoryPath, Path.GetFileName(address));
            httpClient = new(address, path);
            if (ProgressEventReceives != null) httpClient.ProgressChanged += ProgressEventReceives;
            try { httpClient.StartDownload().Wait(); }
            catch (AggregateException) { }
            return path;
        }
    }
}
