using System;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using FFN_Switcher.Logging;
using System.Runtime.InteropServices;
using FFN_Switcher.Teamspeak;

namespace FFN_Switcher.Processors
{
    // TODO: wenn in anderen channel verschoben sagt gateway selbst nichtsmehr (man kann nicht überprüfen obs noch rauscht)
    // TODO: Modus damit man gateway "offiziell" in anderen channel verschieben kann
    // TODO: restart vom Teamspeak bei unerwartetem fehler

    class TeamspeakProcessor
    {
        #region Data
        public bool done;
        public Settings.Settings internalSettings;
        public bool paused;
        public bool shutdowndone;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public TeamspeakProcessor(Settings.Settings Settings)
        {
            internalSettings = Settings;
            done = false;
            paused = false;
            shutdowndone = false;
        }
        #endregion

        #region Helper Methods

        #region OpenTeamspeakClient
        public static int OpenTeamspeakClient(String TeamspeakClientURL)
        {
            Process proc = null;
            Process[] procs = null;

            try
            {
                if (File.Exists(TeamspeakClientURL))
                {
                    ConsoleOutputLogger.WriteLine("[TS] Starting Teamspeak...");
                    proc = new Process();
                    proc.StartInfo.FileName = TeamspeakClientURL;
                    proc.StartInfo.Verb = proc.StartInfo.Verbs[0];
                    proc.Start();
                }
            }
            catch (Exception e)
            {
                throw (e);
            }
            return (proc == null ? -1 : 0);
        }
        #endregion

