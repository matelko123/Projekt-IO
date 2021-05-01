using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Projekt_wlasciwy
{
    public class SettingsController
    {
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

        public static string serializeObject(List<DirectoryModel> toSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<DirectoryModel>), new XmlRootAttribute("path"));

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }
        }

        public static List<DirectoryModel> deserializeObject(string toDeserialize)
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
            var serializedData = await Task.Run(() => serializeObject(DirectoryController.Dirs));

            Update("Path", serializedData);
        }

        public async static Task LoadDataDir()
        {
            var data = GetSettings("Path");
            var deserializedData = await Task.Run(() => deserializeObject(data));
            DirectoryController.Dirs = deserializedData;
        }
    }
}
