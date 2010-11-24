using System;
using System.Collections.Generic;
using System.Text;
using FFN_Switcher.Settings;
using FFN_Switcher.Logging;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace FFN_Switcher.HTTP
{
    #region HttpServer
    /// <summary>
    /// Implements a HTTP Server Listener
    /// </summary>
    public class HttpServer
    {
        #region Data
        //public Settings.Settings internalSettings;
        private string docRoot;
        public Socket listener;
        public TemplateProcessor Template_Processor;
        #endregion

        #region Construction
        public HttpServer()
        {
            //internalSettings = Settings;
        }
        #endregion

        #region Listener
        /// <summary>
        /// Create a new server socket, set up all the endpoints, bind the socket and then listen
        /// </summary>
        public void listen()
        {
            // Wait for VCRScheduler...
            //ConsoleOutputLogger.WriteLine("HTTP Server is waiting for VCRScheduler...");
            //while (!internal_vcr_scheduler_set)
            //{
            //    Thread.Sleep(10);
            //}
            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipaddress = IPAddress.Parse(FFN_Switcher.Properties.Settings.Default.WebserverListeningIP);
            IPEndPoint endpoint = new IPEndPoint(ipaddress, Convert.ToInt32(FFN_Switcher.Properties.Settings.Default.WebserverTCPPort));

            try
            {
                // Create a new server socket, set up all the endpoints, bind the socket and then listen
                listener.Bind(endpoint);
                listener.Blocking = true;
                listener.Listen(-1);
                ConsoleOutputLogger.WriteLine("[HTTP] Administrationsoberfläche unter http://" + FFN_Switcher.Properties.Settings.Default.WebserverListeningIP + ":" + Convert.ToInt32(FFN_Switcher.Properties.Settings.Default.WebserverTCPPort) + " erreichbar.");
                while (true)
                {
                    try
                    {
                        // Accept a new connection from the net, blocking till one comes in
                        Socket s = listener.Accept();

                        // Create a new processor for this request
                        HttpProcessor processor = new HttpProcessor(s);


                        // Dispatch that processor in its own thread
                        Thread thread = new Thread(new ThreadStart(processor.process));
                        thread.Start();
                        Thread.Sleep(10);
                        //processor.process();

                    }
                    catch (NullReferenceException)
                    {
                        // Don't even ask me why they throw this exception when this happens
                        ConsoleOutputLogger.WriteLine("[FEHLER@HTTP] Kann nicht auf TCP-Port " + Convert.ToInt32(FFN_Switcher.Properties.Settings.Default.WebserverTCPPort) + " verbinden - wird vermutlich schon benutzt.");
                    }
                }
            }
            catch (Exception e)
            {
                ConsoleOutputLogger.WriteLine("[FEHLER@HTTP] " + e.Message);
            }
        }
        #endregion
    }
    #endregion
}
