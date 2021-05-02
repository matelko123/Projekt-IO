using System;
using System.IO;
using System.Text;

namespace Projekt_wlasciwy
{
    class LoggerController
    {
        public static string path = Path.Combine(Path.GetTempPath(), "logger.txt");

        public static void Log(string text)
        {
            using (StreamWriter sw = File.AppendText(path))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(text);
                sw.WriteLine(info);
            }
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
