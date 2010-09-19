using System;
using System.Collections.Generic;
using System.Text;
using FFN_Switcher.Logging;
using FFN_Switcher.Teamspeak;

namespace FFN_Switcher.Processors
{
    public class ChannelFlags
    {
        private Int32 Flags;

        #region Constructor
        public ChannelFlags()
        {
            //Flags = TSRemote.UserInfo.Channel.ChannelFlags;
        }
        #endregion

        #region HaveVoice
        public bool HaveVoice()
        {
            Flags = TSRemote.UserInfo.Player.PlayerChannelPrivileges;

            if ((Flags | (1 << 3)) == Flags)
                return true;
            else
                return false;
        }
        #endregion
    }
}