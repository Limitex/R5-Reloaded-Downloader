using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R5_Reloaded_Downloader_GUI
{
    public class DirectorySelector
    {
        private TextBox PathTextBox;
        private Button BrowseButton;

        public DirectorySelector(MainForm form)
        {
            PathTextBox = form.PathSelectTextBox;
            BrowseButton = form.BrowseButton;
            PathTextBox.Text = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Installer.DirName);
            BrowseButton.Click += new EventHandler(BrowseButton_Click);
        }

        private void BrowseButton_Click(object? sender, EventArgs e)
        {
            var fbd = new FolderBrowserDialog();
            fbd.SelectedPath = new DirectoryInfo(PathTextBox.Text)?.Parent?.FullName ?? string.Empty;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                PathTextBox.Text = Path.Combine(fbd.SelectedPath, Installer.DirName);
            }
        }
    }
}
