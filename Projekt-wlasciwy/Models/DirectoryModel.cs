using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Projekt_wlasciwy
{
    /// <summary>
    /// Structure of Directory set by user
    /// </summary>
    [XmlType("DirectoryModel")]
    public class DirectoryModel
    {
        public string FullPath { get; set; }
        public string Name { get { return Path.GetFileName(FullPath); } }
        public List<string> Extensions { get; set; }
        public long Size { get; set; } = 0;
        public long Files { get; set; } = 0;

        #region Constructors
        public DirectoryModel() { }

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
            return  $"Name: {Name}, \t Path: {FullPath}, \t Extensions: {Extensions}";
        }

        /// <summary>
        /// Get info about Directory by path
        /// </summary>
        /// <param name="path">Path to Directory</param>
        /// <param name="reset">Should reset Files and Size</param>
        public async Task GetAsyncInfo(string path, bool reset = false)
        {
            if(reset)
            {
                Files = 0;
                Size = 0;
            }

            IEnumerable<string> dirs;

            try
            {
                // Recursive way for each Directory in the path
                dirs = Directory.EnumerateDirectories(path);
                foreach (string dir in dirs)
                {
                     await GetAsyncInfo(dir);
                }

                dirs = Directory.EnumerateFiles(path, "*");
                Files += dirs.Count();

                // dirs.Select(file => Size += new FileInfo(file).Length);

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