using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Media;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading;

namespace TSUserAlarm
{
    public class TSUserAlarmHelpers
    {
        #region OpenTeamspeakClient
        public static int OpenTeamspeakClient(String TeamspeakClientURL)
        {
            Process proc = null;
            Process[] procs = null;

            try
            {
                if (File.Exists(TeamspeakClientURL))
                {
                    //ConsoleOutputLogger.WriteLine("[TS] Starting Teamspeak...");
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
        public static bool TeamspeakClientRunning()
        {
            IntPtr windowHandle = FindWindowEx(IntPtr.Zero, IntPtr.Zero, "TMainForm", "TeamSpeak 2");

            if (windowHandle == IntPtr.Zero)
                return false;
            else
                return true;
        }
        #endregion

        #region GetLastLinesFromTeamspeak
        private static String LastLine = null;

        public static List<String> GetLastLinesFromTeamSpeak()
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
                ConsoleOutputLogger.WriteLine("[FEHLER@TS] " + e.Message);
            }

            return Output;// strUrlToReturn;
        }
        #endregion

    }

    class TSUserAlarm
    {
        static void Main(string[] args)
        {
            ConsoleOutputLogger.WriteLine("Teamspeak User Alarm - Leberkäs-Semmel-Edition XP Professional 2009");
            ConsoleOutputLogger.WriteLine("");
            ConsoleOutputLogger.WriteLine("Des is a klaans programm für unser Lieblings-Dickerchen Peter - Schlumpfi1.");
            ConsoleOutputLogger.WriteLine("");

            bool showedTSAlert = false;

            HashSet<String> UserAlertPlayed = new HashSet<string>();

            List<String> LastRunUsers = new List<string>();

            while (TSUserAlarmHelpers.TeamspeakClientRunning())
            {
                #region Teamspeak läuft net
                while (!TSUserAlarmHelpers.TeamspeakClientRunning())
                {
                    if (!showedTSAlert)
                        ConsoleOutputLogger.WriteLine("Teamspeak läuft nicht! Haste vergessen zu starten, wah Dickerchen?");
                    showedTSAlert = true;

                    Thread.Sleep(100);
                }
                showedTSAlert = false;
                #endregion

                foreach (TSRemote.TtsrPlayerInfo Player in TSRemote.Players)
                {
                    foreach (String User in Properties.Settings.Default.Users)
                    {
                        if (Player.NickName.ToUpper().Contains(User.ToUpper()))
                        {
                            // found one we should play an alert on..
                            if (UserAlertPlayed.Contains(User))
                            {
                                // do nothing...
                                LastRunUsers.Add(User);
                            }
                            else
                            {
                                // play an alert plz!
                                UserAlertPlayed.Add(User);
                                LastRunUsers.Add(User);

                                Console.WriteLine("Allaaarm! - User " + User + " ist online!");

                                SoundPlayer player = new SoundPlayer();
                                player.SoundLocation = Properties.Settings.Default.Sound;
                                player.Load();
                                player.PlaySync();

                            }
                        }
                    }
                }

                // check if in the last run any users we played an alert for went offline
                UserAlertPlayed.Clear();
                foreach (String User in LastRunUsers)
                {
                    UserAlertPlayed.Add(User);
                }
                LastRunUsers.Clear();

                Thread.Sleep(1000);
            }
        }
    }
}
