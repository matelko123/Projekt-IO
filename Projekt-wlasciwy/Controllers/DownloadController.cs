using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Projekt_wlasciwy
{
    public class DownloadController
    {
        private static readonly int interval = 500;         // Time pause before moving files

        // Get user path do \Download directory
        private static string UserRoot = Environment.GetEnvironmentVariable("USERPROFILE");
        private static string DownloadFolder = Path.Combine(UserRoot, "Downloads");


        public static async Task Watcher()
        {
            Console.WriteLine("Watcher is running...");

            // Find existing files
            var files = Directory.EnumerateFiles(DownloadFolder, "*").ToList();
            foreach(var file in files)
            {
                Console.WriteLine(file);
                await Task.Run(() => MoveFile(Path.Combine(DownloadFolder, file)));
            }


            // Get event on new files
            FileSystemWatcher watcher = new FileSystemWatcher(DownloadFolder)
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

            watcher.Created += OnCreated;
            watcher.Deleted += OnDeleted;
            watcher.Renamed += OnRenamed;
            watcher.Error += OnError;

            watcher.Filter = "*";
            watcher.IncludeSubdirectories = false;
            watcher.EnableRaisingEvents = true;
        }

        private static void OnCreated(object sender, FileSystemEventArgs e)
        {
            string fullpath = e.FullPath;
            string ext = Path.GetExtension(fullpath);

            if(ext == ".tmp" || ext == ".crdownload")
                return;

            //LoggerController.Log($"Created ('{fullpath}') with extension ('{ext}')");
            Console.WriteLine($"Created ('{fullpath}') with extension ('{ext}')");

            Thread.Sleep(interval);
            MoveFile(fullpath);
        }

        /// <summary>
        /// Event on renamed file
        /// </summary>
        /// <param name="sender">Object</param>
        /// <param name="e">Event</param>
        private static void OnRenamed(object sender, RenamedEventArgs e)
        {
            LoggerController.Log($"Renamed:\n Old: {e.OldFullPath}\nNew: {e.FullPath}");

            MoveFile(e.FullPath);
        }

        private static void MoveFile(string fullPath)
        {
            string ext = Path.GetExtension(fullPath);
            string fileName = Path.GetFileName(fullPath);

            foreach(var item in DirectoryController.Dirs)
            {
                if(item.Extensions.Any(ext.Contains))
                {
                    try
                    {
                        File.Move(fullPath, Path.Combine(item.FullPath, fileName));
                        LoggerController.Log($"Moved ('{fileName}') file to ('{Path.Combine(item.FullPath, fileName)}')");
                        break;
                    }
                    // Name reached max lenght
                    catch(PathTooLongException ptex)
                    {
                        LoggerController.PrintException(ptex);
                    }
                    // File already exists
                    catch(IOException)
                    {
                        string newName = RenameFile(fullPath);

                        MoveFile(newName);
                        return;
                    }
                    catch(Exception e)
                    {
                        LoggerController.PrintException(e);
                    }
                }
            }
        }

        private static string RenameFile(string fullPath)
        {
            if(fullPath == "" || !File.Exists(fullPath))
                return fullPath;

            int count = 1;
            string fileNameOnly = Path.GetFileNameWithoutExtension(fullPath);
            string extension = Path.GetExtension(fullPath);
            string path = Path.GetDirectoryName(fullPath);
            string newFullPath = fullPath;

            while(File.Exists(newFullPath))
            {
                string tempFileName = string.Format("{0} ({1})", fileNameOnly, count++);
                newFullPath = Path.Combine(path, tempFileName + extension);
            }

            File.Move(fullPath, newFullPath);

            return newFullPath;
        }

        private static void OnDeleted(object sender, FileSystemEventArgs e) =>
            LoggerController.Log($"Deleted: {e.FullPath}");

        private static void OnError(object sender, ErrorEventArgs e) =>
            LoggerController.PrintException(e.GetException());
    }
}
