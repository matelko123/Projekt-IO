using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Projekt_wlasciwy
{
    public class DirectoryController
    {
        private static string UserRoot = Environment.GetEnvironmentVariable("USERPROFILE");
        private static string DownloadFolder = Path.Combine(UserRoot, "Downloads");

        public static List<DirectoryModel> Dirs = new List<DirectoryModel>();

        public static void PrintAll()
        {
            if (Dirs is null) return;

            foreach(var Dir in Dirs)
            {
                Console.WriteLine(Dir);
            }
        }

        public static void Load()
        {
            DirectoryController.Dirs.Add(new DirectoryModel(Path.Combine(DownloadFolder, "Obrazy"), new List<string>() { ".jpeg", ".jpg", ".png" }));
            DirectoryController.Dirs.Add(new DirectoryModel(Path.Combine(DownloadFolder, "Wideo"), new List<string>() { ".mp4", ".mp3" }));
            DirectoryController.Dirs.Add(new DirectoryModel(Path.Combine(DownloadFolder, "Instalki"), new List<string>() { ".exe", ".msi" }));
            DirectoryController.Dirs.Add(new DirectoryModel(Path.Combine(DownloadFolder, "Dokumenty"), new List<string>() { ".docx", ".txt", ".odt", ".xlsx", ".doc" }));
            DirectoryController.Dirs.Add(new DirectoryModel(Path.Combine(DownloadFolder, "PDF"), new List<string>() { ".pdf" }));
            Console.WriteLine("Loaded default data directory.");
        }

        public static async Task Copy(List<DirectoryModel> _copy)
        {
            if(_copy.Count == 0 && DirectoryController.Dirs.Count == 0)
            {
                await Task.Run(() => Load());
            }
            else
            {
                foreach(DirectoryModel dir in _copy)
                {
                    Dirs.Add(new DirectoryModel(dir.FullPath, dir.Extensions));
                }
            }
        }
    }
}
