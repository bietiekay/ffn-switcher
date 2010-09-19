#include <iostream>
#include <stdio.h>
#include <string.h>

#ifdef WIN32
  #include <conio.h>
#endif

#include "ipc_client_commands.h"
#include "public_errors.h"

#ifdef linux
  #include <sys/ipc.h>
  #include <termios.h>
  #include <errno.h>
#endif

#define MAXBUF 1000


#ifdef linux
#define MKFLAG(which) \
static int io_tio_set_flag_##which(int fd, int val, int on, int *old) \
{ struct termios tio; \
    if (tcgetattr(fd,&tio)) return -1; \
	if (old) *old=(tio.which & (val)); \
    if (on) tio.which |= (val); \
    else tio.which &= ~(val); \
    if (tcsetattr(fd,TCSADRAIN,&tio)) return -1; \
    return 0; \
} \
static int io_tio_get_flag_##which(int fd, int bit, int *value) \
{ struct termios tio; \
    if (tcgetattr(fd,&tio)) return -1; \
	*value=(tio.which & (bit)); \
    return 0; \
}
MKFLAG(c_lflag)

static int getch(int fd, char *c) {
	int old_ICANON, old_ECHO;
	int restoreflag = 0;
	int e = 0;
	int retcode = -1;

	if (-1==io_tio_set_flag_c_lflag(fd,ICANON,0,&old_ICANON)) {
		if (errno!=ENOTTY
			&& errno!=EINVAL) { /* LINUX /dev/random lossage */
			e=errno;
			perror("Turnoff of ICANON failed");
			errno=e;
			return -1;
		}
	} else
		restoreflag=1;
	if (restoreflag && -1==io_tio_set_flag_c_lflag(fd,ECHO,0,&old_ECHO)) {
		e=errno;
		perror("Turnoff of ICANON failed");
	} else {
		if (restoreflag) restoreflag++;
		while (1) {
			retcode=read(fd,c,1);
			if (retcode==1 || retcode==0) break;
			if (errno==EINTR) continue;
			break;
		}
	}
	/* we do not check for errors anymore: If we were able to change
	 * a flag before and are unable to do that afterwards then we
	 * are in unsolvable trouble anyway.
	 * btw, it _could_ happen, in case revoke() is summoned upon
	 * us. Oh, well.
	 */
	/* set echo to old value */
	if (restoreflag==2)
		io_tio_set_flag_c_lflag(fd,ECHO,old_ECHO,NULL);
	/* back to canonical input mode (line by line) */
	if (restoreflag)
		io_tio_set_flag_c_lflag(0,ICANON,old_ICANON,NULL);
    errno=e;
	return retcode;
}
#endif

char buf[MAXBUF];
int id;
bool keeprunning;

void reprintinput() {
  putchar('(');
  for (int i = 0; i < id; ++i)
    putchar(buf[i]);
  putchar(')');
  fflush(stdout);
}

void processCommand(uint64 schid, const char* command, void* object) {
  printf("processcommand %llu %s\n", (long long unsigned int)schid, command);
  reprintinput();
}

void currentServerConnectionChanged(uint64 schid, void* object) {
  printf("currentServerConnectionChanged %llu\n", (long long unsigned int)schid);
  reprintinput();
}

void pluginEvent(unsigned short data, const char* message, void* object) {
  printf("pluginEvent %d %s\n", data, message);
  reprintinput();
}

void onConnectStatusChangeEvent(uint64 schid, int newStatus, unsigned int errorNumber, void* object) {
  printf("onConnectStatusChangeEvent %llu %d %d\n", (long long unsigned int)schid, newStatus, errorNumber);
  reprintinput();
}

void onNewChannelEvent(uint64 schid, uint64 channelID, uint64 channelParentID, void* object) {
  printf("onNewChannelEvent %llu %llu %llu\n", (long long unsigned int)schid, (long long unsigned int)channelID, (long long unsigned int)channelParentID);
  reprintinput();
}

void onNewChannelCreatedEvent(uint64 schid, uint64 channelID, uint64 channelParentID, anyID invokerID, const char* invokerName, const char* invokerUniqueIdentifier, void* object) {
  printf("onNewChannelCreatedEvent %llu %llu %llu %d %s %s\n", (long long unsigned int)schid, (long long unsigned int)channelID, (long long unsigned int)channelParentID, invokerID, invokerName, invokerUniqueIdentifier);
  reprintinput();
}

void onDelChannelEvent(uint64 schid, uint64 channelID, anyID invokerID, const char* invokerName, const char* invokerUniqueIdentifier, void* object) {
  printf("onDelChannelEvent %llu %llu %d %s %s\n", (long long unsigned int)schid, (long long unsigned int)channelID, invokerID, invokerName, invokerUniqueIdentifier);
  reprintinput();
}

void onChannelMoveEvent(uint64 schid, uint64 channelID, uint64 newChannelParentID, anyID invokerID, const char* invokerName, const char* invokerUniqueIdentifier, void* object) {
  printf("onChannelMoveEvent %llu %llu %llu %d %s %s\n", (long long unsigned int)schid, (long long unsigned int)channelID, (long long unsigned int)newChannelParentID, invokerID, invokerName, invokerUniqueIdentifier);
  reprintinput();
}

void onUpdateChannelEvent(uint64 schid, uint64 channelID, void* object) {
  printf("onUpdateChannelEvent %llu %llu\n", (long long unsigned int)schid, (long long unsigned int)channelID);
  reprintinput();
}

void onUpdateChannelEditedEvent(uint64 schid, uint64 channelID, anyID invokerID, const char* invokerName, const char* invokerUniqueIdentifier, void* object) {
  printf("onUpdateChannelEditedEvent %llu %llu %d %s %s\n", (long long unsigned int)schid, (long long unsigned int)channelID, invokerID, invokerName, invokerUniqueIdentifier);
  reprintinput();
}

