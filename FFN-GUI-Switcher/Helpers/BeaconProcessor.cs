using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using FFN_Switcher.Logging;
using System.Media;

namespace FFN_Switcher.Processors
{
    public class BeaconProcessor
    {
        #region Data
        public bool done;
        public bool restart;
        public bool shutdowndone;
        //public Settings.Settings internalSettings;
        public List<String> PlaySounds;
        private SoundPlayer player;
        private bool currentlyPlaying;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public BeaconProcessor()
        {
            PlaySounds = new List<string>();
            //internalSettings = Settings;
            done = false;
            restart = false;
            currentlyPlaying = false;
            shutdowndone = false;
        }
        #endregion

        #region Play
        public void Play(String Soundfile)
        {
            Int32 MaxWait = Convert.ToInt32(FFN_Switcher.Properties.Settings.Default.MaximumBeaconPlayTimeSeconds) * 1000;
            Int32 CurrentWait = 0;

            // wait 
            AddToPlayList(Soundfile);
            
            if (!currentlyPlaying)
            {
                while (!currentlyPlaying)
                {
                    Thread.Sleep(100);
                    CurrentWait = CurrentWait + 100;

                    if (CurrentWait > MaxWait)
                    {
                        ConsoleOutputLogger.WriteLine("[BEACON] Wir warten schon zu lange - offenbar gibt es Probleme mit der Soundkarte oder der Baken-Sounddatei");
                        ConsoleOutputLogger.WriteLine("[BEACON] Breche warten ab...");
                        lock (PlaySounds)
                        {
                            PlaySounds.Clear();
                        }
                        restart = true;
                        return;
                    }
                }
            }

            CurrentWait = 0;
            while (currentlyPlaying)
            {
                Thread.Sleep(100);
                CurrentWait = CurrentWait + 100;

                if (CurrentWait > MaxWait)
                {
                    ConsoleOutputLogger.WriteLine("[BEACON] Wir warten schon zu lange - offenbar gibt es Probleme mit der Soundkarte oder der Baken-Sounddatei");
                    ConsoleOutputLogger.WriteLine("[BEACON] Breche warten ab...");
                    lock (PlaySounds)
                    {
                        PlaySounds.Clear();
                    }
                    restart = true;
                    return;
                }
            }
        }
        #endregion

        #region AddToPlayList
        public void AddToPlayList(String Soundfile)
        {
            lock (PlaySounds)
            {
                PlaySounds.Add(Soundfile);
            }
        }
        #endregion

        #region Thread
        /// <summary>
        /// BeaconProcessor Thread
        /// </summary>
        public void BeaconProcessorThread()
        {
            ConsoleOutputLogger.WriteLine("[BEACON] Gestartet.");
            List<String> localCopyPlaylist = new List<string>();
            while (!done)
            {
                if (restart)
                {
                    ConsoleOutputLogger.WriteLine("[BEACON] Starte Beaconprozessor neu...");
                    localCopyPlaylist = new List<string>();
                    restart = false;
                    currentlyPlaying = false;
                }
                    try
                    {
                        lock (PlaySounds)
                        {
                            localCopyPlaylist.Clear();
                            foreach (String Soundfile in PlaySounds)
                            {
                                localCopyPlaylist.Add(Soundfile);
                            }
                            PlaySounds.Clear();
                        }
                        foreach (String SoundFile in localCopyPlaylist)
                        {
                            if (File.Exists(SoundFile))
                            {
                                ConsoleOutputLogger.WriteLine("[BEACON] Spiele " + SoundFile + " ab.");
                                currentlyPlaying = true;
                                try
                                {
                                    player = new SoundPlayer();
                                    player.SoundLocation = SoundFile;
                                    ConsoleOutputLogger.WriteLine("[BEACON] Lade Soundfile...");
                                    player.Load();
                                    ConsoleOutputLogger.WriteLine("[BEACON] Soundfile geladen...spiele ab.");
                                    player.PlaySync();
                                }
                                catch (Exception e)
                                {
                                    ConsoleOutputLogger.WriteLine("[EXCEPTION@BEACON] "+e.Message);
                                }
                                ConsoleOutputLogger.WriteLine("[BEACON] Abspielen beendet.");
                                currentlyPlaying = false;
                                player.Stop();
                            }
                            else
                            {
                                ConsoleOutputLogger.WriteLine("[BEACON] Kann " + SoundFile + " nicht abspielen - Datei nicht gefunden!");
                                currentlyPlaying = false;
                            }
                        }
                        localCopyPlaylist.Clear();
                        currentlyPlaying = false;
                    }
                    catch (Exception e)
                    {
                        ConsoleOutputLogger.WriteLine("[BEACON] Fehler beim Abspielen einer Sounddatei!");
                        ConsoleOutputLogger.WriteLine("[FEHLER@BEACON] " + e.Message);
                        currentlyPlaying = false;
                    }
                    Thread.Sleep(100);
            }
            ConsoleOutputLogger.WriteLine("[BEACON] Beendet.");
            shutdowndone = true;
        }
        #endregion
    }
}
