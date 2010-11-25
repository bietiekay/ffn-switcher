using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using FFN_Switcher.Logging;
using System.IO;
using System.Runtime.InteropServices;

namespace FFN_Switcher.Teamspeak
{
    public static class TeamspeakHelperMethods
    {
        #region GetText
        #region DLLImports
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

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

        public const int WM_GETTEXT = 0x000D;
        public const int WM_CLOSE = 0x0010;
        public const int WM_GETTEXTLENGTH = 0x000E;
        #endregion

        public static string GetText(IntPtr hwnd)
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
            IntPtr windowHandle = TeamspeakHelperMethods.FindWindowEx(IntPtr.Zero, IntPtr.Zero, "TMainForm", "TeamSpeak 2");

            if (windowHandle == IntPtr.Zero)
                return false;
            else
                return true;
        }
        #endregion

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

        #region GetLastLinesFromTeamspeak
        private static String LastLine = null;

        public static List<String> GetLastLinesFromTeamSpeak()
        {
            List<String> Output = new List<string>();
            String RawText = "";

            try
            {
                IntPtr windowHandle = TeamspeakHelperMethods.FindWindowEx(IntPtr.Zero, IntPtr.Zero, "TMainForm", "TeamSpeak 2");
                IntPtr childHandle;

                IntPtr PanelHandle = TeamspeakHelperMethods.FindWindowEx(windowHandle, IntPtr.Zero, "TPanel", IntPtr.Zero);
                IntPtr PanelHandle2 = TeamspeakHelperMethods.FindWindowEx(windowHandle, PanelHandle, "TPanel", IntPtr.Zero);
                childHandle = TeamspeakHelperMethods.FindWindowEx(PanelHandle2, IntPtr.Zero, "TRichEditWithLinks", IntPtr.Zero);
                if (childHandle != IntPtr.Zero)
                {
                    RawText = TeamspeakHelperMethods.GetText(childHandle);
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
        public static String GenerateConnectString(String GatewayName, String ServerURL, String Nickname, String Loginname, String Password)
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



    }
}
