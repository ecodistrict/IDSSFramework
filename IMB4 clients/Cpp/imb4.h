#ifndef IMB4_H_INCLUDED
#define IMB4_H_INCLUDED

//#pragma once

#include <limits>
#include <string>
#include <vector>
#include <list>
#include <WS2tcpip.h>
#include <map>
#include <cstdint>

// ecodistrict defaults
const std::string imbDefaultRemoteHost = "vps17642.public.cloudvps.com"; // "localhost"
const std::string imbDefaultPrefix = "ecodistrict"; // "nl.imb";

const int imbDefaultSocketRemotePort = 4004;
const int imbDefaultTLSRemotePort = 4443;

// protobuf wire types
const int wtVarInt = 0;                        // int32, int64, uint32, uint64_t, sint32, sint64, bool, enum
const int wt64Bit = 1;                         // double or fixed int64/uint64_t
const int wtLengthDelimited = 2;               // std::string, bytes, embedded messages, packed repeated fields
const int wtStartGroup = 3;                    // deprecated
const int wtEndGroup = 4;                      // deprecated
const int wt32Bit = 5;                         // float (single) or fixed int32/uint32

const int imbMaxStreamBodyBuffer = 8 * 1024;

// change object actions
const int actionNew = 0;
const int actionDelete = 1;
const int actionChange = 2;

// basic event tags
const int icehIntString = 1;                 // <varint>
const int icehIntStringPayload = 2;          // <std::string>
const int icehString = 3;                    // <std::string>
const int icehChangeObject = 4;              // <int32: varint>
const int icehChangeObjectAction = 5;        // <int32: varint>
const int icehChangeObjectAttribute = 6;     // <std::string>

const int icehStreamHeader = 7;              // <std::string> filename
const int icehStreamBody = 8;                // <bytes>
const int icehStreamEnd = 9;                 // <bool> true: ok, false: cancel
const int icehStreamID = 10;                 // <id: bytes/std::string>

const byte imbMagic = 0xFE;

const int imbMinimumPacketSize = 16;
const int imbMaximumPayloadSize = 10 * 1024 * 1024;
const int imbSocketDefaultLingerTimeInSec = 2; // in sec

// client state
const int icsClient = 2;

// command tags
const int icehSubscribe = 2;                   // <uint32: varint>
const int icehPublish = 3;                     // <uint32: varint>
const int icehUnsubscribe = 4;                 // <uint32: varint>
const int icehUnpublish = 5;                   // <uint32: varint>
const int icehSetEventIDTranslation = 6;       // <uint32: varint>
const int icehEventName = 7;                   // <std::string>
const int icehEventID = 8;                     // <uint32: varint>

const int icehUniqueClientID = 11;             // <guid>
const int icehHubID = 12;                      // <guid>
const int icehModelName = 13;                  // <std::string>
const int icehModelID = 14;                    // <int32: varint> ?
const int icehState = 16;                      // <uint32: varint>
const int icehEventNameFilter = 17;            // <std::string>
const int icehClose = 21;                      // <bool: varint>

const uint32_t imbInvalidEventID = 0xFFFFFFFF;


// string utils
std::string toUpper(const std::string &s);
std::string intToString(int64_t i);
// guid utils
bool isEmptyGUID(GUID &aGUID);
void emptyGUID(GUID &aGUID);
std::string GUIDToHex(GUID &aGUID);


