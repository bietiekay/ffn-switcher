using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace FFN_Switcher.SettingsOld
{
    #region Teamspeak Server settings (at least one per Gateway)
    [Serializable]
    public class TeamspeakServer
    {
        public String ServerURL;
        public String LoginName;
        public String Password;

        public Int32 ChannelID;
        public String ChannelPW;

        public bool WhisperBlocked;
        public bool AlwaysChannelCommander;

        public TeamspeakServer()
        {
            ServerURL = "voice.ts-ffn.de";
            LoginName = "";
            Password = "";
            ChannelID = 11;
            ChannelPW = "";
            WhisperBlocked = true;

        }
    }
    #endregion

    #region MessageRemoteControl
    [Serializable]
    public class MessageRC
    {
    }
    #endregion

    #region Gateway
    [Serializable]
    public class Gateway
    {
    	public String GatewayName;
        public String Nickname;
        public bool isActive;
        public Int32 WaitForConnectSeconds;
        //public List<TeamspeakServer> ServerList;

        public TeamspeakServer TSServer1;
        public TeamspeakServer TSServer2;

        public Int32 UseServer2AfterNumberOfFailedReconnects;

        public String Comport;
        public bool ComportEnabled;
        public bool RTS; // if false then DTR
        public Int32 DelaySeconds;
        public Int32 DelayAfterSendingMilliSeconds;
        public Int32 WatchdogMinutes;

        public bool AutoReconnect;
        public Int32 AutoReconnectSeconds;
        public Int32 WaitPenaltyMinutes;
        public bool FlagChannelCommanderWhenSending;
        public bool WhenMovedToOtherChannelOutputMute;
        public Int32 NumberOfChecksIfTeamspeakIsLying;

        public bool JumpBackToChannelAfterWaitPenaltyMinutes;

        public Gateway()
        {
            TSServer1 = new TeamspeakServer();
            TSServer2 = new TeamspeakServer();
            WaitForConnectSeconds = 60;
            UseServer2AfterNumberOfFailedReconnects = 2;
            NumberOfChecksIfTeamspeakIsLying = 20;
            ComportEnabled = true;
        }
    }
    #endregion

    #region Logging
    [Serializable]
    public class Logging
    {
        public bool LoggingToFileEnabled;
        public bool LoggingToScreenEnabled;
        public string Logfile;
    }
    #endregion

    #region Beacons
    [Serializable]
    public class Beacons
    {
        public string BeaconDirectory;
        public string GatewayBeaconFile;
        public bool MuteOutputWhenPlayingBeacon;
        public bool ReplayGatewayBeacon;
        public bool ReplayGatewayBeaconOnlyWhenActivity;
        public Int32 GatewayBeaconReplayInMinutes;

        public Int32 MaximumBeaconPlayTimeSeconds;

        public string ConnectFile;
        public bool PlayAtConnect;
        public string DisconnectFile;
        public bool PlayAtDisconnect;
        public string GatewayDisabledFile;
        public bool PlayWhenDisabled;
        public string RogerBeepFile;
        public bool PlayRogerBeep;

        public string GatewayOfflineFile;
        public bool PlayWhenOffline;

        public bool ReplayWhenDisabled;
        public Int32 ReplayTimeInMinutes;

        public Beacons()
        {
            MaximumBeaconPlayTimeSeconds = 120;
        }
    }
    #endregion

    #region TeamspeakClient
    [Serializable]
    public class TeamspeakClient
    {
        public String TeamSpeakClientURL;
        public bool StartTeamspeakClientAtStartup;
    }
    #endregion

    #region internal HTTP Server
    [Serializable]
    public class HTTPServer
    {
        public Int32 TCPPort;
        public String ListeningIP;
        public String DocumentRoot;
        public bool verboseLogging;
    }
    #endregion

    [Serializable]
    public class Settings
    {
        public Gateway Gateway;
        public Logging Logging;
        public bool SetupWizardDone = false;
        public HTTPServer HTTP;
        public Beacons Beacon;
        public MessageRC MessageRemoteControl;
        public TeamspeakClient TSClient;

        public Settings()
        {
            Gateway = new Gateway();
            Logging = new Logging();
            HTTP = new HTTPServer();
            Beacon = new Beacons();
            MessageRemoteControl = new MessageRC();
            TSClient = new TeamspeakClient();

            #region Defaults
            Gateway.WaitPenaltyMinutes = 20;
            HTTP.TCPPort = 80;
            HTTP.ListeningIP = "127.0.0.1";
            HTTP.DocumentRoot = "./www";
            HTTP.verboseLogging = false;
            Logging.Logfile = "ffn-switcher.log";
            Logging.LoggingToFileEnabled = false;
            Logging.LoggingToScreenEnabled = true;

            Beacon.MuteOutputWhenPlayingBeacon = true;
            Beacon.MaximumBeaconPlayTimeSeconds = 120;

            Gateway.isActive = true;
            Gateway.NumberOfChecksIfTeamspeakIsLying = 20;
            Gateway.UseServer2AfterNumberOfFailedReconnects = 2;
            Gateway.JumpBackToChannelAfterWaitPenaltyMinutes = false;

            Gateway.AutoReconnect = true;
            Gateway.AutoReconnectSeconds = 30;
            Gateway.Comport = "COM1";
            //Gateway.DelayAfterSendingSeconds = 2;
            Gateway.DelayAfterSendingMilliSeconds = 1500;
            Gateway.TSServer1.AlwaysChannelCommander = false;
            Gateway.ComportEnabled = true;
            Gateway.TSServer2.AlwaysChannelCommander = false;

            Beacon.BeaconDirectory = "./beacon/";
            Beacon.ConnectFile = Beacon.BeaconDirectory + "connect.wav";
            Beacon.DisconnectFile = Beacon.BeaconDirectory + "disconnect.wav";
            Beacon.GatewayBeaconFile = Beacon.BeaconDirectory + "gateway.wav";
            Beacon.GatewayDisabledFile = Beacon.BeaconDirectory + "gateway_disabled.wav";
            Beacon.GatewayOfflineFile = Beacon.BeaconDirectory + "gateway_offline.wav";
            Beacon.RogerBeepFile = Beacon.BeaconDirectory + "rogerbeep.wav";

            Gateway.FlagChannelCommanderWhenSending = true;
            Gateway.WhenMovedToOtherChannelOutputMute = false;
            Gateway.Nickname = "Standard-Nickname";

            TSClient.StartTeamspeakClientAtStartup = true;
            TSClient.TeamSpeakClientURL = "C:/Programme/Teamspeak2_RC2/TeamSpeak.exe";
            #endregion
        }

        #region GetSettingsValue
        public String GetSettingsValue(String SettingsName)
        {
            object fieldvalue = null;
            Type reflectiontype;

            bool done = false;
            // what to start with...
            reflectiontype = typeof(Settings);


            while (!done)
            {
                #region Settings.
                // reflect the given object...
                foreach (FieldInfo field in reflectiontype.GetFields())
                {
                    if (fieldvalue != null)
                        break;

                    // only do non generic+public types.
                    if ((field.IsPublic) && (!field.FieldType.IsGenericType) && (field.FieldType.FullName.Contains("System.")))
                    {
                        if (("Settings." + field.Name) == SettingsName)
                        {
                            fieldvalue = field.GetValue(this);
                            done = true;
                            break;
                        }
                    }
                    else
                    {
                        Type reflectiontype2 = field.FieldType;
                        String fieldname = "Settings." + field.Name+".";
                        foreach (FieldInfo field2 in reflectiontype2.GetFields())
                        {
                            if (fieldvalue != null)
                                break;
                            if ((field2.IsPublic) && (!field2.FieldType.IsGenericType) && (field2.FieldType.FullName.Contains("System.")))
                            {
                                if ((fieldname + field2.Name) == SettingsName)
                                {
                                    fieldvalue = field2.GetValue(field.GetValue(this));
                                    done = true;
                                    break;
                                }
                            }
                            else
                            {
                                Type reflectiontype3 = field.FieldType;
                                String fieldname2 = fieldname + field2.Name + ".";
                                foreach (FieldInfo field3 in reflectiontype3.GetFields())
                                {
                                    if (fieldvalue != null)
                                        break;
                                    if ((field3.IsPublic) && (!field3.FieldType.IsGenericType) && (field3.FieldType.FullName.Contains("System.")))
                                    {
                                        if ((fieldname2 + field3.Name) == SettingsName)
                                        {
                                            fieldvalue = field3.GetValue(field2.GetValue(field.GetValue(this)));
                                            done = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion
            }

            if (fieldvalue is bool)
                return ((bool)fieldvalue).ToString();

            return fieldvalue.ToString();
        }
        #endregion

        #region SetSettingsValue
        public void SetSettingsValue(String SettingsName, object Value)
        {
            object fieldvalue = null;
            Type reflectiontype;

            bool done = false;
            // what to start with...
            reflectiontype = typeof(Settings);


            while (!done)
            {
                #region Settings.
                // reflect the given object...
                foreach (FieldInfo field in reflectiontype.GetFields())
                {
                    if (done)
                        break;

                    // only do non generic+public types.
                    if ((field.IsPublic) && (!field.FieldType.IsGenericType) && (field.FieldType.FullName.Contains("System.")))
                    {
                        if (("Settings." + field.Name) == SettingsName)
                        {
                            field.SetValue(this, Value);
                            done = true;
                            break;
                        }
                    }
                    else
                    {
                        Type reflectiontype2 = field.FieldType;
                        String fieldname = "Settings." + field.Name + ".";
                        foreach (FieldInfo field2 in reflectiontype2.GetFields())
                        {
                            if (fieldvalue != null)
                                break;
                            if ((field2.IsPublic) && (!field2.FieldType.IsGenericType) && (field2.FieldType.FullName.Contains("System.")))
                            {
                                if ((fieldname + field2.Name) == SettingsName)
                                {
                                    field2.SetValue(field.GetValue(this),Value);
                                    done = true;
                                    break;
                                }
                            }
                            else
                            {
                                Type reflectiontype3 = field.FieldType;
                                String fieldname2 = fieldname + field2.Name + ".";
                                foreach (FieldInfo field3 in reflectiontype3.GetFields())
                                {
                                    if (fieldvalue != null)
                                        break;
                                    if ((field3.IsPublic) && (!field3.FieldType.IsGenericType) && (field3.FieldType.FullName.Contains("System.")))
                                    {
                                        if ((fieldname2 + field3.Name) == SettingsName)
                                        {
                                            field3.SetValue(field2.GetValue(field.GetValue(this)),Value);
                                            done = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion
            }

        }
        #endregion

        #region GetSettingsNameListings
        public List<String> GetSettingsNameListings()
        {
            List<String> Output = new List<string>();

            // TODO: this is just 3 layered... make it a while clause to make it indefinite (almost)

            #region Settings.
            Type reflectiontype = typeof(Settings);
            // reflect the given object...
            foreach (FieldInfo field in reflectiontype.GetFields())
            {
                // only do non generic+public types.
                if ((field.IsPublic) && (!field.FieldType.IsGenericType) && (field.FieldType.FullName.Contains("System.")))
                {
                    //Output.Add(field.FieldType.FullName.Replace(field.FieldType.Namespace, ""));
                    Output.Add("Settings." + field.Name);
                }
                else
                {
                    #region layer 2
                    foreach (FieldInfo field2 in field.FieldType.GetFields())
                    {
                        if ((field2.IsPublic) && (!field2.FieldType.IsGenericType) && (field2.FieldType.FullName.Contains("System.")))
                        {
                            //Output.Add(field.FieldType.FullName.Replace(field.FieldType.Namespace, ""));
                            Output.Add("Settings." + field.Name + "." + field2.Name);
                        }
                        else
                        {
                            #region layer 3
                            foreach (FieldInfo field3 in field2.FieldType.GetFields())
                            {
                                if ((field3.IsPublic) && (!field3.FieldType.IsGenericType) && (field3.FieldType.FullName.Contains("System.")))
                                {
                                    //Output.Add(field.FieldType.FullName.Replace(field.FieldType.Namespace, ""));
                                    Output.Add("Settings." + field.Name +"."+ field2.Name +"."+ field3.Name);
                                }
                            }
                            #endregion
                        }
                    }
                    #endregion
                }
            }
            #endregion

            return Output;
        }
        #endregion
    }
}
