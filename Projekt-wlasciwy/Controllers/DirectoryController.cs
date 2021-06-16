using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Projekt_wlasciwy
{
    public class DirectoryController
    {
        public static List<DirectoryModel> Dirs = new List<DirectoryModel>();

        public static void PrintAll()
        {
            if (Dirs is null || Dirs.Count <= 0) return;

            foreach(var Dir in Dirs)
            {
                Console.WriteLine(Dir);
            }
        }

        public static void Load()
        {
            Dirs.Add(new DirectoryModel(Path.Combine(SettingsController.DownloadFolder, "Obrazy"), new List<string>() { ".jpeg", ".jpg", ".png" }));
            Dirs.Add(new DirectoryModel(Path.Combine(SettingsController.DownloadFolder, "Wideo"), new List<string>() { ".mp4", ".mp3" }));
            Dirs.Add(new DirectoryModel(Path.Combine(SettingsController.DownloadFolder, "Instalki"), new List<string>() { ".exe", ".msi" }));
            Dirs.Add(new DirectoryModel(Path.Combine(SettingsController.DownloadFolder, "Dokumenty"), new List<string>() { ".docx", ".txt", ".odt", ".xlsx", ".doc" }));
            Dirs.Add(new DirectoryModel(Path.Combine(SettingsController.DownloadFolder, "PDF"), new List<string>() { ".pdf" }));
            Console.WriteLine("Loaded default data directory.");
        }

        public static async Task Copy(List<DirectoryModel> _copy)
        {
            if(_copy.Count == 0 && Dirs.Count == 0)
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
