using System;
using System.Collections.Generic;

namespace Projekt_wlasciwy
{
    public class DirectoryController
    {
        public static List<DirectoryModel> Dirs = new List<DirectoryModel>();

        public static void PrintAll()
        {
            if (Dirs == null) return;

            foreach(var Dir in Dirs)
            {
                Console.WriteLine(Dir);
            }
        }

        public static void LoadDataFromSettings()
        {
            var data = SettingsController.GetSettings("Path");
            if (data == null) return;

            Console.WriteLine($"XML Data: {data}");

            var d = SettingsController.DeserializeObject(data);
            if (d == null) return;
            Console.WriteLine($"d: {d}");

            // Dirs = d;
        }

        public static void SaveDataToSettings() 
        {
            string data = SettingsController.SerializeObject(Dirs);
            SettingsController.Update("Path", data);
        }
    }
}
