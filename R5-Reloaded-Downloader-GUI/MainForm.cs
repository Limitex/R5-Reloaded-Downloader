using System.Diagnostics;

namespace R5_Reloaded_Downloader_GUI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            _ = new LinkLabelOpener(this);
            _ = new DirectorySelector(this);
        }

        public static void ProcessStart(string fileName)
        {
            Process.Start(new ProcessStartInfo
            {
                UseShellExecute = true,
                FileName = fileName,
            });
        }
    }
}