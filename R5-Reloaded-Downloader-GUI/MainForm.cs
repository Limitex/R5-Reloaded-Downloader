using System.Diagnostics;

namespace R5_Reloaded_Downloader_GUI
{
    public partial class MainForm : Form
    {
        public static bool IsDuringInstallation = false;

        public MainForm()
        {
            InitializeComponent();
            _ = new LinkLabelOpener(this);
            _ = new DirectorySelector(this);
            _ = new Installer(this);
        }

        public static void ProcessStart(string fileName)
        {
            Process.Start(new ProcessStartInfo
            {
                UseShellExecute = true,
                FileName = fileName,
            });
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsDuringInstallation)
            {
                var dr = MessageBox.Show("Installation is in progress.\nDo you want to finish?", "Warning",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (dr == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}