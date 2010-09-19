using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using FFN_Switcher.Logging;
using System.IO.Ports;

namespace FFN_Switcher.Processors
{
    public class SerialportProcessor
    {
        #region Data
        public bool done;
        public bool shutdowndone;
        //public Settings.Settings internalSettings;
        private bool Enabled;
        private bool Enable;
        private bool Disable;
        private SerialPort Port;
        public bool paused;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public SerialportProcessor()
        {
            //internalSettings = Settings;
            done = false;
            Enabled = false;
            Enable = false;
            Disable = true;
            paused = false;
            Port = null;
            shutdowndone = false;
        }
        #endregion

        #region SwitchOn
        public void SwitchOn()
        {
            if (!Enabled)
                Enable = true;
        }
        #endregion

        #region SwitchOff
        public void SwitchOff()
        {
            if (Enabled)
                Disable = true;
        }
        #endregion

        #region Init
        public void Init()
        {
            try
            {
                try
                {
                    if (Port != null)
                    {
                        ConsoleOutputLogger.WriteLine("[COM] COM-Port soll reinitialisiert werden - schliesse alten COM-Port.");
                        Port.Close();
                    }
                }
                catch (Exception e)
                {
                    ConsoleOutputLogger.WriteLine("[FEHLER@COM] Beim Versuch den COM-Port zu schliessen ist ein Fehler aufgetreten.");
                    ConsoleOutputLogger.WriteLine("[FEHLER@COM] "+e.Message);
                }

                Port = new SerialPort("COM"+Convert.ToInt32(FFN_Switcher.Properties.Settings.Default.Comport));
                Port.Open();
                ConsoleOutputLogger.WriteLine("[COM] COM-Port " + Convert.ToInt32(FFN_Switcher.Properties.Settings.Default.Comport) + " geöffnet und bereit.");
            }
            catch (Exception e)
            {
                ConsoleOutputLogger.WriteLine("[FEHLER@COM] " + e.Message);
                ConsoleOutputLogger.WriteLine("[COM] Kann COM-Port nicht öffnen - bitte Konfiguration überprüfen.");
                Pause();
            }
        }
        #endregion

        #region Pause
        public void Pause()
        {
            if (!paused)
            {
                paused = true;
                ConsoleOutputLogger.WriteLine("[COM] COM-Port Steuerung pausiert.");
            }
        }
        #endregion

        #region UnPause
        public void UnPause()
        {
            if (paused)
            {
                paused = false;
                ConsoleOutputLogger.WriteLine("[COM] COM-Port Steuerung startet wieder.");
            }
        }
        #endregion

        #region Thread
        /// <summary>
        /// SerialportProcessor Thread
        /// </summary>
        public void SerialportProcessorThread()
        {
            ConsoleOutputLogger.WriteLine("[COM] Gestartet.");
            Init();
            while (!done)
            {
                while((paused) && (!done))
                {
                    Thread.Sleep(100);
                }
                
                if (Enable)
                {
                    Enabled = true;
                    Enable = false;
                    if (FFN_Switcher.Properties.Settings.Default.RTS)
                    {
                        ConsoleOutputLogger.WriteLine("[COM] Aktiviere RTS Leitung an COM-Port "+FFN_Switcher.Properties.Settings.Default.Comport);
                        Port.RtsEnable = true;
                    }
                    else
                    {
                        ConsoleOutputLogger.WriteLine("[COM] Aktiviere DTR Leitung an COM-Port " + FFN_Switcher.Properties.Settings.Default.Comport);
                        Port.DtrEnable = true;
                    }
                }
                #region Disable
                if (Disable)
                {
                    Disable = false;
                    if (FFN_Switcher.Properties.Settings.Default.RTS)
                    {
                        ConsoleOutputLogger.WriteLine("[COM] Deaktiviere RTS Leitung an COM-Port " + FFN_Switcher.Properties.Settings.Default.Comport);
                        Port.RtsEnable = false;
                    }
                    else
                    {
                        ConsoleOutputLogger.WriteLine("[COM] Deaktiviere DTR Leitung an COM-Port " + FFN_Switcher.Properties.Settings.Default.Comport);
                        Port.DtrEnable = false;
                    }
                    Enabled = false;
                }
                #endregion
                Thread.Sleep(100);
            }
            try
            {
                ConsoleOutputLogger.WriteLine("[COM] COM-Port Steuerung fährt herunter - schliesse COM-Port.");
                if (Port != null) 
                    Port.Close();
                shutdowndone = true;
            }
            catch (Exception e)
            {
                ConsoleOutputLogger.WriteLine("[FEHLER@COM] " + e.Message);
            }
            ConsoleOutputLogger.WriteLine("[COM] Beendet.");
        }
        #endregion
    }
}
