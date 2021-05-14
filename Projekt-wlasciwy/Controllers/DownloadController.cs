using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Projekt_wlasciwy
{
    public class DownloadController
    {

        /// <summary>
        /// Find existing files
        /// </summary>
        /// <returns></returns>
        public static async Task Check()
        {
            var files = Directory.EnumerateFiles(SettingsController.DownloadFolder, "*").ToList();
            foreach(var file in files)
            {
                await Task.Run(() => MoveFile(Path.Combine(SettingsController.DownloadFolder, file)));
            }
        }

        public static void Watcher()
        {
            Console.WriteLine("Watcher is running...");

            // Get event on new files
            FileSystemWatcher watcher = new(SettingsController.DownloadFolder)
            {
                NotifyFilter = NotifyFilters.Attributes
                             | NotifyFilters.CreationTime
                             | NotifyFilters.DirectoryName
                             | NotifyFilters.FileName
                             | NotifyFilters.LastAccess
                             | NotifyFilters.LastWrite
                             | NotifyFilters.Security
                             | NotifyFilters.Size
            };

            // Event list
            watcher.Created += OnCreated;
            watcher.Deleted += OnDeleted;
            watcher.Renamed += OnRenamed;
            watcher.Error += OnError;

            // Config
            watcher.Filter = "*";
            watcher.IncludeSubdirectories = false;
            watcher.EnableRaisingEvents = true;
        }

        private static async Task MoveFile(string fullPath)
        {
            string extension = Path.GetExtension(fullPath);
            string fileName = Path.GetFileName(fullPath);

            foreach(var item in DirectoryController.Dirs)
            {
                if(item.Extensions.Any(extension.Contains))
                {
                    try
                    {
                        File.Move(fullPath, Path.Combine(item.FullPath, fileName));
                        LoggerController.Log($"Moved ('{fileName}') file to ('{Path.Combine(item.FullPath, fileName)}')");
                        break;
                    }
                    // File already exists
                    catch(IOException ioe)
                    {
                        if(File.Exists(Path.Combine(item.FullPath, fileName)))
                        {
                            string newName = RenameFile(fullPath, item.FullPath);
                            //Console.WriteLine(newName);
                            await Task.Run(() => File.Move(fullPath, newName));
                            await MoveFile(newName);
                        }

                        LoggerController.PrintException(ioe);
                    }
                    catch(Exception e)
                    {
                        LoggerController.PrintException(e);
                    }
                }
            }
        }

        private static string RenameFile(string fullPath, string pathToMove)
        {
            if(fullPath == "" || !File.Exists(fullPath) || pathToMove == "")
                return fullPath;

            string fileName = Path.GetFileNameWithoutExtension(fullPath);
            string fileExtension = Path.GetExtension(fullPath);
            int number = 0;

            while(File.Exists(fullPath))
            {
                number++;
                string newFileName = $"{fileName} ({number}){fileExtension}";
                fullPath = Path.Combine(pathToMove, newFileName);
            }

            return fullPath;
        }


        #region File Event
        private static void OnCreated(object sender, FileSystemEventArgs e)
        {
            string ext = Path.GetExtension(e.FullPath);

            if(ext == ".tmp" || ext == ".crdownload")
                return;

            LoggerController.Log($"Created ('{e.FullPath}')");

            Thread.Sleep(200);
            Task.Run(() => MoveFile(e.FullPath));
        }

        private static void OnRenamed(object sender, RenamedEventArgs e)
        {
            LoggerController.Log($"Renamed:\n Old: {e.OldFullPath}\nNew: {e.FullPath}");
            Task.Run(() => MoveFile(e.FullPath));
        }

        private static void OnDeleted(object sender, FileSystemEventArgs e) =>
            LoggerController.Log($"Deleted: {e.FullPath}");

        private static void OnError(object sender, ErrorEventArgs e) =>
            LoggerController.PrintException(e.GetException());
        #endregion
    }
}