class TByteBuffer
{
public:
	TByteBuffer(int aCapacity = 0);
	TByteBuffer(const byte* aBuffer, int aLength);
	TByteBuffer(const byte* aBuffer, int aOffset, int aLimit);
	TByteBuffer(const TByteBuffer &aBuffer);
	~TByteBuffer(void);
public:
	TByteBuffer& operator=(const TByteBuffer &aBuffer);
public:
	byte* getBuffer() { return fBuffer; };
	byte* getBuffer(int aOffset) { if (fBuffer != 0) return &fBuffer[aOffset]; else return 0; };
	int getCapacity() { return fCapacity; };
	void setCapacity(int aNewCapacity);
	int getOffset() { return fOffset; };
	void setOffset(int aValue) { if (aValue >= 0) { fOffset = aValue; if (fOffset > fCapacity) setCapacity(fOffset); } };
	int getLimit() { return fLimit; };
	void setLimit(int aValue) { if (aValue >= 0) { fLimit = aValue; if (fLimit > fCapacity) setCapacity(fLimit); } };
	byte getFirstByte() { return fBuffer[0]; };
public:
	int remaining() { return fLimit - fOffset; };
	void shiftLeftOneByte(byte aRighbyte);
private:
	byte* fBuffer;
	int fCapacity;
	int fOffset;
	int fLimit;
public:
	uint16_t bb_read_uint16(); // fixed 16 bit (cannot be tagged!)

	uint64_t bb_read_uint64(); // unsigned varint
	int64_t bb_read_int64(); // signed varint
	uint32_t bb_read_uint32(); // unsigned varint
	int32_t bb_read_int32(); // signed varint
	bool bb_read_bool(); // 1 byte varint
	double bb_read_double(); // 64 bit float
	float bb_read_single(); //  32 bit float
	GUID bb_read_guid(); // length delimited
	std::string bb_read_string(); // length delimited
	TByteBuffer bb_read_bytes(); // length delimited

	void bb_read_skip(int aWiretype);
	void bb_read_skip_bytes(int aNumberOfBytes);
private:
	void bb_put_uint64(uint64_t aValue);
	void bb_put_bytes(const byte* aValue, int aValueSize);
	void bb_put_bytes(const char* aValue, int aValueSize);
	static int bb_var_int_length(uint64_t aValue);
public:
	static TByteBuffer join(TByteBuffer aBuffers[], int aNumberOfBuffers);
	static TByteBuffer join(const TByteBuffer &aBuffer1, const TByteBuffer &aBuffer2);

	static TByteBuffer bb_uint16(uint16_t aValue); // fixed 16 bit (cannot be tagged)

	static TByteBuffer bb_bool(bool aValue); // unsigned varint
	static TByteBuffer bb_byte(byte aValue); // unsigned varint
	static TByteBuffer bb_uint64(uint64_t aValue); // unsigned varint
	static TByteBuffer bb_uint32(uint32_t aValue); // unsigned varint
	static TByteBuffer bb_int64(int64_t aValue);// signed varint
	static TByteBuffer bb_int32(int32_t aValue);// unsigned varint
	static TByteBuffer bb_single(float aValue); // length delimited
	static TByteBuffer bb_double(double aValue); // length delimited
	static TByteBuffer bb_bytes(byte* aValue, int aValueSize); // length delimited
	static TByteBuffer bb_string(std::string aValue); // length delimited
	static TByteBuffer bb_guid(GUID aValue); // length delimited

	static TByteBuffer bb_tag_int32(uint32_t aTag, int32_t aValue);
	static TByteBuffer bb_tag_uint32(uint32_t aTag, uint32_t aValue);
	static TByteBuffer bb_tag_int64(uint32_t aTag, int64_t aValue);
	static TByteBuffer bb_tag_uint64(uint32_t aTag, uint64_t aValue);
	static TByteBuffer bb_tag_bool(uint32_t aTag, bool aValue);
	static TByteBuffer bb_tag_single(uint32_t aTag, float aValue);
	static TByteBuffer bb_tag_double(uint32_t aTag, double aValue);
	static TByteBuffer bb_tag_guid(uint32_t aTag, GUID aValue);
	static TByteBuffer bb_tag_string(uint32_t aTag, std::string aValue);
	static TByteBuffer bb_tag_bytes(uint32_t aTag, byte* aValue, int aValueSize);
};

enum TLogLevel {
	llRemark,
	llDump,
	llNormal,
	llStart,
	llFinish,
	llPush,
	llPop,
	llStamp,
	llSummary,
	llWarning,
	llError,
	llOK
};

// forward declaration for reference in TEventEntry
class TConnection;

