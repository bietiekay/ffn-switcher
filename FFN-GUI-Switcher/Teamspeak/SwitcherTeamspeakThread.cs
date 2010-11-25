#region Usings
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
#endregion

namespace FFN_Switcher.Processors
{
    /// <summary>
    /// Hier findet die gesamte Arbeit mit Teamspeak 2 statt. Zum aktuellen Zeitpunkt ist hier noch keine
    /// Kapselung für andere Clients (Teamspeak 3, Mumble, ...) vorgesehen aber geplant.
    /// </summary>
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
        public Int32 lastknownChannelID = -1;
        bool ConnectedToServer = false;
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
        #region AddPlayBeacon
        public void AddPlayBeacon(String File)
        {
            lock (PlayBeacons)
            {
                PlayBeacons.Add(File);
            }
        }
        #endregion
        #region KillTeamspeak
        public void RestartTeamspeak()
        {
            #region Kill it
            if (TeamspeakHelperMethods.TeamspeakClientRunning())
            {
                IntPtr windowHandle = TeamspeakHelperMethods.FindWindowEx(IntPtr.Zero, IntPtr.Zero, "TMainForm", "TeamSpeak 2");
                TeamspeakHelperMethods.SendMessage(windowHandle, TeamspeakHelperMethods.WM_CLOSE, 0, new StringBuilder(""));
            }
            #endregion

            ConsoleOutputLogger.WriteLine("[TS] Warte auf Teamspeak Client...");

            Int32 waiting = 0;

            while ((waiting < 10) && (!done))
            {
                Thread.Sleep(Convert.ToInt32(FFN_Switcher.Properties.Settings.Default.WaitForTeamspeakClientTime));
                waiting++;
            }

            #region Start it
            ConsoleOutputLogger.WriteLine("[TS] Starte Teamspeak Client wieder...");
            TeamspeakHelperMethods.OpenTeamspeakClient(FFN_Switcher.Properties.Settings.Default.TeamspeakClientURL);
            #endregion
        }
        #endregion
        #endregion

        #region Thread
        /// <summary>
        /// Dieser Thread kümmert sich um das gesamte Teamspeak 2 Handling.
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
            Int32[] currentlySpeaking = null;
            bool movedByOtherUser = false;
            bool previouslyPaused = false;
            Int32 CurrentShouldChannelID;
            String CurrentShouldChannelPW;
            Int32 MyOwnID;
            Boolean CurrentlySending = false;
            Boolean goneChannelOnFirstConnect = false;
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
                    Thread.Sleep(Convert.ToInt32(FFN_Switcher.Properties.Settings.Default.WaitForPausedTeamspeakClient));
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
                
                while ( (!TeamspeakHelperMethods.TeamspeakClientRunning() && (!done)))
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
                    Thread.Sleep(Convert.ToInt32(FFN_Switcher.Properties.Settings.Default.WaitForTeamspeakClientTime));
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
                        if (FFN_Switcher.Properties.Settings.Default.MuteOutputWhenPlayingBeacon)
                            TeamspeakFlags.OutputMuted = false;
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
                            Thread.Sleep(Convert.ToInt32(FFN_Switcher.Properties.Settings.Default.WaitWhenTeamspeakIsLyingAboutVoice));
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

