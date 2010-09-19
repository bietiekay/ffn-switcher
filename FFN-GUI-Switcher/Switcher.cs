using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using FFN_Switcher.Logging;
using FFN_Switcher.Processors;

namespace FFN_Switcher
{
    public class Switcher
    {
        #region lokale Daten und Konstruktor
        //public Settings.Settings AppSettings;
        public Settings.SettingsProcessor AppSettingsProcessor;
        public HTTP.HttpServer httpServer;
        private Thread http_server_thread;
        private Thread teamspeakThread;
        public bool done = false;
        public TeamspeakProcessor tsProcessor;

        public Switcher()
        {
            //AppSettings = new FFN_Switcher.Settings.Settings();
        }
        #endregion

        public void Shutdown()
        {
            done = true;
            tsProcessor.done = true;
            httpServer.listener.Close();
        }

        #region der eigentliche switcher
        public void Run()
        {
            //bool done = false; // fertig?

            #region internal HTTP Server
            //ConsoleOutputLogger.WriteLine("[SWITCHER] Starte internen Webserver...");
            httpServer = new FFN_Switcher.HTTP.HttpServer();
            http_server_thread = new Thread(new ThreadStart(httpServer.listen));
            http_server_thread.Start();
            #endregion

            #region Teamspeak
            //ConsoleOutputLogger.WriteLine("[SWITCHER] Starting Teamspeak Processor...");
            tsProcessor = new TeamspeakProcessor();
            teamspeakThread = new Thread(new ThreadStart(tsProcessor.TeamSpeakProcessorThread));
            teamspeakThread.Start();
            #endregion

            ConsoleOutputLogger.WriteLine("[SWITCHER] Gestartet.");

            while (!done)
            {
                try
                {
                    if (tsProcessor.done)
                    {
                        ConsoleOutputLogger.WriteLine("[SWITCHER] Teamspeak-Processor resettet...");

                        while (!tsProcessor.shutdowndone)
                        {
                            Thread.Sleep(1);
                        }

                        tsProcessor = new TeamspeakProcessor();
                        teamspeakThread = new Thread(new ThreadStart(tsProcessor.TeamSpeakProcessorThread));
                        teamspeakThread.Start();
                    }
                    Thread.Sleep(1000);   
                }
                catch (Exception e)
                {
                    ConsoleOutputLogger.WriteLine("[FEHLER@SWITCHER] "+e.Message);
                }
            }

            tsProcessor.done = true;
            //beaconProcessor.done = true;
            //serialportProcessor.done = true;

            Thread.Sleep(1000);
            ConsoleOutputLogger.WriteLine("[SWITCHER] Beendet.");
        }
        #endregion
    }
}
