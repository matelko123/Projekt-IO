using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Projekt_wlasciwy
{
    public class DownloadController
    {
        // Time pause before moving 
        private static readonly int interval = 500;

        public static async Task CleanUp()
        {
            Console.WriteLine("Watcher is running...");

            // Find existing files
            var files = Directory.EnumerateFiles(SettingsController.DownloadFolder, "*").ToList();
            Console.WriteLine($"Found: {files.Count()} files.");

            foreach(var file in files)
            {
                Console.WriteLine(file);
                try
                {
                    await MoveFile(file);
                }
                catch(Exception ex)
                {
                    LoggerController.PrintException(ex);
                }
            }
            Console.WriteLine("Watcher is done.");
        }

        public static async Task MoveFile(string fullPath)
        {
            string ext = Path.GetExtension(fullPath);

            foreach(var dir in DirectoryController.Dirs)
            {
                if(dir.Extensions.Any(ext.Contains))
                {
                    await Task.Run(() => TryMoveFile(fullPath, dir.FullPath));
                }
            }
        }

        private static void TryMoveFile(string fullPath, string destinationPath)
        {
            string fileName = Path.GetFileName(fullPath);
            destinationPath = GetUniqueName(fullPath, destinationPath);
            string destinationDirectory = Path.GetDirectoryName(destinationPath);
            
            try
            {
                // if Destination directory doesn't exist.
                if(!Directory.Exists(destinationDirectory))
                    Directory.CreateDirectory(destinationDirectory);

                File.Move(fullPath, destinationPath);
                LoggerController.Log($"Moved ('{fileName}') file to ('{destinationDirectory}')");

            }
            catch(Exception e)
            {
                LoggerController.PrintException(e);
            }
        }

        private static string GetUniqueName(string fullPath, string destinationPath)
        {
            if(fullPath == "" || !File.Exists(fullPath) || destinationPath == Path.GetDirectoryName(fullPath))
                return fullPath;

            int count = 1;
            string fileNameOnly = Path.GetFileNameWithoutExtension(fullPath);
            string extension = Path.GetExtension(fullPath);
            string newFullPath = Path.Combine(destinationPath, fileNameOnly + extension) ;

            while(File.Exists(newFullPath))
            {
                string tempFileName = string.Format("{0} ({1})", fileNameOnly, count++);
                newFullPath = Path.Combine(destinationPath, tempFileName + extension);
            }

            return newFullPath;
        }


        public static void OnCreated(object sender, FileSystemEventArgs e)
        {
            string fullpath = e.FullPath;
            string ext = Path.GetExtension(fullpath);

            if(ext == ".tmp" || ext == ".crdownload")
                return;

            LoggerController.Log($"Created ('{fullpath}')");

            Thread.Sleep(interval);
            MoveFile(fullpath);
            CleanUp();
        }

        public static void OnRenamed(object sender, RenamedEventArgs e)
        {
            LoggerController.Log($"Renamed:\n Old: {e.OldFullPath}\nNew: {e.FullPath}");

            MoveFile(e.FullPath);
        }

        public static void OnDeleted(object sender, FileSystemEventArgs e) =>
            LoggerController.Log($"Deleted: {e.FullPath}");

        public static void OnError(object sender, ErrorEventArgs e) =>
            LoggerController.PrintException(e.GetException());
    }
}