                            //if (FFN_Switcher.Properties.Settings.Default.WhenMovedToOtherChannelOutputMute)
                            //    TeamspeakFlags.OutputMuted = true;
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
                    lastknownChannelID = TSRemote.UserInfo.Channel.ChannelID;
                    if (((lastknownChannelID == CurrentShouldChannelID) || (CurrentlyOnPenalty)))
                    {
                        #region Right Channel
                        // we're already in the channel we should be...

                        #region We're back in the right channel...
                        if ((lastknownChannelID == CurrentShouldChannelID) && (CurrentlyOnPenalty))
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
                        List<String> newText = TeamspeakHelperMethods.GetLastLinesFromTeamSpeak();
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
                                        ConsoleOutputLogger.WriteLine("[TS] Gateway spricht zu lange...was soll ich tun?");
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

                                            // eventuell sollen wir das gateway um nachtasten zu verhindern nach einem durchgang kurz stumm schalten?
                                            if (FFN_Switcher.Properties.Settings.Default.MuteGatewayAfterTalking)
                                            {
                                                ConsoleOutputLogger.WriteLine("[TS] Gateway wird für " + FFN_Switcher.Properties.Settings.Default.MuteGatewayAfterTalkingTime + " Millisekunden stumm geschaltet");
                                                // Mute it!
                                                TeamspeakFlags.OutputMuted = true;
                                                Thread.Sleep(Convert.ToInt32(FFN_Switcher.Properties.Settings.Default.MuteGatewayAfterTalkingTime));
                                                TeamspeakFlags.OutputMuted = false;
                                            }

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
                                Thread.Sleep(Convert.ToInt32(FFN_Switcher.Properties.Settings.Default.WaitBeforeNextTSReconnectTry));
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

                        if (!goneChannelOnFirstConnect)
                        {
                            ConsoleOutputLogger.WriteLine("[TS] Erster Connect -> Wechsle in Channel " + CurrentShouldChannelID);
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
                                ConsoleOutputLogger.WriteLine("[TS] Teamspeak meldet dass wir in Channel mit ID " + CurrentShouldChannelID + " gewechselt haben.");
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
                            }
                            goneChannelOnFirstConnect = true;
                        }
                        #endregion

                    }
                    #endregion

                    #endregion
                }
                else
                {
                    #region Connect to Server
                    Thread.Sleep(Convert.ToInt32(FFN_Switcher.Properties.Settings.Default.WaitBeforeConnectToTS));
                    goneChannelOnFirstConnect = false;
                    ConsoleOutputLogger.WriteLine("[TS] Verbinde mit " + FFN_Switcher.Properties.Settings.Default.TSServer1_ServerURL);
                    ConsoleOutputLogger.WriteLine("[TS] Warte " + FFN_Switcher.Properties.Settings.Default.WaitForConnectSeconds + " Sekunden auf Teamspeak Client Rückmeldung...");
                    // connect
                    TSRemote.Connect(TeamspeakHelperMethods.GenerateConnectString(FFN_Switcher.Properties.Settings.Default.Nickname, FFN_Switcher.Properties.Settings.Default.TSServer1_ServerURL, FFN_Switcher.Properties.Settings.Default.Nickname, FFN_Switcher.Properties.Settings.Default.TSServer1_LoginName, FFN_Switcher.Properties.Settings.Default.TSServer1_Password));
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
                        Thread.Sleep(Convert.ToInt32(FFN_Switcher.Properties.Settings.Default.WaitForChannelList));
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
                                TSRemote.Connect(TeamspeakHelperMethods.GenerateConnectString(FFN_Switcher.Properties.Settings.Default.Nickname, FFN_Switcher.Properties.Settings.Default.TSServer2_ServerURL, FFN_Switcher.Properties.Settings.Default.Nickname, FFN_Switcher.Properties.Settings.Default.TSServer2_LoginName, FFN_Switcher.Properties.Settings.Default.TSServer2_Password));
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
                        ConsoleOutputLogger.WriteLine("[TS] Konnte mit keinem Teamspeak Server verbinden. Warte "+Convert.ToInt32(FFN_Switcher.Properties.Settings.Default.WaitForReconnectAfterTriedBefore)+" Millisekunden und versuche dann nochmal.");
                        if (!done)
                            Thread.Sleep(Convert.ToInt32(FFN_Switcher.Properties.Settings.Default.WaitForReconnectAfterTriedBefore));
                    }
                    #endregion
                }

                Thread.Sleep(Convert.ToInt32(FFN_Switcher.Properties.Settings.Default.TeamspeakUpdateTime));  // it seems to be important to wait in some cases...
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
