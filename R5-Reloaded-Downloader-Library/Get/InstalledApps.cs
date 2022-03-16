using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Versioning;

namespace R5_Reloaded_Downloader_Library.Get
{
    [SupportedOSPlatform("windows")]
    public static class InstalledApps
    {
        private static string RegistryPath_64 = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
        private static string RegistryPath_32 = @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall";

        public static string[] DisplayNameList()
        {
            var List64 = GetUninstallList(RegistryPath_64);
            var List32 = GetUninstallList(RegistryPath_32);
            var ListAll = List64.Concat(List32).ToArray();
            Array.Sort(ListAll, StringComparer.OrdinalIgnoreCase);
            return ListAll;
        }

        private static List<string> GetUninstallList(string path)
        {
            var list = new List<string>();
            var keys = Registry.LocalMachine.OpenSubKey(path, false);
            if (keys == null) return list;
            foreach (var subKey in keys.GetSubKeyNames())
            {
                var subKeys = Registry.LocalMachine.OpenSubKey(path + @"\" + subKey, false);
                if (subKeys == null) continue;
                var displayName = subKeys.GetValue("DisplayName");
                if (displayName == null) list.Add(subKey);
                else list.Add(displayName.ToString() ?? "NoName");
            }
            return list;
        }
    }
}