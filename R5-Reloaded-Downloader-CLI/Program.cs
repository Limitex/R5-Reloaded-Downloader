using R5_Reloaded_Downloader_Library.Exclusive;
using R5_Reloaded_Downloader_Library.Get;
using R5_Reloaded_Downloader_Library.IO;
using R5_Reloaded_Downloader_Library.SevenZip;
using R5_Reloaded_Downloader_Library.Text;
using SevenZip;
using System;
using System.Runtime.Versioning;

namespace R5_Reloaded_Downloader_CLI
{
    [SupportedOSPlatform("windows")]
    class Program
    {
        private static string FinalDirectoryName = "R5-Reloaded";
        private static string ScriptsDirectoryPath = Path.Combine("platform", "scripts");
        private static string WorldsEdgeAfterDarkPath = "package";
        private static long AboutByteSize = 64L * 1024L * 1024L * 1024L;
        static void Main(string[] args)
        {
            ConsoleExpansion.DisableEasyEditMode();
            ConsoleExpansion.EnableVirtualTerminalProcessing();

            Console.Write("\n" +
                "  ┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓\n" +
                "  ┃                                 ┃\n" +
                "  ┃      R5-Reloaded Downloader     ┃\n" +
                "  ┃                                 ┃\n" +
                "  ┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛\n\n" +
                "  This program was created by Limitex.\n" +
                "  Please refer to the link below for the latest version of this program.\n\n" +
                "  https://github.com/Limitex/R5-Reloaded-Downloader/releases \n\n" +
                "Welcome!\n\n");

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

            if (!(InstalledApps.DisplayNameList() ?? Array.Empty<string>()).Contains("Origin"))
            {
                ConsoleExpansion.LogError("\'Origin\' is not installed.");
                ConsoleExpansion.LogError("Do you want to continue?");
                ConsoleExpansion.LogError("R5 Reloaded cannot be run without \'Origin\' installed.");
                if (!ConsoleExpansion.ConsentInput()) ConsoleExpansion.Exit();
            }

            var DriveRoot = Path.GetPathRoot(DirectionPath) ?? string.Empty;
            var DriveSize = FileExpansion.GetDriveFreeSpace(DirectionPath);
            ConsoleExpansion.LogWrite("[" + DriveRoot + "] Drive Size >> " + StringProcessing.ByteToStringWithUnits(DriveSize));
            ConsoleExpansion.LogWrite("Workspace Size required for this program >> " + StringProcessing.ByteToStringWithUnits(AboutByteSize));
            if (AboutByteSize > DriveSize)
            {
                ConsoleExpansion.LogError("There is not enough space to install.");
                ConsoleExpansion.LogWrite("Do you want to continue?");
                if (!ConsoleExpansion.ConsentInput()) ConsoleExpansion.Exit();
            }

            ConsoleExpansion.LogWrite("Do you want to start running?");
            ConsoleExpansion.LogWrite("It will take about 60 minutes to complete.");
            if (!ConsoleExpansion.ConsentInput()) ConsoleExpansion.Exit();

            ConsoleExpansion.WriteWidth('=', " Downloading");
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
            var downloadedByteSize = StringProcessing.ByteToStringWithUnits(totalBytesDownloaded).PadLeft(11);
            var totalByteSize = StringProcessing.ByteToStringWithUnits(totalFileSize ?? 0).PadLeft(11);
            var progressPercent = ((int?)progressPercentage ?? 0).ToString().PadLeft(3);
            ConsoleExpansion.LogWrite($"{downloadedByteSize} / {totalByteSize}  ({progressPercent}%) Downloading Completed.");
        }

        private static void Extractor_EventHandler(object? sender, ProgressEventArgs args)
        {
            ConsoleExpansion.LogWrite("(" + args.PercentDone.ToString().PadLeft(3) + "%) Extracting Completed.");
        }
    }
}