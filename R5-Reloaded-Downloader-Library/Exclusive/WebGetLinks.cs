using R5_Reloaded_Downloader_Library.Get;
using R5_Reloaded_Downloader_Library.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R5_Reloaded_Downloader_Library.Exclusive
{
    public static class WebGetLinks
    {
        public static string DetoursR5()
        {
            foreach (var link in GitHub.GetLatestRelease("Mauler125", "r5sdk"))
                if (StringProcessing.GetExtension(link) == "zip")
                    return link;
            throw new("Unable to retrieve the link for detours_r5.");
        }

        public static string ScriptsR5() =>
            "https://github.com/Mauler125/scripts_r5/archive/refs/heads/S3_N1094.zip";

        public static string UpdaterR5() =>
            "https://downloads.r5reloaded.com/tools/R5ReloadedUpdater.exe";

        public static string ApexClient() =>
            "https://downloads.r5reloaded.com/builds/R5pc_r5launch_N1094_CL456479_2019_10_30_05_20_PM.zip";

        public static string WorldsEdgeAfterDark() =>
            "https://eax.re/r5/mp_rr_desertlands_64k_x_64k_nx.7z";
    }
}