class TStreamCacheEntry
{
public:
	TStreamCacheEntry() { fStream = 0; fStreamName = ""; };
	TStreamCacheEntry(std::ostream* aStream, const std::string &aStreamName) { fStream = aStream; fStreamName = aStreamName; };
public:
	std::ostream* fStream;
	std::string fStreamName;
};


// helper for GUID to support using it being used in map
inline bool operator < (const GUID & Left, const GUID & Right)
{
	// comparison logic goes here
	return memcmp(&Left, &Right, sizeof(GUID)) < 0;
}


[event_source(native)]
class TEventEntry
{
public:
	TEventEntry(TConnection* aConnection, int32_t aID, std::string aEventName);
public:
	const static int32_t TRC_INFINITE = INT_MAX;
	TEventEntry* subscribe();
	TEventEntry* publish();
	TEventEntry* unSubscribe();
	TEventEntry* unPublish();

	bool isEmpty() { return !(fIsSubscribed || fIsPublished); };
	bool existSubscribers() { return fSubscribers; };
	bool existPublishers() { return fPublishers; };
	int32_t getEventtID() { return fEventID; };
	std::string getEventName() { return fEventName; };
	void setEventName(std::string aEventName) { fEventName = aEventName; };
	bool isPublished() { return fIsPublished; };
	bool isSubscribed() { return fIsSubscribed; };
	
	void handleEvent(TByteBuffer &aBuffer);
	void signalEvent(TByteBuffer &aEventPayload);
	void signalEvent(TByteBuffer &aEventPayload1, TByteBuffer &aEventPayload2);
	void signalEvent(TByteBuffer &aEventPayload1, TByteBuffer &aEventPayload2, TByteBuffer &aEventPayload3);
	void signalStream(const std::string &aStreamName, std::istream &aStream);
	void signalChangeObject(int32_t aAction, int32_t aObjectID, const std::string &aAttribute);
	void signalIntString(int32_t aint, const std::string &aString);
	void signalString(const std::string &aString);
	void handleSubAndPub(uint32_t aCommand);
	void copyHandlersFrom(TEventEntry* aEventEntry);
private:
	void signalSubscribe();
	void signalPublish();
	void signalUnSubscribe();
	void signalUnPublish();
public: // events
	__event void onChangeObject(TEventEntry &aEventEntry, int32_t aAction, int32_t aObjectID, const std::string &aAttribute);
	__event void onIntString(TEventEntry &aEvent, int aInt, const std::string &aString);
	__event void onString(TEventEntry &aEvent, const std::string &aString);
	__event void onTag(TEventEntry &aEvent, uint32_t aFieldInfo, TByteBuffer &aPayload);
	__event void onStreamCreate(TEventEntry &aEvent, const std::string &aStreamName, std::ostream* &aStream);
	__event void onStreamEnd(TEventEntry &aEvent, const std::string &aStreamName, std::ostream* &aStream, bool aCancel);
	__event void onSubAndPub(TEventEntry &aEvent, uint32_t aCommand);
private:
	TConnection* fConnection; // ref only
	int32_t fEventID;
	std::string fEventName;
	bool fIsPublished;
	bool fIsSubscribed;
	bool fSubscribers;
	bool fPublishers;

	std::map<GUID, TStreamCacheEntry> fStreamCache;
};


[event_source(native)]
class TConnection
{
public:
	TConnection(std::string aModelName, int aModelID, std::string  aPrefix = imbDefaultPrefix);
	~TConnection(void);
public: // abstract/pure virtual methods
	virtual bool getConnected()=0;
	virtual void setConnected(bool aValue)=0;
	virtual void writePacket(TByteBuffer &aPacket, bool aCallCloseOnError = true) = 0;
protected: // abstract/pure virtual methods
	virtual int readBytes(byte aBuffer[], int aOffset, int aLimit) = 0;
protected: //  fields
	std::string fModelName;
	int fModelID;
	std::string fPrefix;
	
	GUID fUniqueClientID;
	GUID fHubID;
	
	std::vector<TEventEntry*> fLocalEventEntries;
	std::map<uint16_t, TEventEntry*> fRemoteEventEntries;
	
