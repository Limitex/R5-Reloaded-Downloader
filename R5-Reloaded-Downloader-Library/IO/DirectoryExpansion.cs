using System;
using System.Diagnostics;
using System.IO;

namespace R5_Reloaded_Downloader_Library.IO
{
    public static class DirectoryExpansion
    {
        public static void CreateOverwrite(string path)
        {
            if (Directory.Exists(path)) DirectoryDelete(path);
            Directory.CreateDirectory(path);
        }

        public static void CreateIfNotFound(string path)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        }

        public static void MoveOverwrite(string sourcePath, string destinationPath)
        {
            DirectoryCopy(sourcePath, destinationPath);
            DirectoryDelete(sourcePath);
        }

        public static void DirectoryDelete(string targetDirectoryPath)
        {
            if (!Directory.Exists(targetDirectoryPath)) return;

            string[] filePaths = Directory.GetFiles(targetDirectoryPath);
            foreach (string filePath in filePaths)
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
                File.Delete(filePath);
            }

            string[] directoryPaths = Directory.GetDirectories(targetDirectoryPath);
            foreach (string directoryPath in directoryPaths) DirectoryDelete(directoryPath);

            Directory.Delete(targetDirectoryPath, false);
        }

        public static void DirectoryCopy(string sourcePath, string destinationPath)
        {
            var sourceDirectory = new DirectoryInfo(sourcePath);
            var destinationDirectory = new DirectoryInfo(destinationPath);

            if (destinationDirectory.Exists == false)
            {
                destinationDirectory.Create();
                destinationDirectory.Attributes = sourceDirectory.Attributes;
            }

            foreach (var fileInfo in sourceDirectory.GetFiles())
                fileInfo.CopyTo(destinationDirectory.FullName + @"\" + fileInfo.Name, true);

            foreach (var directoryInfo in sourceDirectory.GetDirectories())
                DirectoryCopy(directoryInfo.FullName, destinationDirectory.FullName + @"\" + directoryInfo.Name);
        }
    }
}
