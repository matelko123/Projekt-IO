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


        public static async Task CleanUp()
        {
            Console.WriteLine("Watcher is running...");

            // Find existing files
            var files = Directory.EnumerateFiles(DownloadFolder, "*").ToList();
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
        }

        public static async Task MoveFile(string fullPath)
        {
            string ext = Path.GetExtension(fullPath);

            foreach(var dir in DirectoryController.Dirs)
            {
                if(dir.Extensions.Any(ext.Contains))
                {
                    await TryMoveFile(fullPath, dir.FullPath);
                }
            }
        }

        private static async Task TryMoveFile(string fullPath, string destinationPath)
        {
            string fileName = Path.GetFileName(fullPath);
            try
            {
                File.Move(fullPath, destinationPath);
                LoggerController.Log($"Moved ('{fileName}') file to ('{destinationPath}')");
            }
            // Podana ścieżka, nazwa pliku lub obie przekraczają maksymalną długość zdefiniowaną przez system.
            catch(PathTooLongException ptex)
            {
                LoggerController.PrintException(ptex);
            }
            // Plik o takiej nazwie już istnieje.
            catch(IOException ioe)
            {
                LoggerController.PrintException(ioe);
                if (!Directory.Exists(destinationPath)) Directory.CreateDirectory(destinationPath);
                else RenameFile(fullPath, destinationPath);
            }
            catch(Exception e)
            {
                LoggerController.PrintException(e);
            }
        }

        private static void RenameFile(string fullPath, string destinationPath)
        {
            if(fullPath == "" || !File.Exists(fullPath))
                return;

            int count = 1;
            string fileNameOnly = Path.GetFileNameWithoutExtension(fullPath);
            string extension = Path.GetExtension(fullPath);
            string newFullPath = fullPath;

            while(File.Exists(newFullPath))
            {
                string tempFileName = string.Format("{0} ({1})", fileNameOnly, count++);
                newFullPath = Path.Combine(destinationPath, tempFileName + extension);
            }

            TryMoveFile(fullPath, newFullPath);
        }


        public static void OnCreated(object sender, FileSystemEventArgs e)
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