void onUpdateClientEvent(uint64 schid, anyID clientID, void* object) {
  printf("onUpdateClientEvent %llu %d\n", (long long unsigned int)schid, clientID);
  reprintinput();
}

void onClientMoveEvent(uint64 schid, anyID clientID, uint64 oldChannelID, uint64 newChannelID, int visibility, const char* moveMessage, void* object) {
  printf("onClientMoveEvent %llu %d %llu %llu %d %s\n", (long long unsigned int)schid, clientID, (long long unsigned int)oldChannelID, (long long unsigned int)newChannelID, visibility, moveMessage);
  reprintinput();
}

void onClientMoveSubscriptionEvent(uint64 schid, anyID clientID, uint64 oldChannelID, uint64 newChannelID, int visibility, void* object) {
  printf("onClientMoveSubscriptionEvent %llu %d %llu %llu %d\n", (long long unsigned int)schid, clientID, (long long unsigned int)oldChannelID, (long long unsigned int)newChannelID, visibility);
  reprintinput();
}

void onClientMoveTimeoutEvent(uint64 schid, anyID clientID, uint64 oldChannelID, uint64 newChannelID, int visibility, const char* timeoutMessage, void* object) {
  printf("onClientMoveTimeoutEvent %llu %d %llu %llu %d %s\n", (long long unsigned int)schid, clientID, (long long unsigned int)oldChannelID, (long long unsigned int)newChannelID, visibility, timeoutMessage);
  reprintinput();
}

void onClientMoveMovedEvent(uint64 schid, anyID clientID, uint64 oldChannelID, uint64 newChannelID, int visibility, anyID moverID, const char* moverName, const char* moverUniqueIdentifier, const char* moveMessage, void* object) {
  printf("onClientMoveMovedEvent %llu %d %llu %llu %d %d %s %s %s\n", (long long unsigned int)schid, clientID, (long long unsigned int)oldChannelID, (long long unsigned int)newChannelID, visibility, moverID, moverName, moverUniqueIdentifier, moveMessage);
  reprintinput();
}

void onClientKickFromChannelEvent(uint64 schid, anyID clientID, uint64 oldChannelID, uint64 newChannelID, int visibility, anyID kickerID, const char* kickerName, const char* kickerUniqueIdentifier, const char* kickMessage, void* object) {
  printf("onClientKickFromChannelEvent %llu %d %llu %llu %d %d %s %s %s\n", (long long unsigned int)schid, clientID, (long long unsigned int)oldChannelID, (long long unsigned int)newChannelID, visibility, kickerID, kickerName, kickerUniqueIdentifier, kickMessage);
  reprintinput();
}

void onClientKickFromServerEvent(uint64 schid, anyID clientID, uint64 oldChannelID, uint64 newChannelID, int visibility, anyID kickerID, const char* kickerName, const char* kickerUniqueIdentifier, const char* kickMessage, void* object) {
  printf("onClientKickFromServerEvent %llu %d %llu %llu %d %d %s %s %s\n", (long long unsigned int)schid, clientID, (long long unsigned int)oldChannelID, (long long unsigned int)newChannelID, visibility, kickerID, kickerName, kickerUniqueIdentifier, kickMessage);
  reprintinput();
}

void onServerEditedEvent(uint64 schid, anyID editerID, const char* editerName, const char* editerUniqueIdentifier, void* object) {
  printf("onServerEditedEvent %llu %d %s %s\n", (long long unsigned int)schid, editerID, editerName, editerUniqueIdentifier);
  reprintinput();
}

void onServerUpdatedEvent(uint64 schid, void* object) {
  printf("onServerUpdatedEvent %llu\n", (long long unsigned int)schid);
  reprintinput();
}

void onServerErrorEvent(uint64 schid, const char* errorMessage, unsigned int error, const char* returnCode, const char* extraMessage, void* object) {
  printf("onServerErrorEvent %llu %s %d %s %s\n", (long long unsigned int)schid, errorMessage, error, returnCode, extraMessage);
  reprintinput();
}

void onServerStopEvent(uint64 schid, const char* shutdownMessage, void* object) {
  printf("onServerStopEvent %llu %s\n", (long long unsigned int)schid, shutdownMessage);
  reprintinput();
}

void onTextMessageEvent(uint64 schid, anyID targetMode, anyID toID, anyID fromID, const char* fromName, const char* fromUniqueIdentifier, const char* message, int ffIgnored, void* object) {
  printf("onTextMessageEvent %llu %d %d %d %s %s %s %d\n", (long long unsigned int)schid, targetMode, toID, fromID, fromName, fromUniqueIdentifier, message, ffIgnored);
  reprintinput();
}

void onTalkStatusChangeEvent(uint64 schid, int status, int isReceivedWhisper, anyID clientID, void* object) {
  printf("onTalkStatusChangeEvent %llu %d %d %d\n", (long long unsigned int)schid, status, isReceivedWhisper, clientID);
  reprintinput();
}

void onConnectionInfoEvent(uint64 schid, anyID clientID, void* object) {
  printf("onConnectionInfoEvent %llu %d\n", (long long unsigned int)schid, clientID);
  reprintinput();
}

void onServerConnectionInfoEvent(uint64 schid, void* object) {
  printf("onServerConnectionInfoEvent %llu\n", (long long unsigned int)schid);
  reprintinput();
}

void onChannelSubscribeEvent(uint64 schid, uint64 channelID, void* object) {
  printf("onChannelSubscribeEvent %llu %llu\n", (long long unsigned int)schid, (long long unsigned int)channelID);
  reprintinput();
}

void onChannelSubscribeFinishedEvent(uint64 schid, void* object) {
  printf("onChannelSubscribeFinishedEvent %llu\n", (long long unsigned int)schid);
  reprintinput();
}

