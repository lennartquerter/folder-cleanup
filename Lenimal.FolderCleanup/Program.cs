using System;
using System.IO;
using System.Linq;
using Lenimal.FolderCleanup.Models;
using Newtonsoft.Json;
using Serilog;

namespace Lenimal.FolderCleanup
{
    class Program
    {
        static void Main(string[] args)
        {
            var loggerConfig = new LoggerConfiguration();
            loggerConfig.WriteTo.Console();
            Log.Logger = loggerConfig.CreateLogger();

            var configStr = File.ReadAllText("config.json");
            var config = JsonConvert.DeserializeObject<CleanupSettings>(configStr);

            config.Folders.ForEach(Clean);
        }

        private static void Clean(CleanupFolder folder)
        {
            if (!Directory.Exists(folder.Source))
            {
                throw new ArgumentException($"Could not clean folder {folder.Source}, it does not exist");
            }

            var folderDestination = folder.Format
                .Where(x => x.Value.Contains("__folder__"))
                .ToList();

            if (folderDestination.Count > 1)
            {
                throw new ArgumentException("You should only have one folder destination");
            }

            if (!Directory.Exists(folder.Destination))
            {
                Directory.CreateDirectory(folder.Destination);
            }

            var files = new DirectoryInfo(folder.Source);

            foreach (var fileInfo in files.GetFiles())
            {
                var destinationDir = folder
                    .Format
                    .FirstOrDefault(f => f.Value.Contains(fileInfo.Extension));

                if (destinationDir.Value != null)
                {
                    var fullDestination = Path.Combine(folder.Destination, destinationDir.Key);
                    if (!Directory.Exists(fullDestination))
                    {
                        Directory.CreateDirectory(fullDestination);
                    }

                    fileInfo.MoveTo(Path.Combine(fullDestination, fileInfo.Name));
                }
            }


            if (folderDestination.Any())
            {
                foreach (var dir in files.GetDirectories())
                {
                    var fullDestination = Path.Combine(folder.Destination, folderDestination.First().Key, dir.Name);
                    if (!Directory.Exists(fullDestination))
                    {
                        Directory.CreateDirectory(fullDestination);
                    }

                    DirectoryFullCopy(dir.FullName, fullDestination);
                    Directory.Delete(dir.FullName, true);
                }
            }

            Log.Information($"Cleaned folder {folder.Source} to {folder.Destination}");
        }

        private static void DirectoryFullCopy(string sourceDirName, string destDirName)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.MoveTo(temppath, true);
            }

            foreach (DirectoryInfo subdir in dirs)
            {
                string temppath = Path.Combine(destDirName, subdir.Name);
                DirectoryFullCopy(subdir.FullName, temppath);
            }
        }
    }
}