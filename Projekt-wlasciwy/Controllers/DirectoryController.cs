using System;
using System.Collections.Generic;

namespace Projekt_wlasciwy
{
    public class DirectoryController
    {
        public static List<DirectoryModel> Dirs = new List<DirectoryModel>();

        public static void PrintAll()
        {
            foreach(var Dir in Dirs)
            {
                Console.WriteLine(Dir);
            }
        }

        public static void LoadDataFromSettings()
        {
            var data = SettingsController.GetSettings("Paths");
            if (data == null) return;

            Console.WriteLine($"XML Data: {data}");

            var d = SettingsController.deserializeObject(data);
            if (d == null) return;
            Console.WriteLine($"d: {d}");

            Dirs = d;
        }

        public static void SaveDataToSettings() 
        {
            string data = SettingsController.serializeObject(Dirs);
            SettingsController.AddUpdateAppSettings("Paths", data);
        }
    }
}
