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

        public override string ToString()
        {
            string result = $"Name: {Name}, \t Path: {FullPath}, \t Extensions: {Extensions}";
            /*foreach (string ext in Extensions)
                string.Concat(result, ext);*/

            return result;
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
        /// <summary>
        /// Calculate bytes for ex.: 2048B = 2kB
        /// </summary>
        /// <param name="size">Bytes</param>
        /// <returns>String of Bytes for ex.: 2kB</returns>
        public static string calcBytes(long size)
        {
            double sizes = size;
            string str = "B";

            if (sizes > 1024)
            {
                sizes /= 1024;
                str = "kB";
            }
            if (sizes > 1024)
            {
                sizes /= 1024;
                str = "MB";
            }
            if (sizes > 1024)
            {
                sizes /= 1024;
                str = "GB";
            }

            return string.Concat(Math.Round(sizes, 2), str);
        }
    }
}