void onChannelUnsubscribeEvent(uint64 schid, uint64 channelID, void* object) {
  printf("onChannelUnsubscribeEvent %llu %llu\n", (long long unsigned int)schid, (long long unsigned int)channelID);
  reprintinput();
}

void onChannelUnsubscribeFinishedEvent(uint64 schid, void* object) {
  printf("onChannelUnsubscribeFinishedEvent %llu\n", (long long unsigned int)schid);
  reprintinput();
}

void onChannelDescriptionUpdateEvent(uint64 schid, uint64 channelID, void* object) {
  printf("onChannelDescriptionUpdateEvent %llu %llu\n", (long long unsigned int)schid, (long long unsigned int)channelID);
  reprintinput();
}

void onChannelPasswordChangedEvent(uint64 schid, uint64 channelID, void* object) {
  printf("onChannelPasswordChangedEvent %llu %llu\n", (long long unsigned int)schid, (long long unsigned int)channelID);
  reprintinput();
}

void onPlaybackShutdownCompleteEvent(uint64 schid, void* object) {
  printf("onPlaybackShutdownCompleteEvent %llu\n", (long long unsigned int)schid);
  reprintinput();
}

void onUserLoggingMessageEvent(const char* logMessage, int logLevel, const char* logChannel, uint64 logID, const char* logTime, const char* completeLogString, void* object) {
  printf("onUserLoggingMessageEvent %s %d %s %llu %s %s\n", logMessage, logLevel, logChannel, (long long unsigned int)logID, logTime, completeLogString);
  reprintinput();
}

void onVoiceRecordDataEvent(const float* data, unsigned int dataSize, void* object) {
  printf("onVoiceRecordDataEvent\n");
  reprintinput();
}

void onClientBanFromServerEvent(uint64 schid, anyID clientID, uint64 oldChannelID, uint64 newChannelID, int visibility, anyID kickerID, const char* kickerName, const char* kickerUniqueIdentifier, uint64 time, const char* kickMessage, void* object) {
  printf("onClientBanFromServerEvent %llu %d %llu %llu %d %d %s %s %llu %s\n", (long long unsigned int)schid, clientID, (long long unsigned int)oldChannelID, (long long unsigned int)newChannelID, visibility, kickerID, kickerName, kickerUniqueIdentifier, (long long unsigned int)time, kickMessage);
  reprintinput();
}

void onClientPokeEvent(uint64 schid, anyID fromClientID, const char* pokerName, const char* pokerUniqueIdentity, const char* message, int ffIgnored, void* object) {
  printf("onClientPokeEvent %llu %d %s %s %s %d\n", (long long unsigned int)schid, fromClientID, pokerName, pokerUniqueIdentity, message, ffIgnored);
  reprintinput();
}

void onClientSelfVariableUpdateEvent(uint64 schid, int flag, const char* oldValue, const char* newValue, void* object) {
  printf("onClientSelfVariableUpdateEvent %llu %d %s %s\n", (long long unsigned int)schid, flag, oldValue, newValue);
  reprintinput();
}

void onFileListEvent(uint64 schid, uint64 channelID, const char* path, const char* name, uint64 size, uint64 datetime, int type, void* object) {
  printf("onFileListEvent %llu %llu %s %s %llu %llu %d\n", (long long unsigned int)schid, (long long unsigned int)channelID, path, name, (long long unsigned int)size, (long long unsigned int)datetime, type);
  reprintinput();
}

void onFileListFinishedEvent(uint64 schid, uint64 channelID, const char* path, void* object) {
  printf("onFileListFinishedEvent %llu %llu %s\n", (long long unsigned int)schid, (long long unsigned int)channelID, path);
  reprintinput();
}

void onFileInfoEvent(uint64 schid, uint64 channelID, const char* name, uint64 size, uint64 datetime, void* object) {
  printf("onFileInfoEvent %llu %llu %s %llu %llu\n", (long long unsigned int)schid, (long long unsigned int)channelID, name, (long long unsigned int)size, (long long unsigned int)datetime);
  reprintinput();
}

void onServerGroupListEvent(uint64 schid, uint64 serverGroupID, const char* name, int type, int iconID, int saveDB, void* object) {
  printf("onServerGroupListEvent %llu %llu %s %d %d %d\n", (long long unsigned int)schid, (long long unsigned int)serverGroupID, name, type, iconID, saveDB);
  reprintinput();
}

void onServerGroupListFinishedEvent(uint64 schid, void* object) {
  printf("onServerGroupListFinishedEvent %llu\n", (long long unsigned int)schid);
  reprintinput();
}

void onServerGroupByClientIDEvent(uint64 schid, const char* name, uint64 serverGroupList, uint64 clientDatabaseID, void* object) {
  printf("onServerGroupByClientIDEvent %llu %s %llu %llu\n", (long long unsigned int)schid, name, (long long unsigned int)serverGroupList, (long long unsigned int)clientDatabaseID);
  reprintinput();
}

void onServerGroupPermListEvent(uint64 schid, uint64 serverGroupID, anyID permissionID, int permissionValue, int permissionNegated, int permissionSkip, void* object) {
  printf("onServerGroupPermListEvent %llu %llu %d %d %d %d\n", (long long unsigned int)schid, (long long unsigned int)serverGroupID, permissionID, permissionValue, permissionNegated, permissionSkip);
  reprintinput();
}

void onServerGroupPermListFinishedEvent(uint64 schid, uint64 serverGroupID, void* object) {
  printf("onServerGroupPermListFinishedEvent %llu %llu\n", (long long unsigned int)schid, (long long unsigned int)serverGroupID);
  reprintinput();
}

void onServerGroupClientListEvent(uint64 schid, uint64 serverGroupID, uint64 clientDatabaseID, const char* clientNameIdentifier, const char* clientUniqueID, void* object) {
  printf("onServerGroupClientListEvent %llu %llu %llu %s %s\n", (long long unsigned int)schid, (long long unsigned int)serverGroupID, (long long unsigned int)clientDatabaseID, clientNameIdentifier, clientUniqueID);
  reprintinput();
}

