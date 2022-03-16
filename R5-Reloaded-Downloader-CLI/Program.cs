using R5_Reloaded_Downloader_Library.Exclusive;
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
        private static string ScriptsDirectoryPath = Path.Combine("platform", "scripts");
        private static string WorldsEdgeAfterDarkPath = "package";
        static void Main(string[] args)
        {

            ConsoleExpansion.LogWrite("Preparing...");
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

            ConsoleExpansion.WriteWidth('=', "Downloading");
            var download = new Download(DirectionPath);
            download.ProgressEventReceives += HttpClientProcess_EventHandler;
            ConsoleExpansion.WriteWidth('-', "Detours_R5");
            var detoursR5DirPath = download.Run(WebGetLinks.DetoursR5());
            ConsoleExpansion.WriteWidth('-', "Scripts_R5");
            var scriptsR5DirPath = download.Run(WebGetLinks.ScriptsR5());
            ConsoleExpansion.WriteWidth('-', "Worlds Edge After Dark");
            var worldsEdgeAfterDarkDirPath = download.Run(WebGetLinks.WorldsEdgeAfterDark());
            ConsoleExpansion.WriteWidth('-', "Apex Client S3");
            var apexClientDirPath = download.Run(WebGetLinks.ApexClient());
            download.Dispose();
            ConsoleExpansion.WriteWidth('=');

            ConsoleExpansion.WriteWidth('=', "Extracting");
            var extractor = new Extractor();
            extractor.ProgressEventReceives += Extractor_EventHandler;
            ConsoleExpansion.WriteWidth('-', "Detours_R5");
            detoursR5DirPath = extractor.Run(detoursR5DirPath);
            ConsoleExpansion.WriteWidth('-', "Scripts_R5");
            scriptsR5DirPath = extractor.Run(scriptsR5DirPath);
            ConsoleExpansion.WriteWidth('-', "Worlds Edge After Dark");
            worldsEdgeAfterDarkDirPath = extractor.Run(worldsEdgeAfterDarkDirPath);
            ConsoleExpansion.WriteWidth('-', "Apex Client S3");
            apexClientDirPath = extractor.Run(apexClientDirPath);
            extractor.Dispose();
            ConsoleExpansion.WriteWidth('=');

            ConsoleExpansion.LogWrite("Creating the R5-Reloaded");

            DirectoryExpansion.MoveOverwrite(detoursR5DirPath, apexClientDirPath);
            Directory.Move(scriptsR5DirPath, Path.Combine(apexClientDirPath, ScriptsDirectoryPath));
            DirectoryExpansion.MoveOverwrite(Path.Combine(worldsEdgeAfterDarkDirPath, WorldsEdgeAfterDarkPath), apexClientDirPath);
            DirectoryExpansion.DirectoryDelete(worldsEdgeAfterDarkDirPath);
            DirectoryExpansion.DirectoryFix(DirectionPath);
            ConsoleExpansion.LogWrite("Done.");
            ConsoleExpansion.Exit();
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