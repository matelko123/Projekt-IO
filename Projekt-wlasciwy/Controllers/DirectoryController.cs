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
    }
}