	CRITICAL_SECTION fEventEntryListLock;
	uintptr_t fReaderThread;
public: // getters/setters
	std::string getModelName() { return fModelName; };
	int getModelID() { return fModelID; };
	std::string getPrefix() { return fPrefix; };
	GUID getUniqueClient() { return fUniqueClientID; };
	GUID getHubID() { return fHubID; };

	std::string getMonitorEventName();
	std::string getPrivateEventName();

	std::vector<TEventEntry*>* getEventEntries();
public: // events
	__event void onDisconnect(TConnection &aConnection);
	__event void onException(TConnection &aConnection, const std::exception &aException);

protected:
	void waitForConnected(int aSpinCount = 20);
	void handleCommand(TByteBuffer &aBuffer);
	void signalConnectInfo(std::string aModelName, int aModelID);
	void readPackets();
	static unsigned int __stdcall executeReaderThread(void *aConnection);
	void startReaderThread();
public:
	void close(bool aSendCloseCmd = true);

	TEventEntry* subscribe(const std::string &aEventName, bool aUsePrefix = true);
	TEventEntry* publish(const std::string &aEventName, bool aUsePrefix = true);
	void unSubscribe(const std::string &aEventName, bool aUsePrefix = true);
	void unPublish(const std::string &aEventName, bool aUsePrefix = true);

	void writeCommand(const TByteBuffer &aPayload);
	void writeCommand(const TByteBuffer &aPayload1, const TByteBuffer &aPayload2);
	void writeCommand(const TByteBuffer &aPayload1, const TByteBuffer &aPayload2, const TByteBuffer &aPayload3);
	void writeCommand(const TByteBuffer &aPayload1, const TByteBuffer &aPayload2, const TByteBuffer &aPayload3, const TByteBuffer &aPayload4);
	void writeCommand(const TByteBuffer &aPayload1, const TByteBuffer &aPayload2, const TByteBuffer &aPayload3, const TByteBuffer &aPayload4, const TByteBuffer &aPayload5);
	void writeCommand(const TByteBuffer &aPayload1, const TByteBuffer &aPayload2, const TByteBuffer &aPayload3, const TByteBuffer &aPayload4, const TByteBuffer &aPayload5, const TByteBuffer &aPayload6);
};

class TSocketConnection : public TConnection
{
public:
	TSocketConnection(
		std::string aModelName, int aModelID = 0,
		std::string aPrefix = imbDefaultPrefix,
		std::string aRemoteHost = imbDefaultRemoteHost, int aRemotePort = imbDefaultSocketRemotePort);
	~TSocketConnection();
public: // override of abstract/pure virtual methods
	virtual bool getConnected();
	virtual void setConnected(bool aValue);
	virtual void writePacket(TByteBuffer &aPacket, bool aCallCloseOnError = true);
protected: // override of abstract/pure virtual methods
	virtual int readBytes(byte aBuffer[], int aOffset, int aLimit);
private:
	std::string fRemoteHost;
	int fRemotePort;
	SOCKET fSocket;
	ADDRESS_FAMILY fAddrFamily;
	CRITICAL_SECTION fWritePacketLock;
};

// todo: NOT implement yet
class TTLSConnection : public TConnection
{
public:
	TTLSConnection(
		std::string aCertFile, std::string aCertFilePassword, std::string aRootCertFile,
		std::string aModelName, int aModelID = 0,
		std::string aPrefix = imbDefaultPrefix,
		std::string aRemoteHost = imbDefaultRemoteHost, int aRemotePort = imbDefaultTLSRemotePort);
	~TTLSConnection();
public: // override of abstract/pure virtual methods
	virtual bool getConnected();
	virtual void setConnected(bool aValue);
	virtual void writePacket(TByteBuffer &aPacket, bool aCallCloseOnError = true);
protected: // override of abstract/pure virtual methods
	virtual int readBytes(byte aBuffer[], int aOffset, int aLimit);
private:
	std::string fRemoteHost;
	int fRemotePort;
	SOCKET fSocket;
	ADDRESS_FAMILY fAddrFamily;
	CRITICAL_SECTION fWritePacketLock;

};

#endif // IMB4_H_INCLUDED