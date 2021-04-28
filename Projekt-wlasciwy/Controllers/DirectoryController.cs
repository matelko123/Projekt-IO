using System;
using System.Collections.Generic;

namespace Projekt_wlasciwy
{
    public class DirectoryController
    {
        public static List<DirectoryStructure> Dirs = new List<DirectoryStructure>();

        public static void PrintAll()
        {
            foreach(var Dir in Dirs)
            {
                Dir.Print();
            }
        }

        public static void Load()
        {
            var data = SettingsController.GetSettings("Paths");
            if (data == null) return;

            var d = SettingsController.deserializeObject(data);
            if (d == null) return;

            d.Print();
        }

        public static void Save() 
        {
            string data = SettingsController.serializeObject(Dirs);
            SettingsController.AddUpdateAppSettings("Paths", data);
        }
    }
}
