using System;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace FFN_Switcher.Logging
{
    /// <summary>
    /// This Class stores a number of Console Output Lines into a ring buffer
    /// </summary>
    public static class ConsoleOutputLogger
    {
        private static int Max_Number_Of_Entries = 500;
        private static LinkedList<String> LoggerList = new LinkedList<String>();
        public static bool verbose = false;

        public static MainWindow _Window;

        public static String Logfilename = "ffn-switcher.log";
        
        private static bool _writeToLogfile;

        public static bool writeLogfile
        {
            get { return _writeToLogfile; }
            set 
            { 
                if (_writeToLogfile) 
                {
                    ShutdownLog();
                }
                
                _writeToLogfile = value;
            }
        }

        public static StreamWriter Logfile = null;

        public static void SetNumberOfMaxEntries(int Number)
        {
            // TODO: It would be nice to keep at least the Number of Lines we're setting
            lock(LoggerList)
            {
                LoggerList.Clear();
            }
            Max_Number_Of_Entries = Number;
        }

        public static void ShutdownLog()
        {
            if (_writeToLogfile)
            {
                Logfile.Flush();
                Logfile.Close();
            }
        }


        public static int GetMaxNumberOfEntries()
        {
            return Max_Number_Of_Entries;
        }

        public static void LogToFile(String text)
        {
            try
            {
                if (Logfile == null)
                {
                    Logfile = new StreamWriter(Logfilename, true);
                }
                lock (Logfile)
                {
                    Logfile.WriteLine(text);
                }
            }
            catch (Exception)
            {
                writeLogfile = false;
                WriteLine("[FEHLER@LOGGING] Kann Logdatei nicht schreiben. Logging ist temporär deaktiviert.");
            }
        }

        public static void WriteLine(String text)
        {
            DateTime TimeDate = DateTime.Now;

            text = TimeDate.ToShortDateString() + " - " + TimeDate.ToLongTimeString() + " " + text;
            
            // write it to the console
            if (verbose)
            {
                //System.Diagnostics.Debug.WriteLine(text);
                _Window.AddStatusText(text);
            }
            if (writeLogfile) LogToFile(text);

            lock(LoggerList)
            {
                if (LoggerList.Count == Max_Number_Of_Entries)
                {
                    LoggerList.RemoveFirst();
                }
                
                LoggerList.AddLast(text);
            }
        }

        public static String[] GetLoggedLines()
        {
            String[] Output = new String[Max_Number_Of_Entries];
            int Current_Position = 0;

            lock (LoggerList)
            {
                foreach (String line in LoggerList)
                {
                    Output[Current_Position] = line;
                    Current_Position++;
                }
            }
            return Output;
        }
    }
}
