using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using FFN_Switcher.Logging;

namespace FFN_Switcher.TeamspeakRemote
{
    class HandleMessageCommands
    {
        #region User32 DLL Imports
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, IntPtr windowTitle);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hwnd, int wMsg,
            IntPtr wParam,
            IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SendMessage")]
        private static extern int SendMessage(IntPtr hwnd, int wMsg,
            int wParam,
            StringBuilder lParam);

        private const int WM_GETTEXT = 0x000D;
        private const int WM_GETTEXTLENGTH = 0x000E;
        #endregion

        #region GetText from Handle method
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

        String LastLine = null;

        #region GetTextSinceLastPoll
        /// <summary>
        /// This method gets the changed text since the last poll from the TeamSpeak 2 Client textwindow
        /// </summary>
        /// <returns>a list of strings containing a line of the textwindow of the TeamSpeak 2 Client or null if any Window could not be found or a different error occurred</returns>
        public List<String> GetTextSinceLastPoll()
        {
            List<String> Output = new List<string>();

            #region Find Window than Panel, then the next Panel, then find TRichEditWithLinks and get the text there
            String RawText = "";
            try
            {
                IntPtr windowHandle = FindWindowEx(IntPtr.Zero, IntPtr.Zero, "TMainForm", "TeamSpeak 2");

                if (windowHandle != IntPtr.Zero)
                {
                    IntPtr PanelHandle = FindWindowEx(windowHandle, IntPtr.Zero, "TPanel", IntPtr.Zero);
                    if (PanelHandle != IntPtr.Zero)
                    {
                        IntPtr PanelHandle2 = FindWindowEx(windowHandle, PanelHandle, "TPanel", IntPtr.Zero);
                        if (PanelHandle2 != IntPtr.Zero)
                        {
                            IntPtr childHandle = FindWindowEx(PanelHandle2, IntPtr.Zero, "TRichEditWithLinks", IntPtr.Zero);
                            if (childHandle != IntPtr.Zero)
                            {
                                RawText = GetText(childHandle);
                            }
                            else
                            return null;
                        }
                        else
                        return null;
                    }
                    else
                    return null;
                }
                else
                return null;
            }
            catch (Exception e)
            {
                ConsoleOutputLogger.WriteLine("[EXCEPTION@HandleMessageCommands.GetTExtSinceLastPoll] "+e.Message);
                return null;
            }
            #endregion

            #region Extract each line from the Teamspeak 2 Textwindow contents
            String[] splitter = new String[1];
            String[] RawTextLines = null;

            splitter[0] = "\r\n";

            if (RawText != "")
            {
                RawTextLines = RawText.Split(splitter,StringSplitOptions.RemoveEmptyEntries);
            }
            #endregion

            #region Find the starting point where we stopped last time
            Int32 StartingPointIndex = 0;

            if (LastLine != null)
            {
                if (RawTextLines != null)
                {
                    foreach (String line in RawTextLines)
                    {
                        if (line == LastLine)
                            break;
                        else
                            StartingPointIndex++;
                    }
                }
            }
            #endregion

            #region Extract the new lines...
            if (StartingPointIndex == RawTextLines.Length - 1)
                return Output;

            for (int i = StartingPointIndex; i < RawTextLines.Length; i++)
            {
                Output.Add(RawTextLines[i]);
            }
            #endregion

            #region SetLastLine
            if (Output.Count > 0)
            {
                LastLine = Output[Output.Count - 1];
            }
            #endregion

            return Output;
        }
        #endregion

    }
}