void onChannelGroupListEvent(uint64 schid, uint64 channelGroupID, const char* name, int type, int iconID, int saveDB, void* object) {
  printf("onChannelGroupListEvent %llu %llu %s %d %d %d\n", (long long unsigned int)schid, (long long unsigned int)channelGroupID, name, type, iconID, saveDB);
  reprintinput();
}

void onChannelGroupListFinishedEvent(uint64 schid, void* object) {
  printf("onChannelGroupListFinishedEvent %llu\n", (long long unsigned int)schid);
  reprintinput();
}

void onChannelGroupPermListEvent(uint64 schid, uint64 channelGroupID, anyID permissionID, int permissionValue, int permissionNegated, int permissionSkip, void* object) {
  printf("onChannelGroupPermListEvent %llu %llu %d %d %d %d\n", (long long unsigned int)schid, (long long unsigned int)channelGroupID, permissionID, permissionValue, permissionNegated, permissionSkip);
  reprintinput();
}

void onChannelGroupPermListFinishedEvent(uint64 schid, uint64 channelGroupID, void* object) {
  printf("onChannelGroupPermListFinishedEvent %llu %llu\n", (long long unsigned int)schid, (long long unsigned int)channelGroupID);
  reprintinput();
}

void onChannelPermListEvent(uint64 schid, uint64 channelID, anyID permissionID, int permissionValue, int permissionNegated, int permissionSkip, void* object) {
  printf("onChannelPermListEvent %llu %llu %d %d %d %d\n", (long long unsigned int)schid, (long long unsigned int)channelID, permissionID, permissionValue, permissionNegated, permissionSkip);
  reprintinput();
}

void onChannelPermListFinishedEvent(uint64 schid, uint64 channelID, void* object) {
  printf("onChannelPermListFinishedEvent %llu %llu\n", (long long unsigned int)schid, (long long unsigned int)channelID);
  reprintinput();
}

void onClientPermListEvent(uint64 schid, uint64 clientDatabaseID, anyID permissionID, int permissionValue, int permissionNegated, int permissionSkip, void* object) {
  printf("onClientPermListEvent %llu %llu %d %d %d %d\n", (long long unsigned int)schid, (long long unsigned int)clientDatabaseID, permissionID, permissionValue, permissionNegated, permissionSkip);
  reprintinput();
}

void onClientPermListFinishedEvent(uint64 schid, uint64 clientDatabaseID, void* object) {
  printf("onClientPermListFinishedEvent %llu %llu\n", (long long unsigned int)schid, (long long unsigned int)clientDatabaseID);
  reprintinput();
}

void onChannelClientPermListEvent(uint64 schid, uint64 channelID, uint64 clientDatabaseID, anyID permissionID, int permissionValue, int permissionNegated, int permissionSkip, void* object) {
  printf("onChannelClientPermListEvent %llu %llu %llu %d %d %d %d\n", (long long unsigned int)schid, (long long unsigned int)channelID, (long long unsigned int)clientDatabaseID, permissionID, permissionValue, permissionNegated, permissionSkip);
  reprintinput();
}

void onChannelClientPermListFinishedEvent(uint64 schid, uint64 channelID, uint64 clientDatabaseID, void* object) {
  printf("onChannelClientPermListFinishedEvent %llu %llu %llu\n", (long long unsigned int)schid, (long long unsigned int)channelID, (long long unsigned int)clientDatabaseID);
  reprintinput();
}

void onClientChannelGroupChangedEvent(uint64 schid, uint64 channelGroupID, uint64 channelID, anyID clientID, anyID invokerClientID, const char* invokerName, const char* invokerUniqueIdentity, void* object) {
  printf("onClientChannelGroupChangedEvent %llu %llu %llu %d %d %s %s\n", (long long unsigned int)schid, (long long unsigned int)channelGroupID, (long long unsigned int)channelID, clientID, invokerClientID, invokerName, invokerUniqueIdentity);
  reprintinput();
}

void onServerPermissionErrorEvent(uint64 schid, const char* errorMessage, unsigned int error, const char* returnCode, anyID failedPermissionID, void* object) {
  printf("onServerPermissionErrorEvent %llu %s %d %s %d\n", (long long unsigned int)schid, errorMessage, error, returnCode, failedPermissionID);
  reprintinput();
}

void onPermissionListEvent(uint64 schid, anyID permissionID, const char* permissionName, const char* permissionDescription, void* object) {
  printf("onPermissionListEvent %llu %d %s %s\n", (long long unsigned int)schid, permissionID, permissionName, permissionDescription);
  reprintinput();
}

void onPermissionListFinishedEvent(uint64 schid, void* object) {
  printf("onPermissionListFinishedEvent %llu\n", (long long unsigned int)schid);
  reprintinput();
}

void onPermissionOverviewEvent(uint64 schid, uint64 clientDatabaseID, uint64 channelID, int overviewType, uint64 overviewID1, uint64 overviewID2, anyID permissionID, int permissionValue, int permissionNegated, int permissionSkip, void* object) {
  printf("onPermissionOverviewEvent %llu %llu %llu %d %llu %llu %d %d %d %d\n", (long long unsigned int)schid, (long long unsigned int)clientDatabaseID, (long long unsigned int)channelID, overviewType, (long long unsigned int)overviewID1, (long long unsigned int)overviewID2, permissionID, permissionValue, permissionNegated, permissionSkip);
  reprintinput();
}

void onPermissionOverviewFinishedEvent(uint64 schid, void* object) {
  printf("onPermissionOverviewFinishedEvent %llu\n", (long long unsigned int)schid);
  reprintinput();
}

