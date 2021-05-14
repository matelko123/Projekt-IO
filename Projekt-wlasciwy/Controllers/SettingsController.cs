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
        public static readonly string UserRoot = Environment.GetEnvironmentVariable("USERPROFILE");
        public static readonly string DownloadFolder = Path.Combine(UserRoot, "Downloads");

        private static readonly List<DirectoryModel> defaultDirs = new()
        {
            new DirectoryModel(Path.Combine(DownloadFolder, "Obrazy"), new List<string>() { ".jpeg", ".jpg", ".png" }),
            new DirectoryModel(Path.Combine(DownloadFolder, "Wideo"), new List<string>() { ".mp4", ".mp3" }),
            new DirectoryModel(Path.Combine(DownloadFolder, "Instalki"), new List<string>() { ".exe", ".msi" }),
            new DirectoryModel(Path.Combine(DownloadFolder, "Dokumenty"), new List<string>() { ".docx", ".txt", ".odt", ".xlsx", ".doc" }),
            new DirectoryModel(Path.Combine(DownloadFolder, "PDF"), new List<string>() { ".pdf" })
        };


        /// <summary>
        /// Read all value from settings
        /// </summary>
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
            catch(Exception e)
            {
                LoggerController.PrintException(e);
            }
        }

        /// <summary>
        /// Get searching value from given kay
        /// </summary>
        /// <param name="key">Searching key</param>
        /// <returns>Value of key</returns>
        public static string GetSettings(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                string result = appSettings[key] ?? "Not Found";
                return result;
            }
            catch (Exception e)
            {
                LoggerController.PrintException(e);
            }
            return "";
        }

        /// <summary>
        /// Update Value on given Key
        /// </summary>
        /// <param name="key">Key of setting</param>
        /// <param name="value">New value</param>
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
            XmlSerializer xmlSerializer = new(typeof(List<DirectoryModel>), new XmlRootAttribute("path"));

            using (StringWriter textWriter = new())
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
                using (StringReader textReader = new(toDeserialize))
                {
                    XmlSerializer deserializer = new(typeof(List<DirectoryModel>), new XmlRootAttribute("path"));
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

            if(deserializedData == null || deserializedData.Count == 0)
            {
                DirectoryController.Copy(defaultDirs);
            } 
            else
            {
                DirectoryController.Copy(deserializedData);
            }

            DirectoryController.PrintAll();
        }
    }
}
