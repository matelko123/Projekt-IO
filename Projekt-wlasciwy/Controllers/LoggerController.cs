using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

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
                MessageBox.Show(ex.Message);
            }
            
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