void onServerGroupClientAddedEvent(uint64 schid, anyID clientID, const char* clientName, const char* clientUniqueIdentity, uint64 serverGroupID, anyID invokerClientID, const char* invokerName, const char* invokerUniqueIdentity, void* object) {
  printf("onServerGroupClientAddedEvent %llu %d %s %s %llu %d %s %s\n", (long long unsigned int)schid, clientID, clientName, clientUniqueIdentity, (long long unsigned int)serverGroupID, invokerClientID, invokerName, invokerUniqueIdentity);
  reprintinput();
}

void onServerGroupClientDeletedEvent(uint64 schid, anyID clientID, const char* clientName, const char* clientUniqueIdentity, uint64 serverGroupID, anyID invokerClientID, const char* invokerName, const char* invokerUniqueIdentity, void* object) {
  printf("onServerGroupClientDeletedEvent %llu %d %s %s %llu %d %s %s\n", (long long unsigned int)schid, clientID, clientName, clientUniqueIdentity, (long long unsigned int)serverGroupID, invokerClientID, invokerName, invokerUniqueIdentity);
  reprintinput();
}

void onClientNeededPermissionsEvent(uint64 schid, anyID permissionID, int permissionValue, void* object) {
  printf("onClientNeededPermissionsEvent %llu %d %d\n", (long long unsigned int)schid, permissionID, permissionValue);
  reprintinput();
}

void onClientNeededPermissionsFinishedEvent(uint64 schid, void* object) {
  printf("onClientNeededPermissionsFinishedEvent %llu\n", (long long unsigned int)schid);
  reprintinput();
}

void onFileTransferStatusEvent(anyID transferID, unsigned int status, const char* statusMessage, uint64 remotefileSize, uint64 schid, void* object) {
  printf("onFileTransferStatusEvent %d %d %s %llu %llu\n", transferID, status, statusMessage, (long long unsigned int)remotefileSize, (long long unsigned int)schid);
  reprintinput();
}

void onClientChatClosedEvent(uint64 schid, anyID clientID, void* object) {
  printf("onClientChatClosedEvent %llu %d\n", (long long unsigned int)schid, clientID);
  reprintinput();
}

void onClientChatComposingEvent(uint64 schid, anyID clientID, void* object) {
  printf("onClientChatComposingEvent %llu %d\n", (long long unsigned int)schid, clientID);
  reprintinput();
}

void onServerLogEvent(uint64 schid, const char* logTimestamp, const char* logChannel, int logLevel, const char* logMsg, void* object) {
  printf("onServerLogEvent %llu %s %s %d %s\n", (long long unsigned int)schid, logTimestamp, logChannel, logLevel, logMsg);
  reprintinput();
}

void onServerLogFinishedEvent(uint64 schid, void* object) {
  printf("onServerLogFinishedEvent %llu\n", (long long unsigned int)schid);
  reprintinput();
}

void onServerQueryEvent(uint64 schid, const char* result, void* object) {
  printf("onServerQueryEvent %llu %s\n", (long long unsigned int)schid, result);
  reprintinput();
}

void onMessageListEvent(uint64 schid, uint64 messageID, const char* fromClientUniqueIdentity, const char* subject, uint64 timestamp, int flagRead, void* object) {
  printf("onMessageListEvent %llu %llu %s %s %llu %d\n", (long long unsigned int)schid, (long long unsigned int)messageID, fromClientUniqueIdentity, subject, (long long unsigned int)timestamp, flagRead);
  reprintinput();
}

void onMessageGetEvent(uint64 schid, uint64 messageID, const char* fromClientUniqueIdentity, const char* subject, const char* message, uint64 timestamp, void* object) {
  printf("onMessageGetEvent %llu %llu %s %s %s %llu\n", (long long unsigned int)schid, (long long unsigned int)messageID, fromClientUniqueIdentity, subject, message, (long long unsigned int)timestamp);
  reprintinput();
}

void onClientDBIDfromUIDEvent(uint64 schid, const char* uniqueClientIdentifier, uint64 clientDatabaseID, void* object) {
  printf("onClientDBIDfromUIDEvent %llu %s %llu\n", (long long unsigned int)schid, uniqueClientIdentifier, (long long unsigned int)clientDatabaseID);
  reprintinput();
}

void onClientNamefromUIDEvent(uint64 schid, const char* uniqueClientIdentifier, uint64 clientDatabaseID, const char* clientNickName, void* object) {
  printf("onClientNamefromUIDEvent %llu %s %llu %s\n", (long long unsigned int)schid, uniqueClientIdentifier, (long long unsigned int)clientDatabaseID, clientNickName);
  reprintinput();
}

void onClientNamefromDBIDEvent(uint64 schid, const char* uniqueClientIdentifier, uint64 clientDatabaseID, const char* clientNickName, void* object) {
  printf("onClientNamefromDBIDEvent %llu %s %llu %s\n", (long long unsigned int)schid, uniqueClientIdentifier, (long long unsigned int)clientDatabaseID, clientNickName);
  reprintinput();
}

void onComplainListEvent(uint64 schid, uint64 targetClientDatabaseID, const char* targetClientNickName, uint64 fromClientDatabaseID, const char* fromClientNickName, const char* complainReason, uint64 timestamp, void* object) {
  printf("onComplainListEvent %llu %llu %s %llu %s %s %llu\n", (long long unsigned int)schid, (long long unsigned int)targetClientDatabaseID, targetClientNickName, (long long unsigned int)fromClientDatabaseID, fromClientNickName, complainReason, (long long unsigned int)timestamp);
  reprintinput();
}

void onBanListEvent(uint64 schid, uint64 banid, const char* ip, const char* name, const char* uid, uint64 creationTime, uint64 durationTime, const char* invokerName, uint64 invokercldbid, const char* invokeruid, const char* reason, int numberOfEnforcements, void* object) {
  printf("onBanListEvent %llu %llu %s %s %s %llu %llu %s %llu %s %s %d\n", (long long unsigned int)schid, (long long unsigned int)banid, ip, name, uid, (long long unsigned int)creationTime, (long long unsigned int)durationTime, invokerName, (long long unsigned int)invokercldbid, invokeruid, reason, numberOfEnforcements);
  reprintinput();
}

