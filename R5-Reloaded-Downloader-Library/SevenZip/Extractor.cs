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
            SevenZipDll = new("7za.dll");
            SevenZipBase.SetLibraryPath(SevenZipDll.GetPath);
        }

        public void Dispose()
        {
            SevenZipDll.Dispose();
        }

        public string Run(string SourceArchive, bool DirectoryFix = false)
        {
            var dirName = Path.GetFileNameWithoutExtension(SourceArchive);
            var ExtractionDirectory = Path.Combine(Path.GetDirectoryName(SourceArchive) ?? string.Empty, dirName);

            var extractor = new SevenZipExtractor(SourceArchive);
            if (ProgressEventReceives != null) extractor.Extracting += ProgressEventReceives;
            //extractor.ExtractionFinished += (sender, args) => { };
            extractor.ExtractArchive(ExtractionDirectory);

            if (DirectoryFix)
            {
                var files = Directory.GetFiles(ExtractionDirectory);
                var dirs = Directory.GetDirectories(ExtractionDirectory);
                if (files.Length == 0 && dirs.Length == 1)
                {
                    Directory.Move(dirs[0], ExtractionDirectory + "_buffer");
                    Directory.Delete(ExtractionDirectory);
                    Directory.Move(ExtractionDirectory + "_buffer", ExtractionDirectory);
                }
            }
            return ExtractionDirectory;
        }
    }
}
