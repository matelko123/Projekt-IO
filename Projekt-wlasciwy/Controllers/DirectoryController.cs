using System.Collections.Generic;

namespace Projekt_wlasciwy
{
    public class DirectoryController
    {
        public List<DirectoryStructure> Dirs = new List<DirectoryStructure>();

        public void PrintAll()
        {
            foreach(var Dir in Dirs)
            {
                Dir.Print();
            }
        }

        public void Load() { }
        public void Save() 
        {
            string data = SettingsManager.serializeObject(Dirs);
            SettingsManager.AddUpdateAppSettings("Paths", data);
        }
    }
}
