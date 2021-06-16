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
        public static string UserRoot = Environment.GetEnvironmentVariable("USERPROFILE");
        public static string DownloadFolder = Path.Combine(UserRoot, "Downloads");

        public static void ReadAllSettings()
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;

                if (appSettings.Count == 0)
                {
                    Console.WriteLine("AppSettings is empty.");
                    return;
                }
                
                foreach (var key in appSettings.AllKeys)
                {
                    Console.WriteLine("Key: {0} Value: {1}", key, appSettings[key]);
                }
            }
            catch (ConfigurationErrorsException)
            {
                LoggerController.Log("Error reading app settings");
            }
        }

        public static string GetSettings(string key)
        {
            try
            {
                return ConfigurationManager.AppSettings[key];
            }
            catch (ConfigurationErrorsException)
            {
                LoggerController.Log("Error reading app settings");
            }
            return null;
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
                LoggerController.Log("Error writing app settings");
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
            if(toDeserialize is null)
                return null;

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

        public static async Task SaveDataDir()
        {
            DirectoryController.Dirs = DirectoryController.Dirs.Where(dir => dir != null).ToList();

            var serializedData = await Task.Run(() => SerializeObject(DirectoryController.Dirs));

            Update("Path", serializedData);
        }

        public static async Task LoadDataDir()
        {
            var data = GetSettings("Path");
            if(data is null)
                DirectoryController.Load();
            else
            {
                var deserializedData = await Task.Run(() => DeserializeObject(data));
                await DirectoryController.Copy(deserializedData);
            }
        }
    }
}
