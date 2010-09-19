using System;
using System.Collections.Generic;
using System.Text;
using FFN_Switcher.Logging;
using FFN_Switcher.Teamspeak;

namespace FFN_Switcher.Processors
{
    public class ClientFlags
    {
        private Int32 Flags;

        #region Constructor
        public ClientFlags()
        {
            Flags = TSRemote.UserInfo.Player.PlayerFlags;
        }
        #endregion

        #region Whisper
        public bool Whisper
        {
            get
            {
                if ((Flags | (1 << 2)) == Flags)
                    return true;
                else
                    return false;
            }
            set
            {
                if (value == true)
                {
                    ConsoleOutputLogger.WriteLine("[TS] NO WHISPER Flag eingeschaltet.");
                    Flags = Flags | (1 << 2);
                }
                else
                {
                    if (Whisper)
                    {
                        ConsoleOutputLogger.WriteLine("[TS] NO WHISPER Flag ausgeschaltet.");
                        Flags = Flags ^ (1 << 2);
                    }
                }

                TSRemote.SetPlayerFlags(Flags);
            }
        }
        #endregion

        #region Away
        public bool Away
        {
            get
            {
                if ((Flags | (1 << 3)) == Flags)
                    return true;
                else
                    return false;
            }
            set
            {
                if (value == true)
                {
                    ConsoleOutputLogger.WriteLine("[TS] Benutzerstatus auf ABWESEND gestellt");
                    Flags = Flags | (1 << 3);
                }
                else
                {
                    if (Away)
                    {
                        ConsoleOutputLogger.WriteLine("[TS] Benutzerstatus auf ANWESEND gestellt");
                        Flags = Flags ^ (1 << 3);
                    }
                }

                TSRemote.SetPlayerFlags(Flags);
            }
        }
        #endregion

        #region InputMuted
        public bool InputMuted
        {
            get
            {
                if ((Flags | (1 << 4)) == Flags)
                    return true;
                else
                    return false;
            }
            set
            {
                if (value == true)
                {
                    ConsoleOutputLogger.WriteLine("[TS] Sound Eingang von Teamspeak stummgeschaltet");
                    Flags = Flags | (1 << 4);
                }
                else
                {
                    if (InputMuted)
                    {
                        ConsoleOutputLogger.WriteLine("[TS] Sound Eingang von Teamspeak eingeschaltet");
                        Flags = Flags ^ (1 << 4);
                    }
                }

                TSRemote.SetPlayerFlags(Flags);
            }
        }
        #endregion

        #region OutputMuted
        public bool OutputMuted
        {
            get
            {
                if ((Flags | (1 << 5)) == Flags)
                    return true;
                else
                    return false;
            }
            set
            {
                if (value == true)
                {
                    ConsoleOutputLogger.WriteLine("[TS] Sound Ausgabe von Teamspeak stummgeschaltet");
                    Flags = Flags | (1 << 5);
                }
                else
                {
                    if (OutputMuted)
                    {
                        ConsoleOutputLogger.WriteLine("[TS] Sound Ausgabe von Teamspeak eingeschaltet");
                        Flags = Flags ^ (1 << 5);
                    }
                }

                TSRemote.SetPlayerFlags(Flags);
            }
        }
        #endregion

        #region ChannelCommander
        public bool ChannelCommander
        {
            get
            {
                if ((Flags | (1 << 0)) == Flags)
                    return true;
                else
                    return false;
            }
            set
            {
                if (value == true)
                {
                    ConsoleOutputLogger.WriteLine("[TS] CHANNELCOMMANDER Status eingeschaltet.");
                    Flags = Flags | (1 << 0);
                }
                else
                {
                    if (ChannelCommander)
                    {
                        ConsoleOutputLogger.WriteLine("[TS] CHANNELCOMMANDER Status ausgeschaltet.");
                        Flags = Flags ^ (1 << 0);
                    }
                }

                TSRemote.SetPlayerFlags(Flags);
            }
        }
        #endregion

        #region ServerAndSwitcherFlagsIdentical
        public bool ServerAndSwitcherFlagsIdentical()
        {
            if (TSRemote.UserInfo.Player.PlayerFlags == Flags)
                return true;
            else
                return false;
        }
        #endregion

        #region SetFlags
        public void SetFlags()
        {
            TSRemote.SetPlayerFlags(Flags);
        }
        #endregion
    }
}