        #region DLLImports
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, IntPtr windowTitle);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hwnd, int wMsg,
            IntPtr wParam,
            IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg,
            int wParam,
            StringBuilder lParam);

        private const int WM_GETTEXT = 0x000D;
        private const int WM_CLOSE = 0x0010;
        private const int WM_GETTEXTLENGTH = 0x000E;
        #endregion

        #region GetText
        private static string GetText(IntPtr hwnd)
        {
            int lngLength;
            StringBuilder strBuffer = new StringBuilder();
            int lngRet;

            lngLength = SendMessage(hwnd, WM_GETTEXTLENGTH, IntPtr.Zero, IntPtr.Zero) + 1;

            strBuffer.Capacity = lngLength;

            lngRet = SendMessage(hwnd, WM_GETTEXT, strBuffer.Capacity, strBuffer);
            if (lngRet > 0)
                return strBuffer.ToString().Substring(0, lngRet);
            return
                null;
        }
        #endregion

        #region TeamspeakClientRunning
        public bool TeamspeakClientRunning()
        {
            IntPtr windowHandle = FindWindowEx(IntPtr.Zero, IntPtr.Zero, "TMainForm", "TeamSpeak 2");

            if (windowHandle == IntPtr.Zero)
                return false;
            else
                return true;
        }
        #endregion
        
        #region GetLastLinesFromTeamspeak
        private String LastLine = null;

        public List<String> GetLastLinesFromTeamSpeak()
        {
            List<String> Output = new List<string>();
            String RawText = "";

            try
            {
                IntPtr windowHandle = FindWindowEx(IntPtr.Zero, IntPtr.Zero, "TMainForm", "TeamSpeak 2");
                IntPtr childHandle;

                IntPtr PanelHandle = FindWindowEx(windowHandle, IntPtr.Zero, "TPanel", IntPtr.Zero);
                IntPtr PanelHandle2 = FindWindowEx(windowHandle, PanelHandle, "TPanel", IntPtr.Zero);
                //try to get a handle to IE's toolbar container
                childHandle = FindWindowEx(PanelHandle2, IntPtr.Zero, "TRichEditWithLinks", IntPtr.Zero);
                if (childHandle != IntPtr.Zero)
                {
                    RawText = GetText(childHandle);
                }
                #region Extract each line from the Teamspeak 2 Textwindow contents
                String[] splitter = new String[1];
                String[] RawTextLines = null;

                splitter[0] = "\r\n";

                if ((RawText != "") && (RawText != null))
                {
                    RawTextLines = RawText.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
                }

                #endregion
                if (RawTextLines != null)
                {
                    for (int i = RawTextLines.Length - 1; i >= 0; i--)
                    {
                        if (RawTextLines[i] == LastLine)
                        {
                            break;
                        }
                        else
                        {
                            Output.Insert(0, RawTextLines[i]);
                        }
                    }
                }
                #region SetLastLine
                if (Output.Count > 0)
                {
                    LastLine = Output[Output.Count - 1];
                }
                #endregion
            }
            catch (Exception e)
            {
                //ConsoleOutputLogger.WriteLine("[FEHLER@TS] " + e.Message);
            }

            return Output;// strUrlToReturn;
        }
        #endregion

        #region GenerateConnectString
        public String GenerateConnectString(String GatewayName, String ServerURL, String Nickname, String Loginname, String Password)
        {
            #region Connect / Reconnect to Server
            // build Connect-String
            String ConnectString = "Teamspeak://" + ServerURL;

            #region Nickname
            if (internalSettings.Gateway.Nickname == "")
                ConnectString = ConnectString + "?Nickname=" + GatewayName;
            else
                ConnectString = ConnectString + "?Nickname=" + Nickname;
            #endregion

            #region Loginname
            if (internalSettings.Gateway.TSServer1.LoginName != "")
                ConnectString = ConnectString + "?loginname=" + Loginname;
            #endregion

            #region Password
            if (internalSettings.Gateway.TSServer1.Password != "")
                ConnectString = ConnectString + "?password=" + Password;
            #endregion

            #endregion

            return ConnectString;
        }
        #endregion

        #region KillTeamspeak
        public void RestartTeamspeak()
        {
            #region Kill it
            if (TeamspeakClientRunning())
            {
                //Process[] localByName = Process.GetProcessesByName("teamspeak");

                //foreach (Process p in localByName)
                //{
                //    Microsoft.Win32.RegistryKey RegKey = Microsoft.Win32.Registry.ClassesRoot;
                //    Microsoft.Win32.RegistryKey RegValue = RegKey.OpenSubKey("teamspeak\\Shell\\Open\\command");
                //    String Value = (string)RegValue.GetValue("");

                //    if (Value.Contains(p.MainModule.FileName))
                //    {
                //        ConsoleOutputLogger.WriteLine("[TS] Beende Teamspeak Client...");
                //        p.Kill();
                //    }
                //}
                IntPtr windowHandle = FindWindowEx(IntPtr.Zero, IntPtr.Zero, "TMainForm", "TeamSpeak 2");
                
                SendMessage(windowHandle, WM_CLOSE, 0, new StringBuilder(""));
            }
            #endregion

            ConsoleOutputLogger.WriteLine("[TS] Warte auf Teamspeak Client...");
            Thread.Sleep(10000);

            #region Start it
            ConsoleOutputLogger.WriteLine("[TS] Starte Teamspeak Client wieder...");
            OpenTeamspeakClient(internalSettings.TSClient.TeamSpeakClientURL);
            #endregion
        }
        #endregion

        #endregion

        #region Thread
        /// <summary>
        /// TeamSpeak Processor Thread
        /// </summary>
        public void TeamSpeakProcessorThread()
        {
            ConsoleOutputLogger.WriteLine("[TS] Gestartet.");

            if (internalSettings.TSClient.StartTeamspeakClientAtStartup)
            {
                RestartTeamspeak();
            }

            #region Beacon
            //ConsoleOutputLogger.WriteLine("[SWITCHER] Starting Beacon Processor...");
            BeaconProcessor beaconProcessor = new BeaconProcessor(internalSettings);
            Thread beaconThread = new Thread(new ThreadStart(beaconProcessor.BeaconProcessorThread));
            beaconThread.Start();
            #endregion

            #region Serial Port
            //ConsoleOutputLogger.WriteLine("[SWITCHER] Starting Serialport Processor...");
            SerialportProcessor serialportProcessor = new SerialportProcessor(internalSettings);
            if (internalSettings.Gateway.ComportEnabled)
            {
                Thread serialThread = new Thread(new ThreadStart(serialportProcessor.SerialportProcessorThread));
                serialThread.Start();
            }
            #endregion

            #region local variables
            bool _PauseDisplayed = false;
            ClientFlags TeamspeakFlags = null;
            Int32 ConnectedToServerNumber = 0;
            bool ConnectedToServer = false;
            Int32[] currentlySpeaking = null;
            bool movedByOtherUser = false;
            bool previouslyPaused = false;
            Int32 CurrentShouldChannelID;
            String CurrentShouldChannelPW;
            Int32 MyOwnID;
            Boolean CurrentlySending = false;
            DateTime StartedToSend = DateTime.Now;
            Int32 NumberOfFailedReconnects = 0;
            DateTime LastBeacon = DateTime.Now;
            bool IamSpeaking = false;
            DateTime IamSpeakingStarted = DateTime.Now;
            bool NobodySpeakingRunning = false;
            DateTime NobodySpeaking = DateTime.Now;
            DateTime PenaltyStarted = DateTime.Now;
            bool CurrentlyOnPenalty = false;
            bool ChannelCOmmanderShouldStayOn = false;
            #endregion

            while (!done)
            {
                #region Paused
                if (paused)
                    previouslyPaused = true;
                while (paused)
                {
                    Thread.Sleep(1000);
                }
                if (previouslyPaused)
                    ConsoleOutputLogger.WriteLine("[TS] Teamspeak Client pausiert nicht mehr.");

                if (!internalSettings.Gateway.isActive)
                {
                    paused = true;
                    ConsoleOutputLogger.WriteLine("[TS] Gateway ist NICHT-aktiv geschaltet.");
                }
                #endregion

                #region check if client is running, if not, pause...restart
                if (_PauseDisplayed)
                    ConsoleOutputLogger.WriteLine("[TS] Teamspeak Client läuft...");

                _PauseDisplayed = false;
                
                while (!TeamspeakClientRunning())
                {
                    if (!_PauseDisplayed)
                    {
                        ConsoleOutputLogger.WriteLine("[TS] Teamspeak Client läuft nicht (mehr). Warte bis er wieder läuft...");
                        _PauseDisplayed = true;
                        ConnectedToServer = false;
                        movedByOtherUser = false;
                        CurrentlyOnPenalty = false;

                        RestartTeamspeak();

                        // Play DisConnect Sound if allowed
                        if (internalSettings.Beacon.PlayWhenOffline)
                        {
                            serialportProcessor.SwitchOn();
                                beaconProcessor.Play(internalSettings.Beacon.GatewayOfflineFile);
                            serialportProcessor.SwitchOff();
                        }
                    }
                    Thread.Sleep(1000);
                }
                #endregion

                if (ConnectedToServer)
                {
                    #region do stuff when connected to server

                    #region Check if we're in right channel
                    #region Get Info from Settings
                    if (ConnectedToServerNumber == 1)
                    {
                        CurrentShouldChannelID = internalSettings.Gateway.TSServer1.ChannelID;
                        CurrentShouldChannelPW = internalSettings.Gateway.TSServer1.ChannelPW;
                    }
                    else
                    {
                        CurrentShouldChannelID = internalSettings.Gateway.TSServer2.ChannelID;
                        CurrentShouldChannelPW = internalSettings.Gateway.TSServer2.ChannelPW;
                    }
                    #endregion

                    // check if we're in this channel...
                    if ((TSRemote.UserInfo.Channel.ChannelID == CurrentShouldChannelID) | (CurrentlyOnPenalty) )
                    {
                        #region Right Channel
                        // we're already in the channel we should be...

                        #region We're back in the right channel...
                        if ((TSRemote.UserInfo.Channel.ChannelID == CurrentShouldChannelID) && (CurrentlyOnPenalty))
                        {
                            ConsoleOutputLogger.WriteLine("[TS] Gateway ist wieder im richtigen Channel.");
                            CurrentlyOnPenalty = false;
                            movedByOtherUser = true;
                            if (internalSettings.Gateway.WhenMovedToOtherChannelOutputMute)
                                TeamspeakFlags.OutputMuted = false;
                        }
                        #endregion

                        #region Check if we have voice
                        if (ConnectedToServer)
                        {
                            
                        }
                        #endregion

                        #region check Whispered Flag
                        if (ConnectedToServerNumber == 2)
                        {
                            if (internalSettings.Gateway.TSServer2.WhisperBlocked)
                            {
                                if (TeamspeakFlags.Whisper == false)
                                    TeamspeakFlags.Whisper = true;
                            }
                        }
                        else
                        {
                            if (internalSettings.Gateway.TSServer1.WhisperBlocked)
                            {
                                if (TeamspeakFlags.Whisper == false)
                                    TeamspeakFlags.Whisper = true;
                            }
                        }
                        #endregion

                        #region check ChannelCommanderFlag
                        if (ConnectedToServerNumber == 2)
                        {
                            if (internalSettings.Gateway.TSServer2.AlwaysChannelCommander)
                            {
                                if (TeamspeakFlags.ChannelCommander == false)
                                    TeamspeakFlags.ChannelCommander = true;
                                ChannelCOmmanderShouldStayOn = true;
                            }
                            else
                            {
                                ChannelCOmmanderShouldStayOn = false;
                            }
                        }
                        else
                        {
                            if (internalSettings.Gateway.TSServer1.AlwaysChannelCommander)
                            {
                                if (TeamspeakFlags.ChannelCommander == false)
                                    TeamspeakFlags.ChannelCommander = true;
                                ChannelCOmmanderShouldStayOn = true;
                            }
                            else
                                ChannelCOmmanderShouldStayOn = false;
                        }
                        #endregion


                        #region monitor text messages
                        List<String> newText = GetLastLinesFromTeamSpeak();
                        foreach (String line in newText)
                        {
                            ConsoleOutputLogger.WriteLine("[TS] " + line);
                            if (line.Contains("Undefined internal error"))
                            {
                                ConsoleOutputLogger.WriteLine("[TS] Vermutlich ist der Teamspeak Client hängengeblieben - er meldet uns 'Undefined internal error'");
                                ConsoleOutputLogger.WriteLine("[TS] Schliesse Teamspeak Client und starte ihn neu...");
                                RestartTeamspeak();
                            }
                        }
                        #endregion

                        #region monitor who is talking...
                        MyOwnID = TSRemote.UserInfo.Player.PlayerID;
                        currentlySpeaking = TSRemote.SpeakerIds;
                        if ((currentlySpeaking != null) && (currentlySpeaking.Length > 0))
                        {
                            if (!CurrentlySending)
                            {
                                bool foundMySelf = false;

                                foreach (Int32 speaker in currentlySpeaking)
                                {
                                    if ((speaker != MyOwnID) && (IamSpeaking == false))
                                    {
                                        // anyone else is talking, stop doing things and listen! (activate hf + shut up)
                                        ConsoleOutputLogger.WriteLine("[TS] " + TSRemote.GetPlayerInfo(speaker).NickName + " spricht im Channel - Schalte Gateway auf Sendung");
                                        //...
                                        serialportProcessor.SwitchOn();
                                        StartedToSend = DateTime.Now;
                                        CurrentlySending = true;
                                    }

                                    if ((speaker == MyOwnID))
                                    {
                                        foundMySelf = true;
                                    }
                                }

                                if (foundMySelf)
                                {
                                    // I am speaking...
                                    if (IamSpeaking == false)
                                    {
                                        IamSpeakingStarted = DateTime.Now;
                                        IamSpeaking = true;
                                        ConsoleOutputLogger.WriteLine("[TS] Gateway spricht selbst - sende in Teamspeak Channel");

                                        if (internalSettings.Gateway.FlagChannelCommanderWhenSending)
                                        {
                                            if (!TeamspeakFlags.ChannelCommander)
                                                TeamspeakFlags.ChannelCommander = true;
                                        }
                                    }

                                    if ((IamSpeakingStarted - DateTime.Now).TotalMinutes >= internalSettings.Gateway.WatchdogMinutes)
                                    {
                                        // when the gateway talked for too long...
                                        // TODO...
                                    }
                                }
                                else
                                {
                                    if (IamSpeaking)
                                    {
                                        if (!ChannelCOmmanderShouldStayOn)
                                        {
                                            if (internalSettings.Gateway.FlagChannelCommanderWhenSending)
                                            {
                                                TeamspeakFlags.ChannelCommander = false;
                                            }
                                        }
                                        ConsoleOutputLogger.WriteLine("[TS] Gateway spricht nichtmehr selbst - Durchgangsdauer: " + (DateTime.Now - IamSpeakingStarted).TotalSeconds + " Sekunden");
                                        IamSpeaking = false;
                                    }
                                }
                            }
                            else
                            {
                                if (NobodySpeakingRunning == true)
                                {
                                    ConsoleOutputLogger.WriteLine("[TS] ACHTUNG: Bitte Sprechpause einhalten !");
                                    NobodySpeakingRunning = false;
                                    NobodySpeaking = DateTime.Now;
                                }
                            }
                        }
                        else
                        {
                            if (IamSpeaking)
                            {

                                if (!ChannelCOmmanderShouldStayOn)
                                    TeamspeakFlags.ChannelCommander = false;
                                ConsoleOutputLogger.WriteLine("[TS] Gateway spricht nichtmehr selbst - Durchgangsdauer: " + (DateTime.Now - IamSpeakingStarted).TotalSeconds + " Sekunden");
                                IamSpeaking = false;                                
                            }

                            if ( (CurrentlySending) && ((DateTime.Now-StartedToSend).TotalSeconds >= internalSettings.Gateway.DelaySeconds) )
                            {
                                if ((TSRemote.SpeakerIds != null) && (TSRemote.SpeakerIds.Length > 0))
                                {
                                    // somebody is speaking...
                                    ConsoleOutputLogger.WriteLine("[TS] ACHTUNG: Bitte Sprechpause einhalten !");
                                    
                                }
                                else
                                {
                                    // nobody is speaking...
                                    if (!NobodySpeakingRunning)
                                    {
                                        NobodySpeakingRunning = true;
                                        NobodySpeaking = DateTime.Now;
                                        ConsoleOutputLogger.WriteLine("[TS] Niemand spricht mehr...warte "+internalSettings.Gateway.DelayAfterSendingMilliSeconds+" Millisekunden auf Nachtasten...");
                                    }
                                    else
                                    {
                                        if ((DateTime.Now - NobodySpeaking).TotalMilliseconds >= internalSettings.Gateway.DelayAfterSendingMilliSeconds) 
                                        {
                                            ConsoleOutputLogger.WriteLine("[TS] Durchgang beendet - schalte Gateway auf Empfang - Durchgangsdauer: " + (DateTime.Now - StartedToSend).TotalSeconds + " Sekunden");
                                            CurrentlySending = false;
                                            NobodySpeakingRunning = false;
                                            if (internalSettings.Beacon.PlayRogerBeep)
                                                beaconProcessor.Play(internalSettings.Beacon.RogerBeepFile);
                                            serialportProcessor.SwitchOff();

                                            ConsoleOutputLogger.WriteLine("[TS] Gateway wieder freigegeben.");
                                        }

                                        // stop sending!
/*                                        if (internalSettings.Gateway.DelayAfterSendingSeconds > 0)
                                        {
                                            ConsoleOutputLogger.WriteLine("[TS] Warte noch " + internalSettings.Gateway.DelayAfterSendingSeconds + " Sekunden vor Gateway Freigabe.");
                                            Thread.Sleep(internalSettings.Gateway.DelayAfterSendingSeconds * 1000);
                                            ConsoleOutputLogger.WriteLine("[TS] Gateway wieder freigegeben.");
                                        }*/
                                    }
                                }
                            }
                        }
                        #endregion

                        #region Check if we should play the beacon...
                        if (internalSettings.Beacon.ReplayGatewayBeacon)
                        {
                            if ((DateTime.Now - LastBeacon).TotalMinutes >= internalSettings.Beacon.GatewayBeaconReplayInMinutes)
                            {
                                if (!CurrentlySending)
                                {
                                    ConsoleOutputLogger.WriteLine("[TS] Zeit für Bake ist erreicht - Spiele Bake ab.");
                                    
                                    if (internalSettings.Beacon.MuteOutputWhenPlayingBeacon)
                                        TeamspeakFlags.OutputMuted = true;
                                    
                                    serialportProcessor.SwitchOn();
                                        beaconProcessor.Play(internalSettings.Beacon.GatewayBeaconFile);
                                    serialportProcessor.SwitchOff();
                                    
                                    if (internalSettings.Beacon.MuteOutputWhenPlayingBeacon)
                                        TeamspeakFlags.OutputMuted = false;

                                    LastBeacon = DateTime.Now;
                                }
                            }
                        }
                        #endregion

                        #endregion
                    }
                    else
                    {
                        #region Wrong Channel

                        #region have we lost server connection?
                        if (TSRemote.ServerInfo.ChannelCount == 0)
                        {
                            // check again 5 times...
                            int checkker = 0;
                            bool immernochdisconnected = true;

                            ConsoleOutputLogger.WriteLine("[TS] Teamspeak meldet dass wir die Verbindung verloren haben - ich glaube ihm nicht...");

                            while (checkker < internalSettings.Gateway.NumberOfChecksIfTeamspeakIsLying)
                            {
                                ConsoleOutputLogger.WriteLine("[TS] Überprüfe...");
                                if (TSRemote.ServerInfo.ChannelCount != 0)
                                {
                                    ConsoleOutputLogger.WriteLine("[TS] HA! Teamspeak hat uns angelogen!");
                                    ConnectedToServer = true;
                                    //movedByOtherUser = false;
                                    immernochdisconnected = false;
                                    break;
                                }
                                else
                                {
                                    ConsoleOutputLogger.WriteLine("[TS] Teamspeak meldet immernoch disconnected...");
                                }
                                checkker++;
                                Thread.Sleep(1000);
                            }

                            if (immernochdisconnected)
                            {

                                ConsoleOutputLogger.WriteLine("[TS] Verbindung zum Teamspeak Server verloren! Resette Teamspeak-Prozessor...");

                                done = true;


                                // Play DisConnect Sound if allowed
                                if (internalSettings.Beacon.PlayAtDisconnect)
                                {
                                    serialportProcessor.SwitchOn();
                                    beaconProcessor.Play(internalSettings.Beacon.DisconnectFile);
                                    serialportProcessor.SwitchOff();
                                }

                                if (internalSettings.Gateway.AutoReconnect)
                                {
                                    ConsoleOutputLogger.WriteLine("[TS] Versuche in " + internalSettings.Gateway.AutoReconnectSeconds + " Sekunden wieder zu verbinden...");
                                    Thread.Sleep(internalSettings.Gateway.AutoReconnectSeconds * 1000);
                                }
                                else
                                {
                                    ConsoleOutputLogger.WriteLine("[TS] Versuche nicht wieder zu verbinden. Switcher pausiert. Bitte manuell wieder aktivieren!");
                                    paused = true;
                                }

                                ConnectedToServer = false;
                                movedByOtherUser = false;
                            }
                        }
                        #endregion

                        // we're obviously not in the channel were we should be
                        #region check if we're on penalty
                        if ((movedByOtherUser) && (ConnectedToServer) && (!CurrentlyOnPenalty))
                        {

                            // Play Disabled Sound if allowed
                            if (internalSettings.Beacon.PlayWhenDisabled)
                            {
                                serialportProcessor.SwitchOn();
                                    beaconProcessor.Play(internalSettings.Beacon.GatewayDisabledFile);
                                serialportProcessor.SwitchOff();
                            }

                            // check if we're already connected or not...
                            ConsoleOutputLogger.WriteLine("[TS] Der Switcher wurde in einen anderen Channel verschoben!");
                            ConsoleOutputLogger.WriteLine("[TS] Wechsle in "+internalSettings.Gateway.WaitPenaltyMinutes+" Minuten wieder zurück in den Channel.");
                            
                            if (internalSettings.Gateway.WhenMovedToOtherChannelOutputMute)
                                TeamspeakFlags.OutputMuted = true;

                            //Thread.Sleep(internalSettings.Gateway.WaitPenaltyMinutes * 1000 * 60);
                            PenaltyStarted = DateTime.Now;
                            CurrentlyOnPenalty = true;
                        }
                        else
                        {
                            if (CurrentlyOnPenalty)
                            {
                                #region CurrentlyOnPenalty
                                if ((DateTime.Now - PenaltyStarted).TotalMinutes >= internalSettings.Gateway.WaitPenaltyMinutes)
                                {
                                    if (internalSettings.Gateway.JumpBackToChannelAfterWaitPenaltyMinutes)
                                    {
                                        ConsoleOutputLogger.WriteLine("[TS] Wartezeit zuende.");
                                        movedByOtherUser = false;
                                        CurrentlyOnPenalty = false;
                                        if (internalSettings.Gateway.WhenMovedToOtherChannelOutputMute)
                                            TeamspeakFlags.OutputMuted = false;
                                    }
                                    else
                                    {
                                        PenaltyStarted = DateTime.Now;
                                    }
                                }
                            }
                            else
                            {
                                ConsoleOutputLogger.WriteLine("[TS] Wechsle in Channel " + CurrentShouldChannelID);
                                Int32 ChannelSwitching = TSRemote.SwitchChannels(CurrentShouldChannelID, CurrentShouldChannelPW);
                                if (ChannelSwitching == -1)
                                    ConnectedToServer = false;
                                if (ChannelSwitching == 20)
                                {
                                    ConsoleOutputLogger.WriteLine("[TS] Wollte in Channel wechseln, aber Channel existiert nicht. Bitte überprüfen!");
                                    ConsoleOutputLogger.WriteLine("[TS] Teamspeak Client pausiert.");
                                    paused = true;
                                }
                                // we successfully switched!!!
                                if (ChannelSwitching == 0)
                                {
                                    ConsoleOutputLogger.WriteLine("[TS] Teamspeak meldet dass wir in Channel mit ID " + CurrentShouldChannelID+ " gewechselt haben.");
                                    // Play enabled Sound if allowed
                                    if (internalSettings.Beacon.PlayAtConnect)
                                    {
                                        if (internalSettings.Beacon.MuteOutputWhenPlayingBeacon)
                                            TeamspeakFlags.OutputMuted = true;
                                        
                                        serialportProcessor.SwitchOn();
                                        beaconProcessor.Play(internalSettings.Beacon.ConnectFile);
                                        serialportProcessor.SwitchOff();
                                        
                                        if (internalSettings.Beacon.MuteOutputWhenPlayingBeacon)
                                            TeamspeakFlags.OutputMuted = false;
                                    }

                                    // if we're now moved we we're moved by another user...
                                    movedByOtherUser = true;

                                    Thread.Sleep(500);
                                }
                                #endregion
                            }
                        }
                        #endregion

                        #endregion
                    }
                    #endregion

                    #endregion
                }
                else
                {
                    #region Connect to Server
                    Thread.Sleep(1000);

                    ConsoleOutputLogger.WriteLine("[TS] Verbinde mit "+internalSettings.Gateway.TSServer1.ServerURL);
                    ConsoleOutputLogger.WriteLine("[TS] Warte " + internalSettings.Gateway.WaitForConnectSeconds+ " Sekunden auf Teamspeak Client Rückmeldung...");
                    // connect
                    TSRemote.Connect(GenerateConnectString(internalSettings.Gateway.GatewayName,internalSettings.Gateway.TSServer1.ServerURL,internalSettings.Gateway.Nickname,internalSettings.Gateway.TSServer1.LoginName,internalSettings.Gateway.TSServer1.Password));
                    ConnectedToServerNumber = 1;
                    // auf Login warten...
                    Int32 Counter = 0;
                    while (TSRemote.Channels.Length == 0)
                    {
                        ConsoleOutputLogger.WriteLine("Channels: " + TSRemote.Channels.Length);
                        if (Counter >= internalSettings.Gateway.WaitForConnectSeconds)
                        {
                            ConsoleOutputLogger.WriteLine("[TS] Lange genug auf Channel-Liste gewartet...Verbinde erneut.");
                            break;
                        }
                        Thread.Sleep(1000);
                        Counter++;
                    }

                    // check if connected
                    if (TSRemote.Channels.Length == 0)
                    {
                        NumberOfFailedReconnects++;

                        RestartTeamspeak();

                        if (internalSettings.Gateway.UseServer2AfterNumberOfFailedReconnects == 0)
                            internalSettings.Gateway.UseServer2AfterNumberOfFailedReconnects = 2;

                        if (NumberOfFailedReconnects == internalSettings.Gateway.UseServer2AfterNumberOfFailedReconnects)
                        {
                            NumberOfFailedReconnects = 0;
                            ConnectedToServer = false;
                            // Try second server...
                            ConsoleOutputLogger.WriteLine("[TS] Konnte mit " + internalSettings.Gateway.TSServer1.ServerURL + " nicht verbinden.");
                            if (internalSettings.Gateway.TSServer2.ServerURL != "")
                            {
                                ConsoleOutputLogger.WriteLine("[TS] Verbinde mit alternativ Server " + internalSettings.Gateway.TSServer2.ServerURL);
                                TSRemote.Connect(GenerateConnectString(internalSettings.Gateway.GatewayName, internalSettings.Gateway.TSServer1.ServerURL, internalSettings.Gateway.Nickname, internalSettings.Gateway.TSServer1.LoginName, internalSettings.Gateway.TSServer1.Password));
                                ConnectedToServerNumber = 2;
                            }
                        }
                    }

                    if (TSRemote.Channels.Length != 0)
                    {
                        ConnectedToServer = true;
                        TeamspeakFlags = new ClientFlags();
                        
                        ConsoleOutputLogger.WriteLine("[TS] Verbunden mit " + TSRemote.ServerInfo.ServerName);
                        ConsoleOutputLogger.WriteLine("[TS]               " + TSRemote.ServerInfo.ChannelCount + " Kanäle");
                        ConsoleOutputLogger.WriteLine("[TS]               " + TSRemote.ServerInfo.PlayerCount + " Benutzer");
                        
                        // set default flags
                        TeamspeakFlags.OutputMuted = false;
                        TeamspeakFlags.InputMuted = false;
                    }
                    else
                    {
                        ConnectedToServerNumber = 0;
                        ConsoleOutputLogger.WriteLine("[TS] Konnte mit keinem Teamspeak Server verbinden. Warte 5 Minuten und versuche dann nochmal.");
                        Thread.Sleep(5 * 60 * 1000);
                    }
                    #endregion
                }

                Thread.Sleep(500);  // it seems to be important to wait in some cases...
            }

            beaconProcessor.done = true;
            serialportProcessor.done = true;
            ConsoleOutputLogger.WriteLine("[TS] Warte auf BeaconProcessor");
            while (!beaconProcessor.shutdowndone)
            {
                Thread.Sleep(1);
            }
            ConsoleOutputLogger.WriteLine("[TS] BeaconProcessor heruntergefahren.");
            ConsoleOutputLogger.WriteLine("[TS] Warte auf SerialPortProcessor");
            while (!serialportProcessor.shutdowndone)
            {
                Thread.Sleep(1);
            }
            ConsoleOutputLogger.WriteLine("[TS] SerialPortProcessor heruntergefahren.");
            ConsoleOutputLogger.WriteLine("[TS] Beendet.");
            shutdowndone = true;
        }
        #endregion
    }
}
