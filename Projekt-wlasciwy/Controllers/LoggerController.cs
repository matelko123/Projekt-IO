using System;
using System.IO;

namespace Projekt_wlasciwy
{
    class LoggerController
    {
        public static string path = Path.Combine(Path.GetTempPath(), "logger.txt");

        public static void Log(string text)
        {
            try
            {
                using(StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine($"| {DateTime.Now} |\n>> {text}\n\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }

        public static void LogError(string text)
        {
            ErrorController.ThrowUserError(text);
            Log(text);
        }

        public static void PrintException(Exception ex)
        {
            if(ex == null)
                return;
            
            Log("Message: " + ex.Message + "\n\t Stacktrace: " + ex.StackTrace + "\n\n");
            PrintException(ex.InnerException);
            
        }
    }
}
