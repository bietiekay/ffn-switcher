#ifndef IPC_CLIENT_COMMANDS_H
#define IPC_CLIENT_COMMANDS_H

#ifdef __cplusplus
extern "C" {
#endif


#ifdef DLLEXPORTFUNCTIONS
  #if defined(WIN32) || defined(__WIN32__) || defined(_WIN64)
    #define DLLFUNCTION __declspec(dllexport)
  #else
    #define DLLFUNCTION __attribute__ ((visibility("default")))
  #endif
#else
  #if defined(WIN32) || defined(__WIN32__) || defined(_WIN64)
    #define DLLFUNCTION __declspec(dllimport)
  #else
    #define DLLFUNCTION __attribute__ ((visibility("default")))
  #endif
#endif

#include "plugin_definitions.h"

#define IPC_ERROR_NO_SERVER 0x000a
#define IPC_ERROR_WRITE 0x000b
#define IPC_ERROR_READ 0x000c
#define IPC_ERROR_SEMAPHORE 0x000d

#if defined(linux) || defined(__linux__)
  #include <sys/types.h>
#endif

#include "ipc_events.h"

#if defined(WIN32) || defined(__WIN32__) || defined(_WIN64)
DLLFUNCTION unsigned int initSDK(char key[5]);
#else
DLLFUNCTION unsigned int initSDK(key_t key);
#endif

DLLFUNCTION unsigned int shutdownSDK();

DLLFUNCTION unsigned int registerCallbacks(ts3_ipc_event_callbacks funcs);

DLLFUNCTION unsigned int callOnPluginInitiation(PLUGININITFUNC func);

DLLFUNCTION unsigned int getServerConnectionInfo(uint64 schid);
DLLFUNCTION unsigned int getChannelConnectionInfo(uint64 schid, uint64 channelID);
DLLFUNCTION unsigned int getClientLibVersion(char** result);
DLLFUNCTION unsigned int spawnNewServerConnectionHandler(int port, uint64* result);
DLLFUNCTION unsigned int destroyServerConnectionHandler(uint64 schid);
DLLFUNCTION unsigned int getErrorMessage(unsigned int errorCode, char** error);

/* Memory management */
DLLFUNCTION unsigned int freeMemory(void* pointer);

/* Logging */
DLLFUNCTION unsigned int logMessage(const char* logMessage, enum LogLevel severity, const char* channel, uint64 logID);

/* Sound */
DLLFUNCTION unsigned int getPlaybackDeviceList(int modeID, char**** result);
DLLFUNCTION unsigned int getPlaybackModeList(char**** result);
DLLFUNCTION unsigned int getCaptureDeviceList(int modeID, char**** result);
DLLFUNCTION unsigned int getCaptureModeList(char**** result);
DLLFUNCTION unsigned int getDefaultPlaybackDevice(int modeID, char*** result);
DLLFUNCTION unsigned int getDefaultPlayBackMode(int* result);
DLLFUNCTION unsigned int getDefaultCaptureDevice(int modeID, char*** result);
DLLFUNCTION unsigned int getDefaultCaptureMode(int* result);
DLLFUNCTION unsigned int openPlaybackDevice(uint64 schid, int modeID, const char* playbackDevice);
DLLFUNCTION unsigned int openCaptureDevice(uint64 schid, int modeID, const char* captureDevice);
//unsigned int openCustomPlaybackDevice(uint64 schid, void* fmodSystem);
//unsigned int openCustomCaptureDevice(uint64 schid, void* fmodSystem, int fmodDriverID);
DLLFUNCTION unsigned int getCurrentPlaybackDeviceName(uint64 schid, char** result);
//unsigned int getCurrentPlaybackDevice(uint64 schid, void** fmodSystemResult);
DLLFUNCTION unsigned int getCurrentPlayBackMode(uint64 schid, int* result);
DLLFUNCTION unsigned int getCurrentCaptureDeviceName(uint64 schid, char** result);
//unsigned int getCurrentCaptureDevice(uint64 schid, void** fmodSystemResult);
DLLFUNCTION unsigned int getCurrentCaptureMode(uint64 schid, int* result);
DLLFUNCTION unsigned int initiateGracefulPlaybackShutdown(uint64 schid);
DLLFUNCTION unsigned int closePlaybackDevice(uint64 schid);
DLLFUNCTION unsigned int closeCaptureDevice(uint64 schid);
DLLFUNCTION unsigned int activateCaptureDevice(uint64 schid);
//unsigned int activateCustomCaptureDevice(uint64 schid, void* fmodSystem, int fmodDriverID);
DLLFUNCTION unsigned int playWaveFile(uint64 schid, const char* path);

/* Preprocessor */
DLLFUNCTION unsigned int getPreProcessorInfoValueFloat(uint64 schid, const char* ident, float* result);
DLLFUNCTION unsigned int getPreProcessorConfigValue(uint64 schid, const char* ident, char** result);
DLLFUNCTION unsigned int setPreProcessorConfigValue(uint64 schid, const char* ident, const char* value);

/* Encoder */
DLLFUNCTION unsigned int getEncodeConfigValue(uint64 schid, const char* ident, char** result);

/* Playback */
DLLFUNCTION unsigned int getPlaybackConfigValueAsFloat(uint64 schid, const char* ident, float* result);
DLLFUNCTION unsigned int setPlaybackConfigValue(uint64 schid, const char* ident, const char* value);

/* Recording */
DLLFUNCTION unsigned int startVoiceRecording();
DLLFUNCTION unsigned int stopVoiceRecording();

/* 3d sound positioning */
DLLFUNCTION unsigned int fmod_Systemset3DListenerAttributes(uint64 schid, const TS3_FMOD_VECTOR* position, const TS3_FMOD_VECTOR* velocity, const TS3_FMOD_VECTOR* forward, const TS3_FMOD_VECTOR* up);
DLLFUNCTION unsigned int fmod_Systemset3DSettings(uint64 schid, float dopplerScale, float distanceFactor, float rolloffScale);
DLLFUNCTION unsigned int fmod_Channelset3DAttributes(uint64 schid, anyID clientID, const TS3_FMOD_VECTOR* position, const TS3_FMOD_VECTOR* velocity);

/* Interaction with the server */
DLLFUNCTION unsigned int startConnection(uint64 schid, const char* identity, const char* ip, unsigned int port, const char* nickname,
                                const char** defaultChannelArray, const char* defaultChannelPassword, const char* serverPassword);
DLLFUNCTION unsigned int stopConnection(uint64 schid, const char* quitMessage);
DLLFUNCTION unsigned int requestClientMove(uint64 schid, anyID clientID, uint64 newChannelID, const char* password, const char* returnCode);
DLLFUNCTION unsigned int requestClientVariables(uint64 schid, anyID clientID, const char* returnCode);
DLLFUNCTION unsigned int requestClientKickFromChannel(uint64 schid, anyID clientID, const char* kickReason, const char* returnCode);
DLLFUNCTION unsigned int requestClientKickFromServer(uint64 schid, anyID clientID, const char* kickReason, const char* returnCode);
DLLFUNCTION unsigned int requestChannelDelete(uint64 schid, uint64 channelID, int force, const char* returnCode);
DLLFUNCTION unsigned int requestChannelMove(uint64 schid, uint64 channelID, uint64 newChannelParentID, uint64 newChannelOrder, const char* returnCode);
DLLFUNCTION unsigned int requestSendPrivateTextMsg(uint64 schid, const char* message, anyID targetClientID, const char* returnCode);
DLLFUNCTION unsigned int requestSendChannelTextMsg(uint64 schid, const char* message, uint64 targetChannelID, const char* returnCode);
DLLFUNCTION unsigned int requestSendServerTextMsg(uint64 schid, const char* message, const char* returnCode);
DLLFUNCTION unsigned int requestConnectionInfo(uint64 schid, anyID clientID, const char* returnCode);
DLLFUNCTION unsigned int requestClientSetWhisperList(uint64 schid, anyID clientID, const uint64* targetChannelIDArray, const anyID* targetClientIDArray, const char* returnCode);
DLLFUNCTION unsigned int requestChannelSubscribe(uint64 schid, uint64 channelID, const char* returnCode);
DLLFUNCTION unsigned int requestChannelSubscribeAll(uint64 schid, const char* returnCode);
DLLFUNCTION unsigned int requestChannelUnsubscribe(uint64 schid, uint64 channelID, const char* returnCode);
DLLFUNCTION unsigned int requestChannelUnsubscribeAll(uint64 schid, const char* returnCode);
DLLFUNCTION unsigned int requestChannelDescription(uint64 schid, uint64 channelID, const char* returnCode);
DLLFUNCTION unsigned int requestMuteClients(uint64 schid, const anyID* clientIDArray, const char* returnCode);
DLLFUNCTION unsigned int requestUnmuteClients(uint64 schid, const anyID* clientIDArray, const char* returnCode);
DLLFUNCTION unsigned int requestClientPoke(uint64 schid, anyID clientID, const char* message, const char* returnCode);
DLLFUNCTION unsigned int clientChatClosed(uint64 schid, const char* clientUniqueIdentifier, anyID clientID, const char* returnCode);
DLLFUNCTION unsigned int clientChatComposing(uint64 schid, anyID clientID, const char* returnCode);

/* Access clientlib information */

/* Query own client ID */
DLLFUNCTION unsigned int getClientID(uint64 schid, anyID* result);

/* Client info */
DLLFUNCTION unsigned int getClientSelfVariableAsInt(uint64 schid, size_t flag, int* result);
DLLFUNCTION unsigned int getClientSelfVariableAsString(uint64 schid, size_t flag, char** result);
DLLFUNCTION unsigned int setClientSelfVariableAsInt(uint64 schid, size_t flag, int value);
DLLFUNCTION unsigned int setClientSelfVariableAsString(uint64 schid, size_t flag, const char* value);
DLLFUNCTION unsigned int flushClientSelfUpdates(uint64 schid);
DLLFUNCTION unsigned int getClientVariableAsInt(uint64 schid, anyID clientID, size_t flag, int* result);
DLLFUNCTION unsigned int getClientVariableAsUInt64(uint64 schid, anyID clientID, size_t flag, uint64* result);
DLLFUNCTION unsigned int getClientVariableAsString(uint64 schid, anyID clientID, size_t flag, char** result);
DLLFUNCTION unsigned int getClientList(uint64 schid, anyID** result);
DLLFUNCTION unsigned int getChannelOfClient(uint64 schid, anyID clientID, uint64* result);

/* Channel info */
DLLFUNCTION unsigned int getChannelVariableAsInt(uint64 schid, uint64 channelID, size_t flag, int* result);
DLLFUNCTION unsigned int getChannelVariableAsString(uint64 schid, uint64 channelID, size_t flag, char** result);
DLLFUNCTION unsigned int getChannelIDFromChannelNames(uint64 schid, char** channelNameArray, uint64* result);
DLLFUNCTION unsigned int getChannelVariableAsUInt64(uint64 schid, uint64 channelID, size_t flag, uint64* result);
DLLFUNCTION unsigned int setChannelVariableAsInt(uint64 schid, uint64 channelID, size_t flag, int value);
DLLFUNCTION unsigned int setChannelVariableAsString(uint64 schid, uint64 channelID, size_t flag, const char* value);
DLLFUNCTION unsigned int setChannelVariableAsUInt64(uint64 schid, uint64 channelID, size_t flag, uint64 value);
DLLFUNCTION unsigned int flushChannelUpdates(uint64 schid, uint64 channelID);
DLLFUNCTION unsigned int flushChannelCreation(uint64 schid, uint64 channelParentID);
DLLFUNCTION unsigned int getChannelList(uint64 schid, uint64** result);
DLLFUNCTION unsigned int getChannelClientList(uint64 schid, uint64 channelID,  anyID** result);
DLLFUNCTION unsigned int getParentChannelOfChannel(uint64 schid, uint64 channelID, uint64* result);

/* Server info */
DLLFUNCTION unsigned int getServerConnectionHandlerList(uint64** result);
DLLFUNCTION unsigned int getServerVariableAsInt(uint64 schid, size_t flag, int* result);
DLLFUNCTION unsigned int getServerVariableAsUInt64(uint64 schid, size_t flag, uint64* result);
DLLFUNCTION unsigned int getServerVariableAsString(uint64 schid, size_t flag, char** result);
DLLFUNCTION unsigned int requestServerVariables(uint64 schid);

/* Connection info */
DLLFUNCTION unsigned int getConnectionStatus(uint64 schid, int* result);
DLLFUNCTION unsigned int getConnectionVariableAsUInt64(uint64 schid, anyID clientID, size_t flag, uint64* result);
DLLFUNCTION unsigned int getConnectionVariableAsDouble(uint64 schid, anyID clientID, size_t flag, double* result);
DLLFUNCTION unsigned int getConnectionVariableAsString(uint64 schid, anyID clientID, size_t flag, char** result);
DLLFUNCTION unsigned int cleanUpConnectionInfo(uint64 schid, anyID clientID);

/*client related*/
DLLFUNCTION unsigned int requestClientDBIDfromUID(uint64 schid, const char* clientUniqueIdentifier, const char* returnCode);
DLLFUNCTION unsigned int requestClientNamefromUID(uint64 schid, const char* clientUniqueIdentifier, const char* returnCode);
DLLFUNCTION unsigned int requestClientNamefromDBID(uint64 schid, uint64 clientDatabaseID, const char* returnCode);
DLLFUNCTION unsigned int requestClientEditDescription(uint64 schid, anyID clientID, const char* clientDescription, const char* returnCode);
DLLFUNCTION unsigned int requestClientSetIsTalker(uint64 schid, anyID clientID, int isTalker, const char* returnCode);
DLLFUNCTION unsigned int requestIsTalker(uint64 schid, int isTalkerRequest, const char* isTalkerRequestMessage, const char* returnCode);

/* Filetransfer */
DLLFUNCTION unsigned int getTransferFileName(anyID transferID, char** result);
DLLFUNCTION unsigned int getTransferFilePath(anyID transferID, char** result);
DLLFUNCTION unsigned int getTransferFileSize(anyID transferID, uint64* result);
DLLFUNCTION unsigned int getTransferFileSizeDone(anyID transferID, uint64* result);
DLLFUNCTION unsigned int isTransferSender(anyID transferID, int* result);  /* 1 == upload, 0 == download */
DLLFUNCTION unsigned int getTransferStatus(anyID transferID, int* result);
DLLFUNCTION unsigned int getCurrentTransferSpeed(anyID transferID, float* result);
DLLFUNCTION unsigned int getAverageTransferSpeed(anyID transferID, float* result);
DLLFUNCTION unsigned int getTransferRunTime(anyID transferID, uint64* result);
DLLFUNCTION unsigned int sendFile(uint64 schid, uint64 channelID, const char* channelPW, const char* file, int overwrite, int resume, const char* sourceDirectory, anyID* result, const char* returnCode);
DLLFUNCTION unsigned int requestFile(uint64 schid, uint64 channelID, const char* channelPW, const char* file, int overwrite, int resume, const char* destinationDirectory, anyID* result, const char* returnCode);
DLLFUNCTION unsigned int haltTransfer(uint64 schid, anyID transferID, int deleteUnfinishedFile, const char* returnCode);
DLLFUNCTION unsigned int requestFileList(uint64 schid, uint64 channelID, const char* channelPW, const char* path, const char* returnCode);
DLLFUNCTION unsigned int requestFileInfo(uint64 schid, uint64 channelID, const char* channelPW, const char* file, const char* returnCode);
DLLFUNCTION unsigned int requestDeleteFile(uint64 schid, uint64 channelID, const char* channelPW, const char** file, const char* returnCode);
DLLFUNCTION unsigned int requestCreateDirectory(uint64 schid, uint64 channelID, const char* channelPW, const char* directoryPath, const char* returnCode);
DLLFUNCTION unsigned int requestRenameFile(uint64 schid, uint64 fromChannelID, const char* channelPW, uint64 toChannelID, const char* toChannelPW, const char* oldFile, const char* newFile, const char* returnCode);

/* Offline message management */
DLLFUNCTION unsigned int requestMessageAdd(uint64 schid, const char* toClientUID, const char* subject, const char* message, const char* returnCode);
DLLFUNCTION unsigned int requestMessageDel(uint64 schid, uint64 messageID, const char* returnCode);
DLLFUNCTION unsigned int requestMessageGet(uint64 schid, uint64 messageID, const char* returnCode);
DLLFUNCTION unsigned int requestMessageList(uint64 schid, const char* returnCode);
DLLFUNCTION unsigned int requestMessageUpdateFlag(uint64 schid, uint64 messageID, int flag, const char* returnCode);

/* Interacting with the server - banning */
DLLFUNCTION unsigned int banclient(uint64 schid, anyID clientID, uint64 timeInSeconds, const char* banReason, const char* returnCode);
DLLFUNCTION unsigned int banadd(uint64 schid, const char* ipRegExp, const char* nameRegexp, const char* uniqueIdentity, uint64 timeInSeconds, const char* banReason, const char* returnCode);
DLLFUNCTION unsigned int bandel(uint64 schid, uint64 banID, const char* returnCode);
DLLFUNCTION unsigned int bandelall(uint64 schid, const char* returnCode);
DLLFUNCTION unsigned int requestBanList(uint64 schid, const char* returnCode);

/* Interacting with the server - complain */
DLLFUNCTION unsigned int requestComplainAdd(uint64 schid, uint64 targetClientDatabaseID, const char* complainReason, const char* returnCode);
DLLFUNCTION unsigned int requestComplainDel(uint64 schid, uint64 targetClientDatabaseID, uint64 fromClientDatabaseID, const char* returnCode);
DLLFUNCTION unsigned int requestComplainDelAll(uint64 schid, uint64 targetClientDatabaseID, const char* returnCode);
DLLFUNCTION unsigned int requestComplainList(uint64 schid, uint64 targetClientDatabaseID, const char* returnCode);

/* Permissions */
DLLFUNCTION unsigned int requestServerGroupList(uint64 schid, const char* returnCode);
DLLFUNCTION unsigned int requestServerGroupAdd(uint64 schid, const char* groupName, int groupType, const char* returnCode);
DLLFUNCTION unsigned int requestServerGroupDel(uint64 schid, uint64 serverGroupID, int force, const char* returnCode);
DLLFUNCTION unsigned int requestServerGroupAddClient(uint64 schid, uint64 serverGroupID, uint64 clientDatabaseID, const char* returnCode);
DLLFUNCTION unsigned int requestServerGroupDelClient(uint64 schid, uint64 serverGroupID, uint64 clientDatabaseID, const char* returnCode);
DLLFUNCTION unsigned int requestServerGroupsByClientID(uint64 schid, uint64 clientDatabaseID, const char* returnCode);
DLLFUNCTION unsigned int requestServerGroupAddPerm(uint64 schid, uint64 serverGroupID, const anyID* permissionIDArray, const int* permissionValueArray, const int* permissionNegatedArray, const int* permissionSkipArray, int arraySize, const char* returnCode);
DLLFUNCTION unsigned int requestServerGroupDelPerm(uint64 schid, uint64 serverGroupID, const anyID* permissionIDArray, int arraySize, const char* returnCode);
DLLFUNCTION unsigned int requestServerGroupPermList(uint64 schid, uint64 serverGroupID, const char* returnCode);
DLLFUNCTION unsigned int requestServerGroupClientList(uint64 schid, uint64 serverGroupID, int withNames, const char* returnCode);
DLLFUNCTION unsigned int requestChannelGroupList(uint64 schid, const char* returnCode);
DLLFUNCTION unsigned int requestChannelGroupAdd(uint64 schid, const char* groupName, int groupType, const char* returnCode);
DLLFUNCTION unsigned int requestChannelGroupDel(uint64 schid, uint64 channelGroupID, int force, const char* returnCode);
DLLFUNCTION unsigned int requestChannelGroupAddPerm(uint64 schid, uint64 channelGroupID, const anyID* permissionIDArray, const int* permissionValueArray, int arraySize, const char* returnCode);
DLLFUNCTION unsigned int requestChannelGroupDelPerm(uint64 schid, uint64 channelGroupID, const anyID* permissionIDArray, int arraySize, const char* returnCode);
DLLFUNCTION unsigned int requestChannelGroupPermList(uint64 schid, uint64 channelGroupID, const char* returnCode);
DLLFUNCTION unsigned int requestSetClientChannelGroup(uint64 schid, const uint64* channelGroupIDArray, const uint64* channelIDArray, const uint64* clientDatabaseIDArray, int arraySize, const char* returnCode);
DLLFUNCTION unsigned int requestChannelAddPerm(uint64 schid, uint64 channelID, const anyID* permissionIDArray, const int* permissionValueArray, int arraySize, const char* returnCode);
DLLFUNCTION unsigned int requestChannelDelPerm(uint64 schid, uint64 channelID, const anyID* permissionIDArray, int arraySize, const char* returnCode);
DLLFUNCTION unsigned int requestChannelPermList(uint64 schid, uint64 channelID, const char* returnCode);
DLLFUNCTION unsigned int requestClientAddPerm(uint64 schid, uint64 clientDatabaseID, const anyID* permissionIDArray, const int* permissionValueArray, const int* permissionSkipArray, int arraySize, const char* returnCode);
DLLFUNCTION unsigned int requestClientDelPerm(uint64 schid, uint64 clientDatabaseID, const anyID* permissionIDArray, int arraySize, const char* returnCode);
DLLFUNCTION unsigned int requestClientPermList(uint64 schid, uint64 clientDatabaseID, const char* returnCode);
DLLFUNCTION unsigned int requestChannelClientAddPerm(uint64 schid, uint64 channelID, uint64 clientDatabaseID, const anyID* permissionIDArray, const int* permissionValueArray, int arraySize, const char* returnCode);
DLLFUNCTION unsigned int requestChannelClientDelPerm(uint64 schid, uint64 channelID, uint64 clientDatabaseID, const anyID* permissionIDArray, int arraySize, const char* returnCode);
DLLFUNCTION unsigned int requestChannelClientPermList(uint64 schid, uint64 channelID, uint64 clientDatabaseID, const char* returnCode);
DLLFUNCTION unsigned int privilegeKeyUse(uint64 schid, const char* tokenKey, const char* returnCode);
DLLFUNCTION unsigned int requestPermissionList(uint64 schid, const char* returnCode);
DLLFUNCTION unsigned int requestPermissionOverview(uint64 schid, uint64 clientDBID, uint64 channelID, const char* returnCode);

/* Client functions */
DLLFUNCTION int getAPIVersion(unsigned int* err);
DLLFUNCTION unsigned int getAppPath(char** path, size_t maxLen);
DLLFUNCTION unsigned int getResourcesPath(char** path, size_t maxLen);
DLLFUNCTION unsigned int getConfigPath(char** path, size_t maxLen);
DLLFUNCTION unsigned int getPluginPath(char** path, size_t maxLen);
DLLFUNCTION uint64       getCurrentServerConnectionHandlerID(unsigned int* err);
DLLFUNCTION unsigned int printMessage(uint64 schid, const char* message, enum PluginMessageTarget messageTarget);
DLLFUNCTION unsigned int printMessageToCurrentTab(const char* message);
DLLFUNCTION unsigned int urlsToBB(const char* text, char* result, size_t maxLen);
DLLFUNCTION unsigned int sendPluginCommand(uint64 schid, const char* commandID, const char* command, int targetMode, const anyID* targetIDs);
DLLFUNCTION unsigned int getServerConnectInfo(uint64 schid, char** host, unsigned short* port, char** password, size_t maxLen);
DLLFUNCTION unsigned int getChannelConnectInfo(uint64 schid, uint64 channelID, char** path, char** password, size_t maxLen);

#ifdef __cplusplus
}
#endif


#endif // IPC_CLIENT_COMMANDS_H
