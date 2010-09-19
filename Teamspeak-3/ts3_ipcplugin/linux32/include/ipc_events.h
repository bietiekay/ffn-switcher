#ifndef IPC_EVENTS_H_INCLUDED
#define IPC_EVENTS_H_INCLUDED

#include "public_definitions.h"

#ifdef __cplusplus
extern "C" {
#endif

typedef void (*PLUGININITFUNC)(void);

struct Ets3_ipc_event_callbacks {
  void* object;
  void (*onIPCPluginShutdown)(void* object);

  void (*processCommand)(uint64 schid, const char* command, void* object);
  void (*currentServerConnectionChanged)(uint64 schid, void* object);
  void (*pluginEvent)(unsigned short data, const char* message, void* object);
  void (*onConnectStatusChangeEvent)(uint64 schid, int newStatus, unsigned int errorNumber, void* object);
  void (*onNewChannelEvent)(uint64 schid, uint64 channelID, uint64 channelParentID, void* object);
  void (*onNewChannelCreatedEvent)(uint64 schid, uint64 channelID, uint64 channelParentID, anyID invokerID, const char* invokerName, const char* invokerUniqueIdentifier, void* object);
  void (*onDelChannelEvent)(uint64 schid, uint64 channelID, anyID invokerID, const char* invokerName, const char* invokerUniqueIdentifier, void* object);
  void (*onChannelMoveEvent)(uint64 schid, uint64 channelID, uint64 newChannelParentID, anyID invokerID, const char* invokerName, const char* invokerUniqueIdentifier, void* object);
  void (*onUpdateChannelEvent)(uint64 schid, uint64 channelID, void* object);
  void (*onUpdateChannelEditedEvent)(uint64 schid, uint64 channelID, anyID invokerID, const char* invokerName, const char* invokerUniqueIdentifier, void* object);
  void (*onUpdateClientEvent)(uint64 schid, anyID clientID, void* object);
  void (*onClientMoveEvent)(uint64 schid, anyID clientID, uint64 oldChannelID, uint64 newChannelID, int visibility, const char* moveMessage, void* object);
  void (*onClientMoveSubscriptionEvent)(uint64 schid, anyID clientID, uint64 oldChannelID, uint64 newChannelID, int visibility, void* object);
  void (*onClientMoveTimeoutEvent)(uint64 schid, anyID clientID, uint64 oldChannelID, uint64 newChannelID, int visibility, const char* timeoutMessage, void* object);
  void (*onClientMoveMovedEvent)(uint64 schid, anyID clientID, uint64 oldChannelID, uint64 newChannelID, int visibility, anyID moverID, const char* moverName, const char* moverUniqueIdentifier, const char* moveMessage, void* object);
  void (*onClientKickFromChannelEvent)(uint64 schid, anyID clientID, uint64 oldChannelID, uint64 newChannelID, int visibility, anyID kickerID, const char* kickerName, const char* kickerUniqueIdentifier, const char* kickMessage, void* object);
  void (*onClientKickFromServerEvent)(uint64 schid, anyID clientID, uint64 oldChannelID, uint64 newChannelID, int visibility, anyID kickerID, const char* kickerName, const char* kickerUniqueIdentifier, const char* kickMessage, void* object);
  void (*onServerEditedEvent)(uint64 schid, anyID editerID, const char* editerName, const char* editerUniqueIdentifier, void* object);
  void (*onServerUpdatedEvent)(uint64 schid, void* object);
  void (*onServerErrorEvent)(uint64 schid, const char* errorMessage, unsigned int error, const char* returnCode, const char* extraMessage, void* object);
  void (*onServerStopEvent)(uint64 schid, const char* shutdownMessage, void* object);
  void (*onTextMessageEvent)(uint64 schid, anyID targetMode, anyID toID, anyID fromID, const char* fromName, const char* fromUniqueIdentifier, const char* message, int ffIgnored, void* object);
  void (*onTalkStatusChangeEvent)(uint64 schid, int status, int isReceivedWhisper, anyID clientID, void* object);
  void (*onConnectionInfoEvent)(uint64 schid, anyID clientID, void* object);
  void (*onServerConnectionInfoEvent)(uint64 schid, void* object);
  void (*onChannelSubscribeEvent)(uint64 schid, uint64 channelID, void* object);
  void (*onChannelSubscribeFinishedEvent)(uint64 schid, void* object);
  void (*onChannelUnsubscribeEvent)(uint64 schid, uint64 channelID, void* object);
  void (*onChannelUnsubscribeFinishedEvent)(uint64 schid, void* object);
  void (*onChannelDescriptionUpdateEvent)(uint64 schid, uint64 channelID, void* object);
  void (*onChannelPasswordChangedEvent)(uint64 schid, uint64 channelID, void* object);
  void (*onPlaybackShutdownCompleteEvent)(uint64 schid, void* object);
  void (*onUserLoggingMessageEvent)(const char* logMessage, int logLevel, const char* logChannel, uint64 logID, const char* logTime, const char* completeLogString, void* object);
  void (*onVoiceRecordDataEvent)(const float* data, unsigned int dataSize, void* object);
  void (*onClientBanFromServerEvent)(uint64 schid, anyID clientID, uint64 oldChannelID, uint64 newChannelID, int visibility, anyID kickerID, const char* kickerName, const char* kickerUniqueIdentifier, uint64 time, const char* kickMessage, void* object);
  void (*onClientPokeEvent)(uint64 schid, anyID fromClientID, const char* pokerName, const char* pokerUniqueIdentity, const char* message, int ffIgnored, void* object);
  void (*onClientSelfVariableUpdateEvent)(uint64 schid, int flag, const char* oldValue, const char* newValue, void* object);
  void (*onFileListEvent)(uint64 schid, uint64 channelID, const char* path, const char* name, uint64 size, uint64 datetime, int type, void* object);
  void (*onFileListFinishedEvent)(uint64 schid, uint64 channelID, const char* path, void* object);
  void (*onFileInfoEvent)(uint64 schid, uint64 channelID, const char* name, uint64 size, uint64 datetime, void* object);
  void (*onServerGroupListEvent)(uint64 schid, uint64 serverGroupID, const char* name, int type, int iconID, int saveDB, void* object);
  void (*onServerGroupListFinishedEvent)(uint64 schid, void* object);
  void (*onServerGroupByClientIDEvent)(uint64 schid, const char* name, uint64 serverGroupList, uint64 clientDatabaseID, void* object);
  void (*onServerGroupPermListEvent)(uint64 schid, uint64 serverGroupID, anyID permissionID, int permissionValue, int permissionNegated, int permissionSkip, void* object);
  void (*onServerGroupPermListFinishedEvent)(uint64 schid, uint64 serverGroupID, void* object);
  void (*onServerGroupClientListEvent)(uint64 schid, uint64 serverGroupID, uint64 clientDatabaseID, const char* clientNameIdentifier, const char* clientUniqueID, void* object);
  void (*onChannelGroupListEvent)(uint64 schid, uint64 channelGroupID, const char* name, int type, int iconID, int saveDB, void* object);
  void (*onChannelGroupListFinishedEvent)(uint64 schid, void* object);
  void (*onChannelGroupPermListEvent)(uint64 schid, uint64 channelGroupID, anyID permissionID, int permissionValue, int permissionNegated, int permissionSkip, void* object);
  void (*onChannelGroupPermListFinishedEvent)(uint64 schid, uint64 channelGroupID, void* object);
  void (*onChannelPermListEvent)(uint64 schid, uint64 channelID, anyID permissionID, int permissionValue, int permissionNegated, int permissionSkip, void* object);
  void (*onChannelPermListFinishedEvent)(uint64 schid, uint64 channelID, void* object);
  void (*onClientPermListEvent)(uint64 schid, uint64 clientDatabaseID, anyID permissionID, int permissionValue, int permissionNegated, int permissionSkip, void* object);
  void (*onClientPermListFinishedEvent)(uint64 schid, uint64 clientDatabaseID, void* object);
  void (*onChannelClientPermListEvent)(uint64 schid, uint64 channelID, uint64 clientDatabaseID, anyID permissionID, int permissionValue, int permissionNegated, int permissionSkip, void* object);
  void (*onChannelClientPermListFinishedEvent)(uint64 schid, uint64 channelID, uint64 clientDatabaseID, void* object);
  void (*onClientChannelGroupChangedEvent)(uint64 schid, uint64 channelGroupID, uint64 channelID, anyID clientID, anyID invokerClientID, const char* invokerName, const char* invokerUniqueIdentity, void* object);
  void (*onServerPermissionErrorEvent)(uint64 schid, const char* errorMessage, unsigned int error, const char* returnCode, anyID failedPermissionID, void* object);
  void (*onPermissionListEvent)(uint64 schid, anyID permissionID, const char* permissionName, const char* permissionDescription, void* object);
  void (*onPermissionListFinishedEvent)(uint64 schid, void* object);
  void (*onPermissionOverviewEvent)(uint64 schid, uint64 clientDatabaseID, uint64 channelID, int overviewType, uint64 overviewID1, uint64 overviewID2, anyID permissionID, int permissionValue, int permissionNegated, int permissionSkip, void* object);
  void (*onPermissionOverviewFinishedEvent)(uint64 schid, void* object);
  void (*onServerGroupClientAddedEvent)(uint64 schid, anyID clientID, const char* clientName, const char* clientUniqueIdentity, uint64 serverGroupID, anyID invokerClientID, const char* invokerName, const char* invokerUniqueIdentity, void* object);
  void (*onServerGroupClientDeletedEvent)(uint64 schid, anyID clientID, const char* clientName, const char* clientUniqueIdentity, uint64 serverGroupID, anyID invokerClientID, const char* invokerName, const char* invokerUniqueIdentity, void* object);
  void (*onClientNeededPermissionsEvent)(uint64 schid, anyID permissionID, int permissionValue, void* object);
  void (*onClientNeededPermissionsFinishedEvent)(uint64 schid, void* object);
  void (*onFileTransferStatusEvent)(anyID transferID, unsigned int status, const char* statusMessage, uint64 remotefileSize, uint64 schid, void* object);
  void (*onClientChatClosedEvent)(uint64 schid, anyID clientID, void* object);
  void (*onClientChatComposingEvent)(uint64 schid, anyID clientID, void* object);
  void (*onServerLogEvent)(uint64 schid, const char* logTimestamp, const char* logChannel, int logLevel, const char* logMsg, void* object);
  void (*onServerLogFinishedEvent)(uint64 schid, void* object);
  void (*onServerQueryEvent)(uint64 schid, const char* result, void* object);
  void (*onMessageListEvent)(uint64 schid, uint64 messageID, const char* fromClientUniqueIdentity, const char* subject, uint64 timestamp, int flagRead, void* object);
  void (*onMessageGetEvent)(uint64 schid, uint64 messageID, const char* fromClientUniqueIdentity, const char* subject, const char* message, uint64 timestamp, void* object);
  void (*onClientDBIDfromUIDEvent)(uint64 schid, const char* uniqueClientIdentifier, uint64 clientDatabaseID, void* object);
  void (*onClientNamefromUIDEvent)(uint64 schid, const char* uniqueClientIdentifier, uint64 clientDatabaseID, const char* clientNickName, void* object);
  void (*onClientNamefromDBIDEvent)(uint64 schid, const char* uniqueClientIdentifier, uint64 clientDatabaseID, const char* clientNickName, void* object);
  void (*onComplainListEvent)(uint64 schid, uint64 targetClientDatabaseID, const char* targetClientNickName, uint64 fromClientDatabaseID, const char* fromClientNickName, const char* complainReason, uint64 timestamp, void* object);
  void (*onBanListEvent)(uint64 schid, uint64 banid, const char* ip, const char* name, const char* uid, uint64 creationTime, uint64 durationTime, const char* invokerName, uint64 invokercldbid, const char* invokeruid, const char* reason, int numberOfEnforcements, void* object);
  void (*onClientServerQueryLoginPasswordEvent)(uint64 schid, const char* loginPassword, void* object);
  void (*onPluginCommandEvent)(uint64 schid, const char* pluginName, const char* pluginCommand, void* object);
};
typedef struct Ets3_ipc_event_callbacks ts3_ipc_event_callbacks;

#ifdef __cplusplus
}
#endif

#endif //IPC_EVENTS_H_INCLUDED
