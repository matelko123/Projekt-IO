using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt_wlasciwy
{
    /// <summary>
    /// Structure of Directory set by user
    /// </summary>
    public class DirectoryModel
    {
        /// <summary>
        /// Full path to directory
        /// </summary>
        public string FullPath { get; set; }

        /// <summary>
        /// Name of directory
        /// </summary>
        public string Name
        { 
            get 
            { 
                return Path.GetFileName(FullPath); 
            } 
        }

        /// <summary>
        /// Extensions used
        /// </summary>
        public List<string> Extensions { get; set; }

        /// <summary>
        /// Files count in directory and subdirectories
        /// </summary>
        public long Size { get; set; } = 0;

        /// <summary>
        /// Files size in directory and subdirectories
        /// </summary>
        public long Files { get; set; } = 0;

        #region Constructors
        public DirectoryModel() { }
        public DirectoryModel(string _FullPath)
        {
            if(_FullPath == null)
                return;

            // Create directory if path doesn't exists
            if(!Directory.Exists(_FullPath))
                Directory.CreateDirectory(_FullPath);

            FullPath = _FullPath;
            Extensions = new List<string>();
        }
        public DirectoryModel(string _FullPath, List<string> _Extensions)
        {
            if (_FullPath == null) return;

            // Create directory if path doesn't exists
            if (!Directory.Exists(_FullPath)) Directory.CreateDirectory(_FullPath);

            FullPath = _FullPath;
            Extensions = _Extensions;
        }
        #endregion

        /// <summary>
        /// Return DirectoryModel as a string
        /// </summary>
        /// <returns>Name + Path + Extensions</returns>
        public override string ToString()
        {
            string result = $"Name: {Name}, \t Path: {FullPath}, \t Extensions: ";
            foreach(string ext in Extensions)
                result += ext;

            return result;
        }

        /// <summary>
        /// Get info about Directory by path
        /// </summary>
        /// <param name="path">Path to Directory</param>
        /// <param name="reset">Should reset Files and Size</param>
        public async Task GetAsyncInfo(string path, bool reset = false)
        {
            if (path == null) path = FullPath;

            List<string> dirs = new List<string>();
            ConcurrentBag<Task> tasks = new ConcurrentBag<Task>();

            try
            {
                // Recursive way for each Directory in the path
                dirs = Directory.EnumerateDirectories(path).ToList();
                foreach (string dir in dirs)
                {
                     tasks.Add(GetAsyncInfo(dir));
                }

                await Task.WhenAll(tasks);

                dirs = Directory.EnumerateFiles(path, "*").ToList();
                Files += dirs.Count();

                foreach (string file in dirs)
                {
                    Size += new FileInfo(file).Length;
                }

            }
            catch (Exception ex)
            {
                LoggerController.PrintException(ex);
            }
        }

        /// <summary>
        /// Calculate bytes for ex.: 2048B = 2kB
        /// </summary>
        /// <param name="size">Bytes</param>
        /// <returns>String of Bytes for ex.: 2kB</returns>
        public static string CalcBytes(long size)
        {
            if (size == 0) return "0";

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