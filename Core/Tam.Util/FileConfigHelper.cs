using System;
using System.Configuration;
using System.IO;

namespace Tam.Util
{
    internal class FileConfigHelper
    {
        private string fileConfigPath;
        private Configuration config = null;

        public bool IsLoadFileConfigSuccess { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="filePath">Path of file config. Ex: ABC.exe.config</param>
        public FileConfigHelper(string filePath)
        {
            fileConfigPath = filePath;
            this.IsLoadFileConfigSuccess = LoadFileConfig();
        }

        private bool LoadFileConfig()
        {
            try
            {
                if (File.Exists(fileConfigPath) == false)
                {
                    throw new FileNotFoundException("Can not find " + fileConfigPath);
                }
                var fileMap = new ExeConfigurationFileMap();
                fileMap.ExeConfigFilename = Path.GetFullPath(fileConfigPath);
                config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetAppSettingValueWithoutDecrypt(string key)
        {
            try
            {
                if (config.AppSettings.Settings[key] == null)
                {
                    throw new Exception("AppSetting " + key + " is not found");
                }
                return config.AppSettings.Settings[key].Value.ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SetAppSetingValue(string key, string value)
        {
            try
            {
                if (config.AppSettings.Settings[key] == null)
                {
                    config.AppSettings.Settings.Add(key, value);
                    config.Save(ConfigurationSaveMode.Full);
                }
                else
                {
                    config.AppSettings.Settings[key].Value = value;
                    config.Save(ConfigurationSaveMode.Modified);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}