void onClientServerQueryLoginPasswordEvent(uint64 schid, const char* loginPassword, void* object) {
  printf("onClientServerQueryLoginPasswordEvent %llu %s\n", (long long unsigned int)schid, loginPassword);
  reprintinput();
}

void onPluginCommandEvent(uint64 schid, const char* pluginName, const char* pluginCommand, void* object) {
  printf("onPluginCommandEvent %llu %s %s\n", (long long unsigned int)schid, pluginName, pluginCommand);
  reprintinput();
}

void printUsage(char* name) {
  printf("usage: %s [option]\n", name);
  printf("\n");
  printf("--noevents \t Don't register for events\n");
}

void printHelp() {
  printf("Available commands:\n");
  printf("help \t\t\t\t\t\t\t Show this help\n");
  printf("printMessageToCurrentTab <str> \t\t\t\t Prints the string <str> to the current tab\n");
  printf("getCurrentServerConnectionHandler \t\t\t Returns the active schid\n");
  printf("getClientID <schid> \t\t\t\t\t Returns your clientid ont <schid>\n");
  printf("getClientLibVersion \t\t\t\t\t Prints the client's lib version\n");
  printf("getServerConnectionHandlerList \t\t\t\t Prints the list of available schids\n");
  printf("getErrorMessage <d> \t\t\t\t\t Prints the message of the error <u>\n");
  printf("logMessage <str1> <d> <str2> <schid> \t\t\t Adds the string <str1> to the clientlog with the severity <d>, in channel <str2> and with schandler <schid>\n");
  printf("requestClientPoke <schid> <d> <str1> <str2> \t\t Pokes the client with id <d> on <schid> with the msg <str1>. Return code is <str2>\n");
  printf("requestClientKickFromChannel <schid> <d> <str1> <str2> \t Kicks the client with id <d> on <schid> from channel with reason <str1>. Return code is <str2>\n");
  printf("requestClientKickFromServer <schid> <d> <str1> <str2> \t Kicks the client with id <d> on <schid> from server with reason <str1>. Return code is <str2>\n");
  printf("getClientList <schid> \t\t\t\t\t Returns the list of client ids on <schid>\n");
  printf("getChannelList <schid> \t\t\t\t\t Returns the list of channel ids on <schid>\n");
  printf("quit \t\t\t\t\t\t\t Quit the application\n");
}

void clearbuf(char* buf, int size) {
  for (int i = 0; i < size; ++i)
    buf[i] = 0;
}

