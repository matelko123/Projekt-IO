using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Projekt_wlasciwy
{
    public class DownloadManager
    {
        public class Dirs
        {
            public string path;
            public string fullName;
            public string[] extensions;
            public Dirs() { }
            public Dirs(string path, string[] ext)
            {
                if (path == null) return;
                // Jeśli dany katalog nie istnieje to go tworzy
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                this.path = path;
                this.fullName = Path.GetFileName(this.path);
                this.extensions = ext;
            }
            public void print()
            {
                Console.WriteLine($"Name: {fullName}, \t Path: {path}, \t Extensions:");
                foreach (string ext in extensions) Console.WriteLine(ext);
            }
        }

        // Get user path do \Download directory
        private static string userRoot = Environment.GetEnvironmentVariable("USERPROFILE");
        private static string downloadFolder = Path.Combine(userRoot, "Downloads");
        private static SettingsManager sm = new SettingsManager();

        private static int interval = 500;           // time pause before move file

        public static List<Dirs> dirs = new List<Dirs>();
        /*private static void loadData(List<Dirs> dirs)
        {
            dirs.Add(new Dirs(Path.Combine(downloadFolder, "Obrazy"), new string[] { ".jpg", ".jpeg", ".png" }));
            dirs.Add(new Dirs(Path.Combine(downloadFolder, "Dokumenty"), new string[] { ".docx", ".txt" }));
            dirs.Add(new Dirs(Path.Combine(downloadFolder, "PDF"), new string[] { ".pdf" }));
            dirs.Add(new Dirs(Path.Combine(downloadFolder, "Video"), new string[] { ".mp4" }));
            dirs.Add(new Dirs(Path.Combine(downloadFolder, "Programs"), new string[] { ".exe" }));
        }*/

        public DownloadManager()
        {
            Console.WriteLine("Running");

            //loadData(dirs);
            dirs.Add(new Dirs(Path.Combine(downloadFolder, "Obrazy"), new string[] { ".jpg", ".jpeg", ".png" }));
            dirs.Add(new Dirs(Path.Combine(downloadFolder, "Dokumenty"), new string[] { ".docx", ".txt" }));
            sm.AddUpdateAppSettings("paths", sm.serializeObject(dirs[0]));
            Console.WriteLine(sm.serializeObject(dirs));

            // Watcher
            FileSystemWatcher watcher = new FileSystemWatcher(downloadFolder)
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

            watcher.Filter = "*.*";     // Dla wszystkich plików
            watcher.IncludeSubdirectories = false;
            watcher.EnableRaisingEvents = true;
            // foreach (var dir in dirs) Console.WriteLine("I am watching {0}, size: {1:F}MB, {2} files", dir.path, getDirSize(dir.path), getDirFiles(dir.path));

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

        private static void MoveFile(string fullPath)
        {
            string ext = Path.GetExtension(fullPath);
            string fileName = Path.GetFileName(fullPath);

            if (ext == ".tmp" || ext == ".crdownload") return;

            try
            {
                Thread.Sleep(interval);
                foreach (var dir in dirs)
                {
                    if (dir.extensions.Any(ext.Contains))
                    {
                        Directory.Move(fullPath, Path.Combine(dir.path, fileName));
                        Console.WriteLine("Moved ('{0}') file to ('{1}')", fileName, Path.Combine(dir.path, fileName));
                        break;
                    }
                }
            }
            catch (System.IO.IOException e)
            {
                PrintException(e);
            }
            catch (Exception e)
            {
                PrintException(e);
            }

        }

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
