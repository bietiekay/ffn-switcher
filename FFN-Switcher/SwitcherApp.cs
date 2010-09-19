/* Freies Funknetz Switcher Tool
 * Released under the Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported license; see license.txt and lizenz.txt
 * Freigegeben unter der Creative Commons Namensnennung-Keine kommerzielle Nutzung-Weitergabe unter gleichen Bedingungen 3.0 Unported Lizenz; siehe lizenz.txt und license.txt
 * (c) Daniel Kirstenpfad, 2008
 * 
 * Homepage: http://www.schrankmonster.de/CategoryView,category,freies%2BFunknetz.aspx
 * 
 * Programmierer:
 *      Daniel Kirstenpfad, btk@schrankmonster.de
 * 
 * Teamspeak Remote Wrapper:
 *      James Mimeault
 * */
#region Usings
using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Diagnostics;
using FFN_Switcher.Logging;
using FFN_Switcher.Settings;
#endregion

namespace FFN_Switcher
{
    public class FFN_Switcher_Console
    {
        public static void Shutdown()
        {
            #region Shutdown
            ConsoleOutputLogger.WriteLine("[SWITCHER] Herunterfahren...");
            ConsoleOutputLogger.ShutdownLog();
            #endregion
        }

        static void Main(string[] args)
        {
            #region Data
            Switcher Switcher = new Switcher();
            #endregion

            #region ConsoleOutputLogger
            ConsoleOutputLogger.verbose = true;
            ConsoleOutputLogger.writeLogfile = false;
            #endregion

            #region Logo
            ConsoleOutputLogger.WriteLine("[SWITCHER] Freies Funknetz Switcher Tool");
            ConsoleOutputLogger.WriteLine("[SWITCHER] Version " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());
            ConsoleOutputLogger.WriteLine("[SWITCHER] Licensed under Attribution-NonCommercial-ShareAlike 3.0 Unported license");
            ConsoleOutputLogger.WriteLine("[SWITCHER] (C) 2008 Daniel Kirstenpfad - http://www.schrankmonster.de");
            #endregion

            #region Configfile
            string ConfigFile = "ffn_switcher.config";

            if (args.Length > 0)
            {
                if (args[0] == "-?")
                {
                    ConsoleOutputLogger.WriteLine("Verwendung mit Parametern:");
                    ConsoleOutputLogger.WriteLine("");
                    ConsoleOutputLogger.WriteLine("ffn-switcher.exe -?                          -   diese Hilfe");
                    ConsoleOutputLogger.WriteLine("ffn-switcher.exe <Konfigurations-Dateiname>  -   eine Konfigurationdatei");
                    ConsoleOutputLogger.WriteLine("                                                 (wird erstellt wenn sie noch nicht existiert)");
                    return;
                }
                else
                {
                    ConfigFile = args[0];
                }
            }
            ConsoleOutputLogger.WriteLine("[SWITCHER] Verwende Konfigurationsdatei: " + ConfigFile);
            #endregion


            #region load settings
            SettingsProcessor Settings_Processor = new SettingsProcessor(ConfigFile, Switcher);
            if (!Settings_Processor.LoadSettings())
            {
                ConsoleOutputLogger.WriteLine("[SWITCHER] Erstelle neue Konfiguration und versuche diese zu laden...");
                Settings_Processor.SaveSettings();
                if (!Settings_Processor.LoadSettings())
                {
                    ConsoleOutputLogger.WriteLine("[SWITCHER] Konnte keine neue Konfiguration erstellen - gebe auf.");
                    Shutdown();
                    return;
                }
            }
            else
            {
                ConsoleOutputLogger.verbose = Switcher.AppSettings.Logging.LoggingToScreenEnabled;
                if (!Switcher.AppSettings.Logging.LoggingToFileEnabled)
                {
                    ConsoleOutputLogger.writeLogfile = false;
                }
                else
                {
                    ConsoleOutputLogger.Logfilename = Switcher.AppSettings.Logging.Logfile;
                    ConsoleOutputLogger.writeLogfile = true;
                }
            }
            #endregion

            #region save settings just to updated them
            Settings_Processor.SaveSettings();
            #endregion

            #region start Switcher...
            Switcher.Run();
            #endregion

            #region Shutdown
            Shutdown();
            #endregion
        }
    }
}
