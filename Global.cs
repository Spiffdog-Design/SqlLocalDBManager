using System.Configuration;

namespace Spiffdog.SqlLocalDbManager
{
    public static class Global
    {
        private static string _sqlLocalDbPath = "";
        private static string _sqlLocalDbPathDefault = "";

        private static string _sqlLocalDbPathKeyName = "Paths.SqlLocalDb";
        private static string _sqlLocalDbPathDefaultKeyName = "Paths.SqlLocalDb.Default";

        public static string SqlLocalDbPath
        {
            get { return _sqlLocalDbPath; }
            set
            {
                _sqlLocalDbPath = value;
                SetAppConfigSetting(_sqlLocalDbPathKeyName, value);
            }
        }

        public static string SqlLocalDbDefaultPath
        {
            get {
                if (string.IsNullOrEmpty(_sqlLocalDbPath)) {
                    _sqlLocalDbPath = ConfigurationManager.AppSettings[_sqlLocalDbPathDefaultKeyName];
                }
                return _sqlLocalDbPathDefault;
            }
        }

        public static void Load()
        {
            SqlLocalDbPath = ConfigurationManager.AppSettings[_sqlLocalDbPathKeyName];
        }

        internal static bool SetAppConfigSetting(string key, string value)
        {
            try {
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                config.AppSettings.Settings.Remove(key);
                var kvElem = new KeyValueConfigurationElement(key, value);
                config.AppSettings.Settings.Add(kvElem);

                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");

                return true;
            } catch {
                return false;
            }
        } // function
    }
}
