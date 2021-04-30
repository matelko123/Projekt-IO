using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace Projekt_wlasciwy
{
    /// <summary>
    /// Structure of Directory set by user
    /// </summary>
    [Serializable()]
    public class DirectoryStructure : ISerializable
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
            string result = $"Name: {Name}, \t Path: {FullPath}, \t Extensions:";
            foreach (string ext in Extensions)
                string.Concat(result, ext);

            return result;
        }

        /// <summary>
        /// Get info about Directory by path
        /// </summary>
        /// <param name="path">Path to Directory</param>
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

        /// <summary>
        /// Serialization function (Stores Object Data in File)
        /// </summary>
        /// <param name="info">SerializationInfo holds the key value pairs</param>
        /// <param name="context">StreamingContext can hold additional info</param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // Assign key value pair for your data
            info.AddValue("FullPath", FullPath);
            info.AddValue("Extensions", Extensions);
            info.AddValue("Size", Size);
            info.AddValue("Files", Files);
        }

        /// <summary>
        /// The deserialize function (Removes Object Data from File)
        /// </summary>
        /// <param name="info">SerializationInfo holds the key value pairs</param>
        /// <param name="ctxt">StreamingContext can hold additional info</param>
        public DirectoryStructure(SerializationInfo info, StreamingContext context)
        {
            //Get the values from info and assign them to the properties
            FullPath = (string)info.GetValue("FullPath", typeof(string));
            Extensions = (string[])info.GetValue("Extensions", typeof(string[]));
            Size = (long)info.GetValue("Size", typeof(long));
            Files = (long)info.GetValue("Files", typeof(long));
        }
    }
}