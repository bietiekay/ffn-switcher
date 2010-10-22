using System;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using FFN_Switcher.Logging;
using System.Runtime.InteropServices;
using FFN_Switcher.Teamspeak;
using System.Media;

namespace FFN_Switcher.Processors
{
    // TODO: wenn in anderen channel verschoben sagt gateway selbst nichtsmehr (man kann nicht überprüfen obs noch rauscht)
    // TODO: Modus damit man gateway "offiziell" in anderen channel verschieben kann
    // TODO: restart vom Teamspeak bei unerwartetem fehler

    public class TeamspeakProcessor
    {
        #region Data
        public bool done;
        //public Settings.Settings internalSettings;
        public bool paused;
        public bool shutdowndone;
        public BeaconProcessor beaconProcessor;
        public SerialportProcessor serialportProcessor;
        public ClientFlags TeamspeakFlags = null;
        public ChannelFlags TeamspeakChannelFlags = null;
        public List<String> PlayBeacons = null;
        public DateTime whenGatewaywasDeactivated = DateTime.MinValue;
        public DateTime whenGatewaywasOffline = DateTime.MinValue;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public TeamspeakProcessor()
        {
            //internalSettings = Settings;
            done = false;
            paused = false;
            shutdowndone = false;
            PlayBeacons = new List<string>();
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
            if (FFN_Switcher.Properties.Settings.Default.Nickname == "")
                ConnectString = ConnectString + "?Nickname=" + GatewayName;
            else
                ConnectString = ConnectString + "?Nickname=" + Nickname;
            #endregion

            #region Loginname
            if (FFN_Switcher.Properties.Settings.Default.TSServer1_LoginName != "")
                ConnectString = ConnectString + "?loginname=" + Loginname;
            #endregion

            #region Password
            if (FFN_Switcher.Properties.Settings.Default.TSServer1_Password != "")
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
            
            Int32 waiting = 0;

            while ((waiting < 10) && (!done) )
            {
                Thread.Sleep(1000);
                waiting++;
            }

            #region Start it
            ConsoleOutputLogger.WriteLine("[TS] Starte Teamspeak Client wieder...");
            OpenTeamspeakClient(FFN_Switcher.Properties.Settings.Default.TeamspeakClientURL);
            #endregion
        }
        #endregion

        public void AddPlayBacon(String File)
        {
            lock (PlayBeacons)
            {
                PlayBeacons.Add(File);
            }
        }
        #endregion