void run_cmd(char* buf, int lastc) {
  char out[60];
  out[0] = 0;
  int count;
  unsigned int res;

  if (!strncmp(buf, "printmessagetocurrenttab", (count = strlen("printmessagetocurrenttab")))) {
    strncpy(out, buf + count, lastc - count);
    printf("error=%d\n", printMessageToCurrentTab(out));
  }
  else if (!strncmp(buf, "getcurrentserverconnectionhandlerid", (count = strlen("getcurrentserverconnectionhandlerid")))) {
    strncpy(out, buf + count, lastc - count);
    printf("schid=%llu\n", (long long unsigned int)getCurrentServerConnectionHandlerID(0));
  }
  else if (!strncmp(buf, "getclientid", (count = strlen("getclientid")))) {
    long long unsigned int schid;
    anyID id;
    strncpy(out, buf + count, lastc - count);
    if (sscanf(out, "%llu", &schid) != EOF) {
      if ((res = getClientID(schid, &id)) == ERROR_ok)
        printf("id=%d\n", id);
      else printf("error id=%d\n", res);
    }
    else printf("error, invalid parameter count\n");
  }
  else if (!strncmp(buf, "getclientlibversion", (count = strlen("getclientlibversion")))) {
    char* ver;
    if ((res = getClientLibVersion(&ver)) == ERROR_ok) {
      printf("version=%s\n", ver);
      freeMemory(ver);
    }
    else printf("error id=%d\n", res);
  }
  else if (!strncmp(buf, "getserverconnectionhandlerlist", (count = strlen("getserverconnectionhandlerlist")))) {
    uint64* schids;
    if ((res = getServerConnectionHandlerList(&schids)) == ERROR_ok) {
      for (int i = 0; schids[i]; ++i)
        printf("%llu\n", (long long unsigned int)schids[i]);
      printf("ok\n");
      freeMemory(schids);
    }
  }
  else if (!strncmp(buf, "geterrormessage", (count = strlen("geterrormessage")))) {
    unsigned int code;
    char* err;
    strncpy(out, buf + count, lastc - count);
    if (sscanf(out, "%u", &code) != EOF) {
      if ((res = getErrorMessage(code, &err)) == ERROR_ok) {
        printf("msg=%s\n", err);
        freeMemory(err);
      }
      else printf("error id=%d\n", res);
    }
    else printf("error, invalid parameter count\n");
  }
  else if (!strncmp(buf, "logmessage", (count = strlen("logmessage")))) {
    char msg[40];
    int sev;
    char chan[40];
    long long unsigned int schid;
    strncpy(out, buf + count, lastc - count);
    if (sscanf(out, "%40s %d %40s %llu", msg, &sev, chan, &schid) != EOF) {
      if ((res = logMessage(msg, (LogLevel)sev, chan, (uint64)schid)) == ERROR_ok)
        printf("ok\n");
      else printf("error id=%d\n", res);
    }
    else printf("error, invalid parameter count\n");
  }
  else if (!strncmp(buf, "requestclientpoke", (count = strlen("requestclientpoke")))) {
    long long unsigned int schid;
    unsigned int id;
    char msg[40];
    char code[40];
    strncpy(out, buf + count, lastc - count);
    if (sscanf(out, "%llu %u %40s %40s", &schid, &id, msg, code) != EOF) {
      printf("%llu %u %s %s\n", schid, id, msg, code);
      if ((res = requestClientPoke((uint64)schid, (anyID)id, msg, code)) == ERROR_ok)
        printf("ok\n");
      else printf("error id=%d\n", res);
    }
    else printf("error, invalid parameter count\n");
  }
  else if (!strncmp(buf, "requestclientkickfromchannel", (count = strlen("requestclientkickfromchannel")))) {
    long long unsigned int schid;
    unsigned int id;
    char reason[40];
    char code[40];
    strncpy(out, buf + count, lastc - count);
    if (sscanf(out, "%llu %u %s %s", &schid, &id, reason, code) != EOF) {
      if ((res = requestClientKickFromChannel((uint64)schid, (anyID)id, reason, code)) == ERROR_ok)
        printf("ok\n");
      else printf("error id=%d\n", res);
    }
    else printf("error, invalid parameter count\n");
  }
  else if (!strncmp(buf, "requestclientkickfromserver", (count = strlen("requestclientkickfromserver")))) {
    long long unsigned int schid;
    unsigned int id;
    char msg[40];
    char code[40];
    strncpy(out, buf + count, lastc - count);
    if (sscanf(out, "%llu %u %40s %40s", &schid, &id, msg, code) != EOF) {
      if ((res = requestClientKickFromServer((uint64)schid, (anyID)id, msg, code)) == ERROR_ok)
        printf("ok\n");
      else printf("error id=%d", res);
    }
    else printf("error, invalid parameters size\n");
  }
  else if (!strncmp(buf, "getclientlist", (count = strlen("getclientlist")))) {
    long long unsigned int schid;
    anyID* clients;
    strncpy(out, buf + count, lastc - count);
    if (sscanf(out, "%llu", &schid) != EOF) {
      if ((res = getClientList((uint64)schid, &clients)) == ERROR_ok) {
        int i;
        for (i = 0; clients[i]; ++i)
          printf("%d\n", clients[i]);
        printf("clientcount: %d\n", i);
        printf("ok\n");

        freeMemory(clients);
      }
      else printf("error id=%d\n", res);
    }
    else printf("error, invalid parameters count\n");
  }
  else if (!strncmp(buf, "getchannellist", (count = strlen("getchannellist")))) {
    long long unsigned int schid;
    uint64* chans;
    strncpy(out, buf + count, lastc - count);
    if (sscanf(out, "%llu", &schid) != EOF) {
      if ((res = getChannelList((uint64)schid, &chans)) == ERROR_ok) {
        int i;
        for (i = 0; chans[i]; ++i)
          printf("%llu\n", (long long unsigned int)chans[i]);
        printf("channelcount: %d\n", i);
        printf("ok\n");

        freeMemory(chans);
      }
      else printf("error id=%d\n", res);
    }
    else printf("error, invalid parameter count\n");
  }
  else if (!strcmp(buf, "quit"))
    keeprunning = false;
  else if (!strcmp(buf, "help"))
    printHelp();
  else printf("Unknown command \"%s\"\n", buf);
}

