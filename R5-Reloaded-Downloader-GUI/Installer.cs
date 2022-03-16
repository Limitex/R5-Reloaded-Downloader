using R5_Reloaded_Downloader_Library.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R5_Reloaded_Downloader_GUI
{
    public class Installer
    {
        private MainForm mainForm;
        public static readonly string DirName = "R5-Reloaded";

        public Installer(MainForm form)
        {
            mainForm = form;
            mainForm.InstallButton.Click += new EventHandler(InstallButton_Click);
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
            FileExpansion.CreateShortcut(path, "R5-Reloaded (Dedicated)", LinkDestination, "-dedicated");
            FileExpansion.CreateShortcut(path, "Scripts", scriptsPath, "");
        }

        private void InstallButton_Click(object? sender, EventArgs e)
        {
            ControlEnabled(false);
            MainForm.IsDuringInstallation = true;


        }
    }
}
