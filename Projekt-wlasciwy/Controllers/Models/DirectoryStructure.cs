using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Projekt_wlasciwy
{
    /// <summary>
    /// Structure of Directory set by user
    /// </summary>
    public class DirectoryStructure
    {
        public string FullPath { get; set; }
        public string Name { get { return Path.GetFileName(FullPath); } }
        public string[] Extensions { get; set; }
        public long Size { get; set; }
        public long Files { get; set; }
        public DirectoryStructure() { }
        public DirectoryStructure(string path, string[] ext)
        {
            if (path == null) return;

            // Create directory if path doesn't exists
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            FullPath = path;
            Extensions = ext;
            GetInfo(FullPath);
        }

        public void Print()
        {
            Console.WriteLine($"Name: {Name}, \t Path: {FullPath}, \t Extensions:");
            foreach (string ext in Extensions) Console.WriteLine(ext);
        }

        private void GetInfo(string path)
        {
            IEnumerable<string> dirs;
            try
            {
                // Recursive way for each Directory in the path
                dirs = Directory.EnumerateDirectories(path);
                foreach (string dir in dirs)
                {
                    GetInfo(dir);
                }

                // How many files and paths to them
                dirs = Directory.EnumerateFiles(path, "*");
                Files += dirs.Count();

                foreach (string file in dirs)
                {
                    Size += new FileInfo(file).Length;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}