int main(int argc, char** argv) {
  bool events = true;
  keeprunning = true;
  bool behindspace = false;

#ifdef linux
  key_t key = ftok("./", 42);
  unsigned int ret = initSDK(key);
#else
  unsigned int ret = initSDK("exam");
#endif

  if (ret != ERROR_ok) {
    printf("Error initiating SDK=%d\n", ret);
    return 0;
  }

  if (argc > 1) {
    if (!strcmp(argv[1], "--noevents"))
      events = false;
    else {
      printUsage(argv[0]);
      return 0;
    }
  }

  if (events) {
    ts3_ipc_event_callbacks cbs;
    memset(&  cbs, 0, sizeof(ts3_ipc_event_callbacks));
    cbs.processCommand = processCommand;
    cbs.currentServerConnectionChanged = currentServerConnectionChanged;
    cbs.pluginEvent = pluginEvent;
    cbs.onConnectStatusChangeEvent = onConnectStatusChangeEvent;
    cbs.onNewChannelEvent = onNewChannelEvent;
    cbs.onNewChannelCreatedEvent = onNewChannelCreatedEvent;
    cbs.onDelChannelEvent = onDelChannelEvent;
    cbs.onChannelMoveEvent = onChannelMoveEvent;
    cbs.onUpdateChannelEvent = onUpdateChannelEvent;
    cbs.onUpdateChannelEditedEvent = onUpdateChannelEditedEvent;
    cbs.onUpdateClientEvent = onUpdateClientEvent;
    cbs.onClientMoveEvent = onClientMoveEvent;
    cbs.onClientMoveSubscriptionEvent = onClientMoveSubscriptionEvent;
    cbs.onClientMoveTimeoutEvent = onClientMoveTimeoutEvent;
    cbs.onClientMoveMovedEvent = onClientMoveMovedEvent;
    cbs.onClientKickFromChannelEvent = onClientKickFromChannelEvent;
    cbs.onClientKickFromServerEvent = onClientKickFromServerEvent;
    cbs.onServerEditedEvent = onServerEditedEvent;
    cbs.onServerUpdatedEvent = onServerUpdatedEvent;
    cbs.onServerErrorEvent = onServerErrorEvent;
    cbs.onServerStopEvent = onServerStopEvent;
    cbs.onTextMessageEvent = onTextMessageEvent;
    cbs.onTalkStatusChangeEvent = onTalkStatusChangeEvent;
    cbs.onConnectionInfoEvent = onConnectionInfoEvent;
    cbs.onServerConnectionInfoEvent = onServerConnectionInfoEvent;
    cbs.onChannelSubscribeEvent = onChannelSubscribeEvent;
    cbs.onChannelSubscribeFinishedEvent = onChannelSubscribeFinishedEvent;
    cbs.onChannelUnsubscribeEvent = onChannelUnsubscribeEvent;
    cbs.onChannelUnsubscribeFinishedEvent = onChannelUnsubscribeFinishedEvent;
    cbs.onChannelDescriptionUpdateEvent = onChannelDescriptionUpdateEvent;
    cbs.onChannelPasswordChangedEvent = onChannelPasswordChangedEvent;
    cbs.onPlaybackShutdownCompleteEvent = onPlaybackShutdownCompleteEvent;
    cbs.onUserLoggingMessageEvent = onUserLoggingMessageEvent;
    cbs.onVoiceRecordDataEvent = onVoiceRecordDataEvent;
    cbs.onClientBanFromServerEvent = onClientBanFromServerEvent;
    cbs.onClientPokeEvent = onClientPokeEvent;
    cbs.onClientSelfVariableUpdateEvent = onClientSelfVariableUpdateEvent;
    cbs.onFileListEvent = onFileListEvent;
    cbs.onFileListFinishedEvent = onFileListFinishedEvent;
    cbs.onFileInfoEvent = onFileInfoEvent;
    cbs.onServerGroupListEvent = onServerGroupListEvent;
    cbs.onServerGroupListFinishedEvent = onServerGroupListFinishedEvent;
    cbs.onServerGroupByClientIDEvent = onServerGroupByClientIDEvent;
    cbs.onServerGroupPermListEvent = onServerGroupPermListEvent;
    cbs.onServerGroupPermListFinishedEvent = onServerGroupPermListFinishedEvent;
    cbs.onServerGroupClientListEvent = onServerGroupClientListEvent;
    cbs.onChannelGroupListEvent = onChannelGroupListEvent;
    cbs.onChannelGroupListFinishedEvent = onChannelGroupListFinishedEvent;
    cbs.onChannelGroupPermListEvent = onChannelGroupPermListEvent;
    cbs.onChannelGroupPermListFinishedEvent = onChannelGroupPermListFinishedEvent;
    cbs.onChannelPermListEvent = onChannelPermListEvent;
    cbs.onChannelPermListFinishedEvent = onChannelPermListFinishedEvent;
    cbs.onClientPermListEvent = onClientPermListEvent;
    cbs.onClientPermListFinishedEvent = onClientPermListFinishedEvent;
    cbs.onChannelClientPermListEvent = onChannelClientPermListEvent;
    cbs.onChannelClientPermListFinishedEvent = onChannelClientPermListFinishedEvent;
    cbs.onClientChannelGroupChangedEvent = onClientChannelGroupChangedEvent;
    cbs.onServerPermissionErrorEvent = onServerPermissionErrorEvent;
    cbs.onPermissionListEvent = onPermissionListEvent;
    cbs.onPermissionListFinishedEvent = onPermissionListFinishedEvent;
    cbs.onPermissionOverviewEvent = onPermissionOverviewEvent;
    cbs.onPermissionOverviewFinishedEvent = onPermissionOverviewFinishedEvent;
    cbs.onServerGroupClientAddedEvent = onServerGroupClientAddedEvent;
    cbs.onServerGroupClientDeletedEvent = onServerGroupClientDeletedEvent;
    cbs.onClientNeededPermissionsEvent = onClientNeededPermissionsEvent;
    cbs.onClientNeededPermissionsFinishedEvent = onClientNeededPermissionsFinishedEvent;
    cbs.onFileTransferStatusEvent = onFileTransferStatusEvent;
    cbs.onClientChatClosedEvent = onClientChatClosedEvent;
    cbs.onClientChatComposingEvent = onClientChatComposingEvent;
    cbs.onServerLogEvent = onServerLogEvent;
    cbs.onServerLogFinishedEvent = onServerLogFinishedEvent;
    cbs.onServerQueryEvent = onServerQueryEvent;
    cbs.onMessageListEvent = onMessageListEvent;
    cbs.onMessageGetEvent = onMessageGetEvent;
    cbs.onClientDBIDfromUIDEvent = onClientDBIDfromUIDEvent;
    cbs.onClientNamefromUIDEvent = onClientNamefromUIDEvent;
    cbs.onClientNamefromDBIDEvent = onClientNamefromDBIDEvent;
    cbs.onComplainListEvent = onComplainListEvent;
    cbs.onBanListEvent = onBanListEvent;
    cbs.onClientServerQueryLoginPasswordEvent = onClientServerQueryLoginPasswordEvent;
    cbs.onPluginCommandEvent = onPluginCommandEvent;

    if (registerCallbacks(cbs) != ERROR_ok)
      printf("Error registering event callbacks\n");
    else printf("Events registered\n");
  }

  char c;
  id = 0;
  clearbuf(buf, MAXBUF);


  while (keeprunning) {
#ifdef WIN32
    c = getch();
#else
    getch(0, &c);
#endif

    if (id == MAXBUF -1) {
      clearbuf(buf, MAXBUF);
      id = 0;
      printf("ERROR, buffer full\n");
      continue;
    }

    switch (c) {
      case 10:
        if (id == 0)
          break;
        buf[id] = 0;
        buf[id+1] = ' ';
        putchar(c);
        run_cmd(buf, id);
        id = 0;
        clearbuf(buf, MAXBUF);
        behindspace = false;
        break;
      case 8:
        break;
      case ' ':
        if (id == 0)
          break;
        behindspace = true;
      default:
        if (behindspace)
          buf[id++] = c;
        else buf[id++] = tolower(c);
        putchar(c);
        fflush(stdout);
    }

  }

  ret = shutdownSDK();

  if (ret != ERROR_ok)
    printf("Error shutting down=%d\n", ret);
  else printf("Shutted down\n");

  return 0;
}
