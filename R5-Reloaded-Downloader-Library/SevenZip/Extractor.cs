using R5_Reloaded_Downloader_Library.External;
using R5_Reloaded_Downloader_Library.IO;
using SevenZip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R5_Reloaded_Downloader_Library.SevenZip
{
    public class Extractor : IDisposable
    {
        public event EventHandler<ProgressEventArgs>? ProgressEventReceives = null;

        private ResourceProcess SevenZipDll;

        public Extractor()
        {
            SevenZipDll = new("7z.dll");
            SevenZipBase.SetLibraryPath(SevenZipDll.GetPath);
        }

        public void Dispose()
        {
            SevenZipDll.Dispose();
        }

        public string Run(string SourceArchive,bool DeleteSource = true, bool DirectoryFix = true)
        {
            var dirName = Path.GetFileNameWithoutExtension(SourceArchive);
            var ExtractionDirectory = Path.Combine(Path.GetDirectoryName(SourceArchive) ?? string.Empty, dirName);

            var extractor = new SevenZipExtractor(SourceArchive);
            if (ProgressEventReceives != null) extractor.Extracting += ProgressEventReceives;
            //extractor.ExtractionFinished += (sender, args) => { };
            extractor.ExtractArchive(ExtractionDirectory);
            if (DeleteSource) File.Delete(SourceArchive);
            if (DirectoryFix) DirectoryExpansion.DirectoryFix(ExtractionDirectory);
            return ExtractionDirectory;
        }
    }
}
