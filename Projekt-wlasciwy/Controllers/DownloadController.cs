using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace Projekt_wlasciwy
{
    public class DownloadManager
    {

        private static int interval = 500;          // Time pause before moving files
        private static string Filter = "*";         // Filter used to watching

        // Get user path do \Download directory
        private static string UserRoot = Environment.GetEnvironmentVariable("USERPROFILE");
        private static string DownloadFolder = Path.Combine(UserRoot, "Downloads");


        
        public DownloadManager()
        {
            Console.WriteLine("Running");

            // Watcher
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

            watcher.Filter = Filter;
            watcher.IncludeSubdirectories = false;
            watcher.EnableRaisingEvents = true;
        }


        private static void OnCreated(object sender, FileSystemEventArgs e)
        {
            string fullpath = e.FullPath;
            string ext = Path.GetExtension(fullpath);

            Console.WriteLine("Created ('{0}') with extension ('{1}')", fullpath, ext);

            // MoveFile(fullpath);
        }

        private static void OnRenamed(object sender, RenamedEventArgs e)
        {
            Console.WriteLine($"Renamed:");
            Console.WriteLine($"    Old: {e.OldFullPath}");
            Console.WriteLine($"    New: {e.FullPath}");

            string ext = Path.GetExtension(e.FullPath);

            // MoveFile(e.FullPath);
        }

        //private static void MoveFile(string fullPath)
        //{
        //    string ext = Path.GetExtension(fullPath);
        //    string fileName = Path.GetFileName(fullPath);

        //    if (ext == ".tmp" || ext == ".crdownload") return;

        //    try
        //    {
        //        Thread.Sleep(interval);
        //        foreach (var dir in Dirs)
        //        {
        //            if (dir.Extensions.Any(ext.Contains))
        //            {
        //                Directory.Move(fullPath, Path.Combine(dir.FullPath, fileName));
        //                Console.WriteLine("Moved ('{0}') file to ('{1}')", fileName, Path.Combine(dir.FullPath, fileName));
        //                break;
        //            }
        //        }
        //    }
        //    catch (IOException e)
        //    {
        //        PrintException(e);
        //    }
        //    catch (Exception e)
        //    {
        //        PrintException(e);
        //    }

        //}

        private static void OnDeleted(object sender, FileSystemEventArgs e) =>
            Console.WriteLine($"Deleted: {e.FullPath}");

        private static void OnError(object sender, ErrorEventArgs e) =>
            PrintException(e.GetException());

        private static void PrintException(Exception ex)
        {
            if (ex != null)
            {
                Console.WriteLine($"Message: {ex.Message}");
                Console.WriteLine("Stacktrace:");
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine();
                PrintException(ex.InnerException);
            }
        }
    }
}
