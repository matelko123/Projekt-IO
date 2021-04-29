﻿using System;
using System.IO;
using System.Text;

namespace Projekt_wlasciwy
{
    class LoggerController
    {
        private static string path = Path.Combine(Path.GetTempPath(), "logger.txt");

        public static void Log(string text)
        {
            using (FileStream fs = File.Create(path))
            {
                AddText(fs, text);
            }
            string result = Path.GetTempPath();
            Console.WriteLine(result);
        }

        private static void AddText(FileStream fs, string value)
        {
            byte[] info = new UTF8Encoding(true).GetBytes(value);
            fs.Write(info, 0, info.Length);
        }

        public static void PrintException(Exception ex)
        {
            if (ex != null)
            {
                Log($"Message: {ex.Message}");
                Log("Stacktrace:");
                Log(ex.StackTrace);
                PrintException(ex.InnerException);
            }
        }
    }
}