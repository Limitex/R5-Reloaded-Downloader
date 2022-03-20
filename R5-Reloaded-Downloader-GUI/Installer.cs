using R5_Reloaded_Downloader_Library.Exclusive;
using R5_Reloaded_Downloader_Library.Get;
using R5_Reloaded_Downloader_Library.IO;
using R5_Reloaded_Downloader_Library.SevenZip;
using R5_Reloaded_Downloader_Library.Text;
using SevenZip;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R5_Reloaded_Downloader_GUI
{
    public class Installer
    {
        public delegate void Delegate();
        public static readonly string DirName = "R5-Reloaded";

        private MainForm mainForm;
        private static readonly string ScriptsDirectoryPath = Path.Combine("platform", "scripts");
        private static readonly string WorldsEdgeAfterDarkPath = "package";
        private static readonly string ExecutableFileName = "launcher.exe";
        private static readonly long AboutByteSize = 64L * 1024L * 1024L * 1024L;
        private static readonly int MaxPercentValue = 100;

        private static readonly int NumOfCalls_SetStepStatusAutoCount = 10;
        
        private static Stopwatch sw = new Stopwatch();
        private static int ProgressStatusValue = 0;
        private static string? TitleText;

        public Installer(MainForm form)
        {
            mainForm = form;
            mainForm.InstallButton.Click += new EventHandler(InstallButton_Click);
            TitleText = mainForm.Text;
        }

        private void ControlEnabled(bool status)
        {
            mainForm.BrowseButton.Enabled = status;
            mainForm.InstallButton.Enabled = status;
            mainForm.PathSelectTextBox.Enabled = status;
            mainForm.CreateDesktopShortcutCheckBox.Enabled = status;
            mainForm.AddToStartMenuShortcutCheckBox.Enabled = status;
        }

        private void CreateR5Shortcut(string path, string LinkDestination, string scriptsPath)
        {
            FileExpansion.CreateShortcut(path, "R5-Reloaded", LinkDestination, "");
            FileExpansion.CreateShortcut(path, "R5-Reloaded (Debug)", LinkDestination, "-debug");
            FileExpansion.CreateShortcut(path, "R5-Reloaded (Release)", LinkDestination, "-release");
            FileExpansion.CreateShortcut(path, "R5-Reloaded (Dedicated Dev)", LinkDestination, "-dedicated_dev");
            FileExpansion.CreateShortcut(path, "R5-Reloaded (Dedicated)", LinkDestination, "-dedicated");
            FileExpansion.CreateShortcut(path, "Scripts", scriptsPath, "");
        }

        private void InstallButton_Click(object? sender, EventArgs e)
        {
            ControlEnabled(false);
            MainForm.IsDuringInstallation = true;
            ProgressStatusValue = 0;

            mainForm.FullStatusLabel.Text = "Preparing...";
            var DirectionPath = mainForm.PathSelectTextBox.Text ?? string.Empty;
            var shortcutCreate_desktop = mainForm.CreateDesktopShortcutCheckBox.Checked;
            var shortcutCreate_startmenu = mainForm.AddToStartMenuShortcutCheckBox.Checked;

            if (AboutByteSize > FileExpansion.GetDriveFreeSpace(DirectionPath))
            {
                var dr = MessageBox.Show("There is not enough space to install.\n" +
                    "You need at least " + StringProcessing.ByteToStringWithUnits(AboutByteSize) + " to install.\n" +
                    "Do you want to continue?", "Warning",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (dr == DialogResult.Cancel)
                {
                    ControlEnabled(true);
                    MainForm.IsDuringInstallation = false;
                    mainForm.FullStatusLabel.Text = "...";
                    return;
                }
                ConsoleExpansion.LogError("There is not enough space to install.");
                ConsoleExpansion.LogWrite("Do you want to continue?");
                if (!ConsoleExpansion.ConsentInput()) ConsoleExpansion.Exit();
            }

            if (!DirectoryExpansion.IsEmpty(DirectionPath))
            {
                var dr = MessageBox.Show("The directory already exists. Move or delete the directory.\n" +
                    "Path : " + DirectionPath + "\nDo you want to delete it and continue?", "Warning",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (dr == DialogResult.Cancel)
                {
                    ControlEnabled(true);
                    MainForm.IsDuringInstallation = false;
                    mainForm.FullStatusLabel.Text = "...";
                    return;
                }
            }

            mainForm.FullStatusLabel.Text = "Deleteing...";
            DirectoryExpansion.DirectoryDelete(DirectionPath);
            mainForm.FullStatusLabel.Text = "...";

            if (MessageBox.Show("Ready was exit.\n" +
                "Do you want to start the installation?\n" +
                "It will take about 60 minutes to complete.", "Infomation",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel)
            {
                ControlEnabled(true);
                MainForm.IsDuringInstallation = false;
                mainForm.FullStatusLabel.Text = "...";
                return;
            }

            mainForm.FullStatusLabel.Text = "Starting...";
            Task.Run(() =>
            {
                while (MainForm.IsDuringInstallation)
                {
                    var ts = sw.Elapsed;
                    var str = ts.Hours.ToString("00") + ":" + ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00");
                    mainForm.Invoke(new Delegate(() => mainForm.Text = TitleText + " - " + str));
                    if (MainForm.IsDuringInstallation) Thread.Sleep(1000);
                }
            });

            sw.Restart();

            Task.Run(() => {
                var download = new Download(DirectionPath);
                download.ProgressEventReceives += HttpClientProcess_EventHandler;
                SetStepStatusAutoCount("downloading detours r5...");
                var detoursR5DirPath = download.Run(WebGetLinks.DetoursR5());
                SetStepStatusAutoCount("downloading scripts r5...");
                var scriptsR5DirPath = download.Run(WebGetLinks.ScriptsR5());
                SetStepStatusAutoCount("downloading r5 updater...");
                var updaterR5FilePath = download.Run(WebGetLinks.UpdaterR5());
                SetStepStatusAutoCount("downloading worlds edge after dark...");
                var worldsEdgeAfterDarkDirPath = download.Run(WebGetLinks.WorldsEdgeAfterDark());
                SetStepStatusAutoCount("downloading apex client s3...");
                var apexClientDirPath = download.Run(WebGetLinks.ApexClient()); 
                download.Dispose();

                var extractor = new Extractor();
                extractor.ProgressEventReceives += Extractor_EventHandler;
                SetStepStatusAutoCount("extracting detours r5...");
                detoursR5DirPath = extractor.Run(detoursR5DirPath);
                SetStepStatusAutoCount("extracting scripts r5...");
                scriptsR5DirPath = extractor.Run(scriptsR5DirPath);
                SetStepStatusAutoCount("extracting worlds edge after dark...");
                worldsEdgeAfterDarkDirPath = extractor.Run(worldsEdgeAfterDarkDirPath);
                SetStepStatusAutoCount("extracting apex client s3...");
                apexClientDirPath = extractor.Run(apexClientDirPath);
                extractor.Dispose();

                SetStepStatusAutoCount();
                mainForm.Invoke(new Delegate(() => mainForm.FullStatusLabel.Text = "Creating the R5-Reloaded..."));
                FileExpansion.Move(updaterR5FilePath, apexClientDirPath);
                DirectoryExpansion.MoveOverwrite(detoursR5DirPath, apexClientDirPath);
                Directory.Move(scriptsR5DirPath, Path.Combine(apexClientDirPath, ScriptsDirectoryPath));
                DirectoryExpansion.MoveOverwrite(Path.Combine(worldsEdgeAfterDarkDirPath, WorldsEdgeAfterDarkPath), apexClientDirPath);
                DirectoryExpansion.DirectoryDelete(worldsEdgeAfterDarkDirPath);
                DirectoryExpansion.DirectoryFix(DirectionPath);

                var AppPath = Path.Combine(DirectionPath, ExecutableFileName);
                var scriptsPath = Path.Combine(DirectionPath, ScriptsDirectoryPath);
                if (shortcutCreate_desktop)
                {
                    var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                    CreateR5Shortcut(desktopPath, AppPath, scriptsPath);
                }

                if (shortcutCreate_startmenu)
                {
                    var startMenuPath = Directory.GetDirectories(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu))[0];
                    var startmenuShortcutPath = Path.Combine(startMenuPath, DirName);
                    Directory.CreateDirectory(startmenuShortcutPath);
                    CreateR5Shortcut(startmenuShortcutPath, AppPath, scriptsPath);
                }

                mainForm.Invoke(new Delegate(() =>
                {
                    mainForm.FullProgressBar.Value = mainForm.FullProgressBar.Maximum;
                    mainForm.MonoProgressBar.Value = mainForm.MonoProgressBar.Maximum;
                    mainForm.FullStatusLabel.Text = "Done.";
                    ControlEnabled(true);
                    MainForm.IsDuringInstallation = false;
                    sw.Stop();
                }));
            });
        }

        private void HttpClientProcess_EventHandler(long? totalFileSize, long totalBytesDownloaded, double? progressPercentage)
        {
            var parcent = progressPercentage ?? 0;
            var downloadedByteSize = StringProcessing.ByteToStringWithUnits(totalBytesDownloaded);
            var totalByteSize = StringProcessing.ByteToStringWithUnits(totalFileSize ?? 0);
            var progressPercent = ((int)parcent).ToString().PadLeft(3);
            mainForm.Invoke(new Delegate(() => {
                mainForm.FullStatusLabel.Text = $"{downloadedByteSize} / {totalByteSize}  ({progressPercent}%) Downloading Completed.";
                mainForm.MonoProgressBar.Value = (int)(parcent * mainForm.MonoProgressBar.Maximum / MaxPercentValue);
                SetFullProgressValue(parcent);
            }));
        }

        private void Extractor_EventHandler(object? sender, ProgressEventArgs args)
        {
            mainForm.Invoke(new Delegate(() => {
                mainForm.FullStatusLabel.Text = "(" + args.PercentDone.ToString().PadLeft(3) + "%) Extracting Completed.";
                mainForm.MonoProgressBar.Value = (int)(args.PercentDone * mainForm.MonoProgressBar.Maximum / MaxPercentValue);
                SetFullProgressValue(args.PercentDone);
            }));
        }

        private void SetFullProgressValue(double value)
        {
            var MaxProgressValue = mainForm.FullProgressBar.Maximum;
            var progress = (int)(MaxProgressValue * (ProgressStatusValue + (value / MaxPercentValue)) / NumOfCalls_SetStepStatusAutoCount);
            mainForm.FullProgressBar.Value = progress < MaxProgressValue ? progress : MaxProgressValue;
        }

        private void SetStepStatusAutoCount(string str = "")
        {
            mainForm.Invoke(new Delegate(() => {
                ProgressStatusValue++;
                mainForm.StepStatusLabel.Text = "Step (" + ProgressStatusValue + "/" + NumOfCalls_SetStepStatusAutoCount + ")" + (str != "" ? " : " : "") + str;
            }));
        }
    }
}