        #region Thread
        /// <summary>
        /// TeamSpeak Processor Thread
        /// </summary>
        public void TeamSpeakProcessorThread()
        {
            ConsoleOutputLogger.WriteLine("[TS] Gestartet.");

            if (FFN_Switcher.Properties.Settings.Default.StartTeamspeakClientAtStartup)
            {
                RestartTeamspeak();
            }

            #region Beacon
            //ConsoleOutputLogger.WriteLine("[SWITCHER] Starting Beacon Processor...");
            beaconProcessor = new BeaconProcessor();
            Thread beaconThread = new Thread(new ThreadStart(beaconProcessor.BeaconProcessorThread));
            beaconThread.Start();
            #endregion

            #region Serial Port
            //ConsoleOutputLogger.WriteLine("[SWITCHER] Starting Serialport Processor...");
            serialportProcessor = new SerialportProcessor();
            if (FFN_Switcher.Properties.Settings.Default.ComportEnabled)
            {
                Thread serialThread = new Thread(new ThreadStart(serialportProcessor.SerialportProcessorThread));
                serialThread.Start();
            }
            #endregion

            #region local variables
            bool _PauseDisplayed = false;
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
            bool haveVoice = false;

            bool voicegenommen = false;
            bool voicewiederda = false;
            #endregion

            while (!done)
            {
                #region Paused
                if (paused)
                    previouslyPaused = true;
                while ((paused) && (!done))
                {
                    Thread.Sleep(1000);
                }
                if (previouslyPaused)
                    ConsoleOutputLogger.WriteLine("[TS] Teamspeak Client pausiert nicht mehr.");

                if (!FFN_Switcher.Properties.Settings.Default.GatewayActivated)
                {
                    paused = true;
                    ConsoleOutputLogger.WriteLine("[TS] Gateway ist NICHT-aktiv geschaltet.");
                }
                #endregion

                #region check if client is running, if not, pause...restart
                if (_PauseDisplayed)
                    ConsoleOutputLogger.WriteLine("[TS] Teamspeak Client läuft...");

                _PauseDisplayed = false;
                
                while ( (!TeamspeakClientRunning() && (!done)))
                {
                    if (!_PauseDisplayed)
                    {
                        ConsoleOutputLogger.WriteLine("[TS] Teamspeak Client läuft nicht (mehr). Warte bis er wieder läuft...");
                        _PauseDisplayed = true;
                        ConnectedToServer = false;
                        movedByOtherUser = false;
                        CurrentlyOnPenalty = false;

                        RestartTeamspeak();

                        // Play Disconnect Sound if allowed
                        if (FFN_Switcher.Properties.Settings.Default.PlayWhenOffline)
                        {
                            ConsoleOutputLogger.WriteLine("[TS] Spiele Gateway-Offline Meldung ab...");
                            serialportProcessor.SwitchOn();
                            beaconProcessor.Play(FFN_Switcher.Properties.Settings.Default.GatewayOfflineFile);
                            serialportProcessor.SwitchOff();
                        }
                    }
                    else
                    {
                        // erneut die Offline Meldung abspielen wenn das so konfiguriert wurde
                        if ((FFN_Switcher.Properties.Settings.Default.ReplayGatewayOffline) && (FFN_Switcher.Properties.Settings.Default.PlayWhenOffline))
                        {
                            // offenbar wurde es so konfiguriert
                            if (whenGatewaywasOffline == DateTime.MinValue)
                            {
                                whenGatewaywasOffline = DateTime.Now;
                            }
                            else
                            {
                                TimeSpan ts2 = DateTime.Now - whenGatewaywasOffline;

                                if (ts2.TotalSeconds >= (double)FFN_Switcher.Properties.Settings.Default.ReplayGatewayOfflineSeconds)
                                {
                                    whenGatewaywasOffline = DateTime.Now;
                                    ConsoleOutputLogger.WriteLine("[TS] Spiele Gateway-Offline Meldung erneut ab...");
                                    serialportProcessor.SwitchOn();
                                    beaconProcessor.Play(FFN_Switcher.Properties.Settings.Default.GatewayOfflineFile);
                                    serialportProcessor.SwitchOff();
                                }
                            }
                        }
                    }
                    Thread.Sleep(1000);
                }
                whenGatewaywasOffline = DateTime.MinValue;
                #endregion

                lock (PlayBeacons)
                {
                    if (PlayBeacons.Count > 0)
                    {
                        foreach (String playit in PlayBeacons)
                        {
                            ConsoleOutputLogger.WriteLine("[TS] Spiele: "+playit+" ab.");
                            try
                            {
                                if (FFN_Switcher.Properties.Settings.Default.MuteOutputWhenPlayingBeacon)
                                    TeamspeakFlags.OutputMuted = true;
                                
                                serialportProcessor.SwitchOn();
                                beaconProcessor.Play(playit);
                                serialportProcessor.SwitchOff();

                                if (FFN_Switcher.Properties.Settings.Default.MuteOutputWhenPlayingBeacon)
                                    TeamspeakFlags.OutputMuted = false;
                            }
                            catch (Exception e)
                            {
                                ConsoleOutputLogger.WriteLine("[EXCEPTION@TS] " + e.Message);
                                
                                try
                                {
                                    SoundPlayer player = new SoundPlayer();
                                    player.SoundLocation = playit;

                                    player.Play();

                                    serialportProcessor.SwitchOff();
                                    if (FFN_Switcher.Properties.Settings.Default.MuteOutputWhenPlayingBeacon)
                                        TeamspeakFlags.OutputMuted = false;
                                }
                                catch (Exception)
                                {
                                }
                            }
                        }

                        PlayBeacons.Clear();
                    }
                }


                if (ConnectedToServer)
                {
                    #region do stuff when connected to server

                    #region Check if we're in right channel
                    #region Get Info from Settings
                    if (ConnectedToServerNumber == 1)
                    {
                        CurrentShouldChannelID = Convert.ToInt32(FFN_Switcher.Properties.Settings.Default.TSServer1_ChannelID);
                        CurrentShouldChannelPW = FFN_Switcher.Properties.Settings.Default.TSServer1_ChannelPW;
                    }
                    else
                    {
                        CurrentShouldChannelID = Convert.ToInt32(FFN_Switcher.Properties.Settings.Default.TSServer2_ChannelID);
                        CurrentShouldChannelPW = FFN_Switcher.Properties.Settings.Default.TSServer2_ChannelPW;
                    }
                    #endregion

                    haveVoice = TeamspeakChannelFlags.HaveVoice();

                    #region Voice Flag Lie Detector
                    if (!haveVoice)
                    {
                        Int32 ChecksDone = 0;

                        while (FFN_Switcher.Properties.Settings.Default.NumberOfChecksIfTeamspeakIsLying > ChecksDone)
                        {
                            Thread.Sleep(100);
                            if (TeamspeakChannelFlags.HaveVoice())
                            {
                                ConsoleOutputLogger.WriteLine("[TS] Teamspeak hat uns angelogen - wir haben doch noch Voice.");
                                haveVoice = true;
                                break;
                            }
                            ChecksDone++;
                        }
                    }
                    #endregion

                    if (!haveVoice)
                    {
                        // wenn wir kein Voice haben...
                        // prüfen ob wir es vorhin schon nicht hatten
                        if (!voicegenommen)
                        {
                            // wir hatten es vorhin noch... also disabled sound abspielen

                            if (FFN_Switcher.Properties.Settings.Default.PlayWhenDisabled)
                            {
                                ConsoleOutputLogger.WriteLine("[TS] Gateway wurde deaktiviert. Spiele Deaktiviert-Sound ab.");
                                serialportProcessor.SwitchOn();
                                beaconProcessor.Play(FFN_Switcher.Properties.Settings.Default.GatewayDisabledFile);
                                serialportProcessor.SwitchOff();
                            }
                            voicegenommen = true;

                            if (FFN_Switcher.Properties.Settings.Default.MuteOutputWhenPlayingBeacon)
                                TeamspeakFlags.OutputMuted = true;
                        }
                        else
                        {
                            if ((FFN_Switcher.Properties.Settings.Default.ReplayGatewayDeactivated) && (FFN_Switcher.Properties.Settings.Default.PlayWhenDisabled))
                            {
                                // uns wurde Voice genommen, wir haben schon einmal das GatewayDisabledFile abgespielt, also
                                // spielen wir es eventuell nach einem festgelegten Zeitraum erneut ab
                                if (whenGatewaywasDeactivated == DateTime.MinValue)
                                {
                                    whenGatewaywasDeactivated = DateTime.Now;
                                }
                                else
                                {
                                    TimeSpan ts1 = DateTime.Now - whenGatewaywasDeactivated;

                                    if (ts1.TotalSeconds >= (double)FFN_Switcher.Properties.Settings.Default.ReplayGatewayDeactivatedSeconds)
                                    {
                                        whenGatewaywasDeactivated = DateTime.Now;
                                        ConsoleOutputLogger.WriteLine("[TS] Gateway wurde deaktiviert. Spiele erneut Deaktiviert-Sound ab.");
                                        serialportProcessor.SwitchOn();
                                        beaconProcessor.Play(FFN_Switcher.Properties.Settings.Default.GatewayDisabledFile);
                                        serialportProcessor.SwitchOff();
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        // wir haben voice... haben wir sie wiederbekommen?

                        if (voicegenommen)
                        {
                            voicegenommen = false;
                            whenGatewaywasDeactivated = DateTime.MinValue;
                            // Play Online Sound if allowed
                            if (FFN_Switcher.Properties.Settings.Default.PlayAtConnect)
                            {
                                serialportProcessor.SwitchOn();
                                beaconProcessor.Play(FFN_Switcher.Properties.Settings.Default.ConnectFile);
                                serialportProcessor.SwitchOff();
                            }

                            if (FFN_Switcher.Properties.Settings.Default.WhenMovedToOtherChannelOutputMute)
                                TeamspeakFlags.OutputMuted = false;

                        }
                    }

                    // check if we're in this channel...and have voice...
                    if (( (TSRemote.UserInfo.Channel.ChannelID == CurrentShouldChannelID) || (CurrentlyOnPenalty)))
                    {
                        #region Right Channel
                        // we're already in the channel we should be...

                        #region We're back in the right channel...
                        if ((TSRemote.UserInfo.Channel.ChannelID == CurrentShouldChannelID) && (CurrentlyOnPenalty))
                        {
                            ConsoleOutputLogger.WriteLine("[TS] Gateway ist wieder im richtigen Channel oder hat wieder Voice erhalten.");
                            
                            // Play Online Sound if allowed
                            if (FFN_Switcher.Properties.Settings.Default.PlayAtConnect)
                            {
                                serialportProcessor.SwitchOn();
                                beaconProcessor.Play(FFN_Switcher.Properties.Settings.Default.ConnectFile);
                                serialportProcessor.SwitchOff();
                            }
                            CurrentlyOnPenalty = false;
                            movedByOtherUser = true;
                            if (FFN_Switcher.Properties.Settings.Default.WhenMovedToOtherChannelOutputMute)
                                TeamspeakFlags.OutputMuted = false;
                        }
                        #endregion

                        #region check Whispered Flag
                        if (ConnectedToServerNumber == 2)
                        {
                            if (FFN_Switcher.Properties.Settings.Default.TSServer2_WhisperBlocked)
                            {
                                if (TeamspeakFlags.Whisper == false)
                                    TeamspeakFlags.Whisper = true;
                            }
                        }
                        else
                        {
                            if (FFN_Switcher.Properties.Settings.Default.TSServer1_WhisperBlocked)
                            {
                                if (TeamspeakFlags.Whisper == false)
                                    TeamspeakFlags.Whisper = true;
                            }
                        }
                        #endregion

                        #region check ChannelCommanderFlag
                        if (ConnectedToServerNumber == 2)
                        {
                            if (FFN_Switcher.Properties.Settings.Default.TSServer2_AlwaysChannelCommander)
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
                            if (FFN_Switcher.Properties.Settings.Default.TSServer1_AlwaysChannelCommander)
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

                                        if (FFN_Switcher.Properties.Settings.Default.FlagChannelCommanderWhenSending)
                                        {
                                            if (!TeamspeakFlags.ChannelCommander)
                                                TeamspeakFlags.ChannelCommander = true;
                                        }
                                    }

                                    if ((IamSpeakingStarted - DateTime.Now).TotalMinutes >= Convert.ToInt32(FFN_Switcher.Properties.Settings.Default.WatchdogMinutes))
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
                                            if (FFN_Switcher.Properties.Settings.Default.FlagChannelCommanderWhenSending)
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
                                    if (FFN_Switcher.Properties.Settings.Default.NotifyIfSpeakPauseFail)
                                    {
                                        TSRemote.SendText("[TS] ACHTUNG: Bitte Sprechpause einhalten !");
                                    }

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

                            if ((CurrentlySending) && ((DateTime.Now - StartedToSend).TotalSeconds >= Convert.ToInt32(FFN_Switcher.Properties.Settings.Default.DelaySeconds)))
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
                                        ConsoleOutputLogger.WriteLine("[TS] Niemand spricht mehr...warte " + Convert.ToInt32(FFN_Switcher.Properties.Settings.Default.DelayAfterSendingMilliSeconds)+ " Millisekunden auf Nachtasten...");
                                    }
                                    else
                                    {
                                        if ((DateTime.Now - NobodySpeaking).TotalMilliseconds >= Convert.ToInt32(FFN_Switcher.Properties.Settings.Default.DelayAfterSendingMilliSeconds)) 
                                        {
                                            ConsoleOutputLogger.WriteLine("[TS] Durchgang beendet - schalte Gateway auf Empfang - Durchgangsdauer: " + (DateTime.Now - StartedToSend).TotalSeconds + " Sekunden");
                                            CurrentlySending = false;
                                            NobodySpeakingRunning = false;
                                            if (FFN_Switcher.Properties.Settings.Default.PlayRogerBeep)
                                                beaconProcessor.Play(FFN_Switcher.Properties.Settings.Default.RogerBeepFile);
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
                        if (FFN_Switcher.Properties.Settings.Default.ReplayGatewayBeacon)
                        {
                            if (Convert.ToInt32(FFN_Switcher.Properties.Settings.Default.GatewayBeaconReplayInMinutes) > 1)
                            {
                                if ((DateTime.Now - LastBeacon).TotalMinutes >= Convert.ToInt32(FFN_Switcher.Properties.Settings.Default.GatewayBeaconReplayInMinutes))
                                {
                                    if (!CurrentlySending)
                                    {
                                        ConsoleOutputLogger.WriteLine("[TS] Zeit für Bake ist erreicht - Spiele Bake ab.");

                                        if (FFN_Switcher.Properties.Settings.Default.MuteOutputWhenPlayingBeacon)
                                            TeamspeakFlags.OutputMuted = true;

                                        serialportProcessor.SwitchOn();
                                        beaconProcessor.Play(FFN_Switcher.Properties.Settings.Default.GatewayBeaconFile);
                                        serialportProcessor.SwitchOff();

                                        if (FFN_Switcher.Properties.Settings.Default.MuteOutputWhenPlayingBeacon)
                                            TeamspeakFlags.OutputMuted = false;

                                        LastBeacon = DateTime.Now;
                                    }
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

                            while (checkker < Convert.ToInt32(FFN_Switcher.Properties.Settings.Default.NumberOfChecksIfTeamspeakIsLying))
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
                                if (FFN_Switcher.Properties.Settings.Default.PlayAtDisconnect)
                                {
                                    serialportProcessor.SwitchOn();
                                    beaconProcessor.Play(FFN_Switcher.Properties.Settings.Default.DisconnectFile);
                                    serialportProcessor.SwitchOff();
                                }

                                if (FFN_Switcher.Properties.Settings.Default.AutoReconnect)
                                {
                                    ConsoleOutputLogger.WriteLine("[TS] Versuche in " + FFN_Switcher.Properties.Settings.Default.AutoReconnectSeconds+ " Sekunden wieder zu verbinden...");
                                    Thread.Sleep(Convert.ToInt32(FFN_Switcher.Properties.Settings.Default.AutoReconnectSeconds) * 1000);
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
                            if (FFN_Switcher.Properties.Settings.Default.PlayWhenDisabled)
                            {
                                serialportProcessor.SwitchOn();
                                beaconProcessor.Play(FFN_Switcher.Properties.Settings.Default.GatewayDisabledFile);
                                serialportProcessor.SwitchOff();
                            }

                            // check if we're already connected or not...
                            ConsoleOutputLogger.WriteLine("[TS] Der Switcher hat kein Voice mehr im Channel oder wurde verschoben!");
                            ConsoleOutputLogger.WriteLine("[TS] Wechsle in " + FFN_Switcher.Properties.Settings.Default .WaitPenaltyMinutes+ " Minuten wieder zurück in den Channel.");

                            if (FFN_Switcher.Properties.Settings.Default.WhenMovedToOtherChannelOutputMute)
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
                                if ((DateTime.Now - PenaltyStarted).TotalMinutes >= Convert.ToInt32(FFN_Switcher.Properties.Settings.Default.WaitPenaltyMinutes))
                                {
                                    if (FFN_Switcher.Properties.Settings.Default.JumpBackToChannelAfterWaitPenaltyMinutes)
                                    {
                                        ConsoleOutputLogger.WriteLine("[TS] Wartezeit zuende.");
                                        movedByOtherUser = false;
                                        CurrentlyOnPenalty = false;
                                        if (FFN_Switcher.Properties.Settings.Default.WhenMovedToOtherChannelOutputMute)
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
                                    if (FFN_Switcher.Properties.Settings.Default.PlayAtConnect)
                                    {
                                        if (FFN_Switcher.Properties.Settings.Default.MuteOutputWhenPlayingBeacon)
                                            TeamspeakFlags.OutputMuted = true;
                                        
                                        serialportProcessor.SwitchOn();
                                        beaconProcessor.Play(FFN_Switcher.Properties.Settings.Default.ConnectFile);
                                        serialportProcessor.SwitchOff();

                                        if (FFN_Switcher.Properties.Settings.Default.MuteOutputWhenPlayingBeacon)
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

                    ConsoleOutputLogger.WriteLine("[TS] Verbinde mit " + FFN_Switcher.Properties.Settings.Default.TSServer1_ServerURL);
                    ConsoleOutputLogger.WriteLine("[TS] Warte " + FFN_Switcher.Properties.Settings.Default.WaitForConnectSeconds + " Sekunden auf Teamspeak Client Rückmeldung...");
                    // connect
                    TSRemote.Connect(GenerateConnectString(FFN_Switcher.Properties.Settings.Default.Nickname, FFN_Switcher.Properties.Settings.Default.TSServer1_ServerURL, FFN_Switcher.Properties.Settings.Default.Nickname, FFN_Switcher.Properties.Settings.Default.TSServer1_LoginName, FFN_Switcher.Properties.Settings.Default.TSServer1_Password));
                    ConnectedToServerNumber = 1;
                    // auf Login warten...
                    Int32 Counter = 0;
                    while (TSRemote.Channels.Length == 0)
                    {
                        ConsoleOutputLogger.WriteLine("Channels: " + TSRemote.Channels.Length);
                        if ((Counter >= FFN_Switcher.Properties.Settings.Default.WaitForConnectSeconds)||(done))
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

                        if (Convert.ToInt32(FFN_Switcher.Properties.Settings.Default.UseServer2AfterNumberOfFailedReconnects) == 0)
                            FFN_Switcher.Properties.Settings.Default.UseServer2AfterNumberOfFailedReconnects = 2;

                        if (NumberOfFailedReconnects == Convert.ToInt32(FFN_Switcher.Properties.Settings.Default.UseServer2AfterNumberOfFailedReconnects))
                        {
                            NumberOfFailedReconnects = 0;
                            ConnectedToServer = false;
                            // Try second server...
                            ConsoleOutputLogger.WriteLine("[TS] Konnte mit " + FFN_Switcher.Properties.Settings.Default.TSServer1_ServerURL+ " nicht verbinden.");
                            if (FFN_Switcher.Properties.Settings.Default .TSServer2_ServerURL!= "")
                            {
                                ConsoleOutputLogger.WriteLine("[TS] Verbinde mit alternativ Server " + FFN_Switcher.Properties.Settings.Default.TSServer2_ServerURL);
                                TSRemote.Connect(GenerateConnectString(FFN_Switcher.Properties.Settings.Default.Nickname, FFN_Switcher.Properties.Settings.Default.TSServer2_ServerURL, FFN_Switcher.Properties.Settings.Default.Nickname, FFN_Switcher.Properties.Settings.Default.TSServer2_LoginName, FFN_Switcher.Properties.Settings.Default.TSServer2_Password));
                                ConnectedToServerNumber = 2;
                            }
                        }
                    }

                    if (TSRemote.Channels.Length != 0)
                    {
                        ConnectedToServer = true;
                        TeamspeakFlags = new ClientFlags();
                        TeamspeakChannelFlags = new ChannelFlags();
                        
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
                        if (!done)
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
