using R5_Reloaded_Downloader_Library.Get;
using R5_Reloaded_Downloader_Library.IO;
using R5_Reloaded_Downloader_Library.SevenZip;
using R5_Reloaded_Downloader_Library.Text;
using SevenZip;
using System;

namespace R5_Reloaded_Downloader_CLI
{
    class Program
    {
        private static string FinalDirectoryName = "R5-Reloaded";

        static void Main(string[] args)
        {
            var DirectionPath = Path.GetDirectoryName(Environment.ProcessPath);
            if (DirectionPath == null) throw new Exception();
            DirectionPath = Path.Combine(DirectionPath, FinalDirectoryName);

            if (!DirectoryExpansion.IsEmpty(DirectionPath))
            {
                ConsoleExpansion.LogError("The directory already exists.");
                ConsoleExpansion.LogError("Move or delete the directory.");
                ConsoleExpansion.LogError("Path : " + DirectionPath);
                ConsoleExpansion.Exit();
                return;
            }

            using (var download = new Download(DirectionPath))
            {
                download.ProgressEventReceives += HttpClientProcess_EventHandler;

            }

            using (var extractor = new Extractor())
            {
                extractor.ProgressEventReceives += Extractor_EventHandler;

            }
        }

        private static void HttpClientProcess_EventHandler(long? totalFileSize, long totalBytesDownloaded, double? progressPercentage)
        {
            var downloadedByteSize = StringProcessing.ByteToStringWithUnits(totalBytesDownloaded);
            var totalByteSize = StringProcessing.ByteToStringWithUnits(totalFileSize ?? 0);
            var progressPercent = ((int?)progressPercentage ?? 0).ToString().PadLeft(2);
            ConsoleExpansion.LogWrite($"{downloadedByteSize} / {totalByteSize} ({progressPercent}%)");
        }

        private static void Extractor_EventHandler(object? sender, ProgressEventArgs args)
        {
            ConsoleExpansion.LogWrite(args.PercentDone.ToString() + "% extracting completed.");
        }
    }
}