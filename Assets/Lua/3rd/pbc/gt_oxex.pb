
!
gt_oxex.protogt_msggt_base.proto"V
TagTableStatus 
cbtablelock (Rcbtablelock"
cbplaystatus (Rcbplaystatus"k
tagUserStatus
wTableID (RwTableID
wChairID (RwChairID"
cbUserStatus (RcbUserStatus"Õ
LogonUserID
user_id (	RuserId#
user_password (	RuserPassword!
user_Version (RuserVersion%
user_LoginType (RuserLoginType
kindtype (	Rkindtype"
strMachineID (	RstrMachineID"K
LogonRoomFail
failCode (RfailCode

failstring (	R
failstring"&
LogonRoomSuccess
code (Rcode"1
UserRoomEnter 
user (2.gt_msg.UserRuser"t
ConfigServer 
wTableCount (RwTableCount 
wChairCount (RwChairCount 
wServerType (RwServerType"A
SystemMessage
szstring (	Rszstring
wType (RwType"r
RoomTabInfo_OxEx

tablecount (R
tablecount>
tablestatusary (2.gt_msg.TagTableStatusRtablestatusary"e
UserSitDown
wtableid (Rwtableid
wchairid (Rwchairid

szpassword (	R
szpassword"Z
RequestFailure
	errorCode (R	errorCode*
szDescribeString (	RszDescribeString"”
GR_UserStatus
dwUserID (	RdwUserID5

UserStatus (2.gt_msg.tagUserStatusR
userStatus
lScore (RlScore
lInsure (RlInsure"
GR_USER_READY"
GR_GameOpiton"f
GR_TableStatus
wTableID (RwTableID8
TableStatus (2.gt_msg.TagTableStatusRtableStatus"þ
GR_UserScore
dwUserID (	RdwUserID
cbReason (RcbReason
lScore (RlScore
lInsure (RlInsure

dwWinCount (R
dwWinCount 
dwLostCount (RdwLostCount 
dwDrawCount (RdwDrawCount 
dwFleeCount (RdwFleeCount"F
GR_UserScoreChange
lScore (RlScore
lInsure (RlInsure"p
CMD_GR_UserStandUp
wTableID (RwTableID
wChairID (RwChairID"
cbForceLeave (RcbForceLeave"
CMD_GR_USEREXIT"§
OXEX_StatusFreeResponse

lCellScore (R
lCellScore

lTurnScore (R
lTurnScore$
lCollectScore (RlCollectScore&
szGameRoomName (	RszGameRoomName"ó
OXEX_StatusCallResponse 
wCallBanker (RwCallBanker$
cbDynamicJoin (RcbDynamicJoin"
cbPlayStatus (RcbPlayStatus

lTurnScore (R
lTurnScore$
lCollectScore (RlCollectScore&
szGameRoomName (	RszGameRoomName"¼
OXEX_StatusScoreResponse"
cbPlayStatus (RcbPlayStatus$
cbDynamicJoin (RcbDynamicJoin$
lTurnMaxScore (RlTurnMaxScore 
lTableScore (RlTableScore 
wBankerUser (RwBankerUser&
szGameRoomName (	RszGameRoomName

lTurnScore (R
lTurnScore$
lCollectScore (RlCollectScore"“
OXEX_StatusPlayResponse"
cbPlayStatus (RcbPlayStatus$
cbDynamicJoin (RcbDynamicJoin$
lTurnMaxScore (RlTurnMaxScore 
lTableScore (RlTableScore 
wBankerUser (RwBankerUser<
cbHandCardData (2.gt_msg.HandCardDataRcbHandCardData
bOxCard (RbOxCard

lTurnScore (R
lTurnScore$
lCollectScore	 (RlCollectScore&
szGameRoomName
 (	RszGameRoomName"]
OXEX_CallBankerResponse 
wCallBanker (RwCallBanker 
bFirstTimes (RbFirstTimes"`
OXEX_GameStartResponse$
lTurnMaxScore (RlTurnMaxScore 
wBankerUser (RwBankerUser"e
OXEX_AddScoreResponse$
wAddScoreUser (RwAddScoreUser&
lAddScoreCount (RlAddScoreCount"Þ
OXEX_GameEndResponse
lGameTax (RlGameTax

lGameScore (R
lGameScore"
cbOxCardType (RcbOxCardType<
cbHandCardData (2.gt_msg.HandCardDataRcbHandCardData(
cbHandCardCount (RcbHandCardCount"6
HandCardData&
cbHandCardData (RcbHandCardData"™
OXEX_SendCardResponse4

cbCardData (2.gt_msg.HandCardDataR
cbCardData 
cbCardCount (RcbCardCount(
bAllAndroidUser (RbAllAndroidUser"h
OXEX_AllCardResponse
bAICount (RbAICount4

cbCardData (2.gt_msg.HandCardDataR
cbCardData"7
OXEX_PlayerExitResponse
	wPlayerID (R	wPlayerID"K
OXEX_OpenCardResponse
	wPlayerID (R	wPlayerID
bOpen (RbOpen"2
OXEX_UserOutResponse
wChairID (RwChairID"2
OXEX_CallBankerRequest
bBanker (RbBanker"9
OXEX_SPECIALRequest"
wUserChairID (RwUserChairID".
OXEX_AddScoreRequest
lScore (RlScore"&
OXEX_OxCardRequest
bOX (RbOXbproto3