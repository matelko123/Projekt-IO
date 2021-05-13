using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Projekt_wlasciwy
{
    public class SettingsController
    {
        private static string UserRoot = Environment.GetEnvironmentVariable("USERPROFILE");
        private static string DownloadFolder = Path.Combine(UserRoot, "Downloads");

        public static void ReadAllSettings()
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;

                if (appSettings.Count == 0)
                {
                    Console.WriteLine("AppSettings is empty.");
                }
                else
                {
                    foreach (var key in appSettings.AllKeys)
                    {
                        Console.WriteLine("Key: {0} Value: {1}", key, appSettings[key]);
                    }
                }
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error reading app settings");
            }
        }

        public static string GetSettings(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                string result = appSettings[key] ?? "Not Found";
                return result;
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error reading app settings");
            }
            return "";
        }

        public static void Update(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }
        }

        public static string SerializeObject(List<DirectoryModel> toSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<DirectoryModel>), new XmlRootAttribute("path"));

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }
        }

        public static List<DirectoryModel> DeserializeObject(string toDeserialize)
        {
            List<DirectoryModel> list;
            try
            {
                //XmlSerializer xmlSerializer = new XmlSerializer(typeof(DirectoryModel));
                using (StringReader textReader = new StringReader(toDeserialize))
                {
                    XmlSerializer deserializer = new XmlSerializer(typeof(List<DirectoryModel>), new XmlRootAttribute("path"));
                    list = (List<DirectoryModel>)deserializer.Deserialize(textReader);
                    return list;
                }
            }
            catch (Exception e)
            {
                LoggerController.PrintException(e);
            }
            return null;
        }

        public async static Task SaveDataDir()
        {
            DirectoryController.Dirs = DirectoryController.Dirs.Where(dir => dir != null).ToList();

            var serializedData = await Task.Run(() => SerializeObject(DirectoryController.Dirs));

            Update("Path", serializedData);
        }

        public async static Task LoadDataDir()
        {
            var data = GetSettings("Path");
            var deserializedData = await Task.Run(() => DeserializeObject(data));
            DirectoryController.Copy(deserializedData);

            if(DirectoryController.Dirs == null || DirectoryController.Dirs.Count == 0)
            {
                DirectoryController.Dirs.Add(new DirectoryModel(Path.Combine(DownloadFolder, "Obrazy"), new List<string>() { ".jpeg", ".jpg", ".png" }));
                DirectoryController.Dirs.Add(new DirectoryModel(Path.Combine(DownloadFolder, "Wideo"), new List<string>() { ".mp4", ".mp3" }));
                DirectoryController.Dirs.Add(new DirectoryModel(Path.Combine(DownloadFolder, "Instalki"), new List<string>() { ".exe", ".msi" }));
                DirectoryController.Dirs.Add(new DirectoryModel(Path.Combine(DownloadFolder, "Dokumenty"), new List<string>() { ".docx", ".txt", ".odt", ".xlsx", ".doc" }));
                DirectoryController.Dirs.Add(new DirectoryModel(Path.Combine(DownloadFolder, "PDF"), new List<string>() { ".pdf" }));
            }

            DirectoryController.PrintAll();
        }
    }
}
