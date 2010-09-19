using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeamSpeakTest
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
                    Flags = Flags | (1 << 2);
                else
                {
                    if (Whisper)
                        Flags = Flags ^ (1 << 2);
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
                    Flags = Flags | (1 << 3);
                else
                {
                    if (Away)
                        Flags = Flags ^ (1 << 3);
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
                    Flags = Flags | (1 << 4);
                else
                {
                    if (InputMuted)
                        Flags = Flags ^ (1 << 4);
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
                    Flags = Flags | (1 << 5);
                else
                {
                    if (OutputMuted)
                        Flags = Flags ^ (1 << 5);
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
                    Flags = Flags | (1 << 0);
                else
                {
                    if (ChannelCommander)
                        Flags = Flags ^ (1 << 0);
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

    class Program
    {
        static void Main(string[] args)
        {
            TeamSpeakTest.TSRemote tr = new TSRemote();

            Console.WriteLine(TeamSpeakTest.TSRemote.ServerInfo.ChannelCount);

            foreach (TSRemote.TtsrChannelInfo channel in TSRemote.Channels)
            {
                Console.WriteLine(channel.Name+" ("+channel.ChannelID+")");
            }

            ClientFlags cl = new ClientFlags();

            cl.InputMuted = true;

            while (true)
            {
                cl.Away = true;
                Console.ReadLine();
                cl.Away = false;
                Console.ReadLine();
                cl.InputMuted = false;
            }
        }
    }
}
