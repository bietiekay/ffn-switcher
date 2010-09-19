using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using FFN_Switcher.Logging;
using System.Xml.Serialization;
using FFN_Switcher.Settings;


namespace FFN_Switcher.Settings
{
    public class SettingsProcessor
    {
        private String ConfigFilename;
        private Switcher internalSwitcherObject;

        /// <summary>
        /// Creates the Settings Processor
        /// </summary>
        /// <param name="Filename">The configfile Filename</param>
        public SettingsProcessor(String Filename, Switcher SwitcherObject)
        {
            ConfigFilename = Filename;
            internalSwitcherObject = SwitcherObject;
            internalSwitcherObject.AppSettingsProcessor = this;
        }

        #region SaveSettings
        /// <summary>
        /// this method saves the settings to the actual config file
        /// </summary>
        public void SaveSettings()
        {
            lock (this)
            {
                FileStream fs;

                ConsoleOutputLogger.WriteLine("[SETTINGS] Speichere Einstellungen...");

                #region Configuration

                fs = new FileStream(ConfigFilename + ".xml", FileMode.Create, FileAccess.Write);
                try
                {
                    System.Xml.Serialization.XmlSerializer xmls = new XmlSerializer(internalSwitcherObject.AppSettings.GetType());
                    xmls.Serialize(fs, internalSwitcherObject.AppSettings);
                }
                finally
                {
                    fs.Close();
                }
                #endregion
            }
        }
        #endregion

        #region LoadSettings
        /// <summary>
        /// guess what it does...it Loads the Settings from the Config File; it's usually only called at startup time
        /// </summary>
        public bool LoadSettings()
        {
            // TODO: Error checking; what happens when the config files are corrupt?
            lock (this)
            {
                FileStream fs;

                ConsoleOutputLogger.WriteLine("[SETTINGS] Lade Einstellungen...");

                if (File.Exists(ConfigFilename+".xml"))
                {
                    fs = new FileStream(ConfigFilename+ ".xml", FileMode.Open, FileAccess.Read);

                    try
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                        internalSwitcherObject.AppSettings = (Settings)serializer.Deserialize(fs);
                    }
                    finally
                    {
                        fs.Close();
                    }
                    return true;
                }
                else
                {
                    ConsoleOutputLogger.WriteLine("[SETTINGS] Konfiguration konnte nicht gefunden werden.");
                    return false;
                }
            }
        }
        #endregion

        #region Setting Checks
        /// <summary>
        /// this one checks the misc settings for plausability 
        /// </summary>
        /// <param name="settings"></param>
        /// <returns>a String with the error/warning messages</returns>
        public String CheckSettings()
        {
            StringBuilder CheckOutput = new StringBuilder();
            bool foundCriticalError = false;

            //// check if the ip adress is configured...
            //#region CriticalErrors
            //if (settings.HTTP_IPAdress == "0.0.0.0")
            //{
            //    foundCriticalError = true;
            //    CheckOutput.AppendLine("LoadSetting.MiscSettings Error: You HAVE TO set a valid IP Adress that YAPS is listening on! Do that by editing the YAPS.Settings.dat.xml file!");
            //}
            //#endregion


            #region React
            if (foundCriticalError)
            {
                Exception ex = new Exception(CheckOutput.ToString());
                throw ex;
            }
            else
            {
                ConsoleOutputLogger.WriteLine(CheckOutput.ToString());
            }
            #endregion

            return CheckOutput.ToString();
        }
        #endregion
    }

}
