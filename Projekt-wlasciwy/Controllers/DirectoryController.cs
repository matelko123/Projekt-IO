using System.Collections.Generic;

namespace Projekt_wlasciwy
{
    class DirectoryController
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
        public void Save() { }
    }
}
