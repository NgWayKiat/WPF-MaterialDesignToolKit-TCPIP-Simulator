using System.Configuration;

namespace WpfTemp.Model
{
    public class Configure
    {
        public static string GetAppSetting(string key, ref bool isExit)
        {
            isExit = true;

            if (ConfigurationManager.AppSettings[key] == null)
            {
                isExit = false;
                return string.Empty;
            }

            return ConfigurationManager.AppSettings[key].ToString();
        }

        public void SetAppSetting(string key, string value)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings[key].Value = value;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        public void AddAppSetting(string key, string value)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Add(key, value);
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        public static void GenerateGlobalSettings()
        {
            bool isExit = true;
            string sTemp = string.Empty;
            string[] arrSTemp;

            //Get MCC118_1
            sTemp = GetAppSetting("MCC118_1", ref isExit);

            if (isExit)
            {
                arrSTemp = sTemp.Split(':');
                if (arrSTemp.Length == 2)
                {
                    Global.sClientIp = arrSTemp[0];
                    Global.sDefaultClientIp = arrSTemp[0];
                    Int32.TryParse(arrSTemp[1], out int iPort);
                    Global.iClientPort = iPort;
                    Global.iDefaultClientPort = iPort;
                }
            }
        }
    }
}
