#include "stdafx.h"

//#include <iostream> //  for debugging output

#include <list>
#include <string>
#include <sstream>
#include <process.h>
#include <cstdint>
#include "imb4.h"

#pragma comment(lib, "Rpcrt4.lib")
#pragma comment(lib, "Ws2_32.lib")

std::string toUpper(const std::string &s)
{
	std::string ret(s.size(), char());
	for (unsigned int i = 0; i < s.size(); ++i)
		ret[i] = ('a' <= s[i] && s[i] <= 'z') ? s[i] - ('a' - 'A') : s[i];
	return ret;
}
std::string intToString(int64_t i)
{
	std::stringstream ss;
	std::string s;
	ss << i;
	s = ss.str();
	return s;
}

// guid

bool isEmptyGUID(GUID &aGUID)
{
	return aGUID.Data1 == 0 && aGUID.Data2 == 0 && aGUID.Data3 == 0 && 
		aGUID.Data4[0] == 0 && aGUID.Data4[1] == 0 && aGUID.Data4[2] == 0 && aGUID.Data4[3] == 0 && 
		aGUID.Data4[4] == 0 && aGUID.Data4[5] == 0 && aGUID.Data4[6] == 0 && aGUID.Data4[7] == 0;
}

void emptyGUID(GUID &aGUID)
{
	ZeroMemory(&aGUID, sizeof(GUID));
}

std::string GUIDToHex(GUID &aGUID)
{
	std::ostringstream oss;
	oss << std::uppercase << std::hex << aGUID.Data1 << std::hex << aGUID.Data2 << std::hex << aGUID.Data3 << 
		std::hex << (int)aGUID.Data4[0] << std::hex << (int)aGUID.Data4[1] << std::hex << (int)aGUID.Data4[2] <<
		std::hex << (int)aGUID.Data4[3] << std::hex << (int)aGUID.Data4[4] << std::hex << (int)aGUID.Data4[5] <<
		std::hex << (int)aGUID.Data4[6] << std::hex << (int)aGUID.Data4[7];
	return oss.str();
}


// TByteBuffer

TByteBuffer::TByteBuffer(int aCapacity)
{
	fOffset = 0;
	fLimit = 0;
	fCapacity = aCapacity;
	if (aCapacity > 0)
		fBuffer = new byte[aCapacity];
	else
		fBuffer = 0;
}
TByteBuffer::TByteBuffer(const byte* aBuffer, int aLength)
{
	fOffset = 0;
	fLimit = aLength;
	fCapacity = aLength;
	if (aLength > 0)
	{
		fBuffer = new byte[aLength];
		if (aBuffer != 0)
			memcpy(fBuffer, aBuffer, aLength);
	}
	else
		fBuffer = 0;
}
TByteBuffer::TByteBuffer(const byte* aBuffer, int aOffset, int aLimit)
{
	// define empty buffer
	fOffset = 0;
	fLimit = aLimit-aOffset;
	fCapacity = fLimit;
	if (fCapacity > 0)
	{
		fBuffer = new byte[fCapacity];
		if (aBuffer != 0)
			memcpy(fBuffer, &aBuffer[aOffset], fCapacity);
	}
	else
		fBuffer = 0;
}
TByteBuffer::TByteBuffer(const TByteBuffer &aBuffer)
{
	//std::cout << "deep copy " << (void*)aBuffer.fBuffer << " (" << aBuffer.fCapacity << ")" << std::endl;
	// deep copy of TByteBuffer
	fOffset = aBuffer.fOffset;
	fLimit = aBuffer.fLimit;
	fCapacity = aBuffer.fCapacity;
	if (fCapacity > 0)
	{
		fBuffer = new byte[fCapacity];
		if (aBuffer.fBuffer != 0)
			memcpy(fBuffer, aBuffer.fBuffer, fCapacity);
	}
	else 
		fBuffer = 0;
}
TByteBuffer::~TByteBuffer()
{
	//std::cout << "destruct " << (void*)fBuffer << " (" << fCapacity << ")" << std::endl;
	if (fBuffer != 0)
	{
		delete[] fBuffer;
		fBuffer = 0;
	}
	fCapacity = 0;
	fLimit = 0;
	fOffset = 0;
}
TByteBuffer& TByteBuffer::operator = (const TByteBuffer &aBuffer)
{
	//std::cout << "assignment " << (void*)fBuffer << " << " << (void*)aBuffer.fBuffer << " (" << aBuffer.fCapacity << ")" << std::endl;
	// check for self-assignment
	if (this == &aBuffer)
		return *this;
	// check for existing buffer
	if (fBuffer != 0)
		delete[] fBuffer;
	// deep copy
	fOffset = aBuffer.fOffset;
	fLimit = aBuffer.fLimit;
	fCapacity = aBuffer.fCapacity;
	if (fCapacity > 0)
	{
		fBuffer = new byte[fCapacity];
		if (aBuffer.fBuffer != 0)
			memcpy(fBuffer, aBuffer.fBuffer, fCapacity);
	}
	else
		fBuffer = 0;
	return *this;
}
void TByteBuffer::setCapacity(int aNewCapacity)
{
	if (aNewCapacity != fCapacity)
	{
		if (aNewCapacity != 0)
		{
			// create new buffer
			byte* newBuffer = new byte[aNewCapacity];
			// copy existing data
			if (fBuffer != 0)
			{
				if (fCapacity > 0)
				{
					// there is data to copy
					if (aNewCapacity > fCapacity)
					{
						memcpy(newBuffer, fBuffer, fCapacity);
						ZeroMemory(&newBuffer[fCapacity], aNewCapacity - fCapacity);
					}
					else
						memcpy(newBuffer, fBuffer, aNewCapacity);
				}
				// data copied -> delete old buffer
				delete[] fBuffer;
			}
			// switch to new buffer
			fBuffer = newBuffer;
			fCapacity = aNewCapacity;
		}
		else
		{
			if (fBuffer != 0)
				delete[] fBuffer;
			fBuffer = 0;
			fCapacity = 0;
		}
		if (fOffset > fCapacity)
			fOffset = fCapacity;
		if (fLimit > fCapacity)
			fLimit = fCapacity;
	}
}
uint64_t TByteBuffer::bb_read_uint64() // unsigned varint
{
	int shiftLeft = 0;
	uint64_t b = 0;
	uint64_t res = 0;
	do
	{
		if (fOffset >= fCapacity)
			throw std::out_of_range("tried to read outside buffered data in TByteBuffer::bb_read_uint64");
		b = fBuffer[fOffset++];
		res |= (b & 0x7F) << shiftLeft;
		shiftLeft += 7;
	} while (b >= 128);
	return res;
}
int64_t TByteBuffer::bb_read_int64() // signed varint
{
	uint64_t ui64 = bb_read_uint64();
	// remove sign bit
	int64_t res = (int64_t)(ui64 >> 1);
	// adjust for negative
	if ((ui64 & 1) == 1)
		res = -(res + 1);
	return res;
}
uint32_t TByteBuffer::bb_read_uint32() // unsigned varint
{
	int shiftLeft = 0;
	uint32_t b = 0;
	uint32_t res = 0;
	do
	{
		if (fOffset >= fCapacity)
			throw std::out_of_range("tried to read outside buffered data in TByteBuffer::bb_read_uint32");
		b = fBuffer[fOffset++];
		res |= (b & 0x7F) << shiftLeft;
		shiftLeft += 7;
	} while (b >= 128);
	return res;
}
int32_t TByteBuffer::bb_read_int32() // signed varint
{
	uint32_t ui32 = bb_read_uint32();
	// remove sign bit
	int32_t res = (int32_t)(ui32 >> 1);
	// adjust for negative
	if ((ui32 & 1) == 1)
		res = -(res + 1);
	return res;
}
uint16_t TByteBuffer::bb_read_uint16() // fixed 16 bit (cannot be tagged)
{
	fOffset += 2;
	if (fOffset > fCapacity)
		throw std::out_of_range("tried to read outside buffered data in TByteBuffer::bb_read_uint16");
	return *(uint16_t *)&fBuffer[fOffset - 2];
}
bool TByteBuffer::bb_read_bool() // 1 byte varint
{
	if (fOffset >= fCapacity)
		throw std::out_of_range("tried to read outside buffered data in TByteBuffer::bb_read_bool");
	return fBuffer[fOffset++] != 0;
}
double TByteBuffer::bb_read_double() // 64 bit float
{
	fOffset += 8;
	if (fOffset > fCapacity)
		throw std::out_of_range("tried to read outside buffered data in TByteBuffer::bb_read_double");
	return *(double *)&fBuffer[fOffset - 8];
}
float TByteBuffer::bb_read_single() //  32 bit float
{
	fOffset += 4;
	if (fOffset > fCapacity)
		throw std::out_of_range("tried to read outside buffered data in TByteBuffer::bb_read_single");
	return *(float *)&fBuffer[fOffset - 4];
}
GUID TByteBuffer::bb_read_guid() // length delimited
{
	uint32_t len = bb_read_uint32();
	fOffset += len;
	if (fOffset > fCapacity)
		throw std::out_of_range("tried to read outside buffered data in TByteBuffer::bb_read_guid");
	return *(GUID *)&fBuffer[fOffset - len];
}
std::string TByteBuffer::bb_read_string() // length delimited
{
	uint32_t len = bb_read_uint32();
	fOffset += len;
	if (fOffset > fCapacity)
		throw std::out_of_range("tried to read outside buffered data in TByteBuffer::bb_read_string");
	return std::string((char *)&fBuffer[fOffset - len], len);
}
TByteBuffer TByteBuffer::bb_read_bytes() // length delimited
{
	uint32_t len = bb_read_uint32();
	fOffset += len;
	if (fOffset > fCapacity)
		throw std::out_of_range("tried to read outside buffered data in TByteBuffer::bb_read_bytes");
	TByteBuffer bb(fBuffer, fOffset - len, fOffset);
	return bb;
}
void TByteBuffer::bb_read_skip(int aWiretype)
{
	switch (aWiretype)
	{
	case wt32Bit:
		fOffset += 4;
		break;
	case wt64Bit:
		fOffset += 8;
		break;
	case wtLengthDelimited:
		fOffset += bb_read_uint32();
		break;
	case wtVarInt:
		bb_read_uint64();
		break;
	default:
		throw std::exception("invalid wiretype of in TByteBuffer::bb_read_skip");
	}
	if (fOffset > fCapacity)
		throw std::out_of_range("skipped outside buffered data in TByteBuffer::bb_read_skip");
}
void TByteBuffer::bb_read_skip_bytes(int aNumberOfBytes)
{
	fOffset += aNumberOfBytes;
	if (fOffset>fCapacity)
		throw std::out_of_range("skipped outside buffered data in TByteBuffer::bb_read_skip_bytes");
}
int TByteBuffer::bb_var_int_length(uint64_t aValue)
{
	// encode in blocks of 7 bits (high order bit of byte is signal that more bytes are to follow
	// encode lower numbers directly for speed
	if (aValue < 128)
		return 1;
	else if (aValue < 128 * 128)
		return 2;
	else if (aValue < 128 * 128 * 128)
		return 3;
	else
	{
		// 4 bytes or more: change to dynamic size detection
		int res = 4;
		aValue >>= 7 * 4;
		while (aValue > 0)
		{
			aValue >>= 7;
			res++;
		}
		return res;
	}
}
TByteBuffer TByteBuffer::join(TByteBuffer aBuffers[], int aNumberOfBuffers)
{
	int capacity = 0;
	for (int b = 0; b < aNumberOfBuffers; b++)
		capacity += aBuffers[b].remaining();
	TByteBuffer bb(capacity);
	for (int b = 0; b < aNumberOfBuffers; b++)
	{
		memcpy(&bb.fBuffer[bb.fLimit], &aBuffers[b].fBuffer[aBuffers[b].fOffset], aBuffers[b].remaining());
		bb.fLimit += aBuffers[b].remaining();
	}
	return bb;
}
TByteBuffer TByteBuffer::join(const TByteBuffer &aBuffer1, const TByteBuffer &aBuffer2)
{
	int rem1 = aBuffer1.fLimit - aBuffer1.fOffset;
	int rem2 = aBuffer2.fLimit - aBuffer2.fOffset;
	int capacity = rem1 + rem2;
	TByteBuffer bb(capacity);
	memcpy(&bb.fBuffer[bb.fLimit], &aBuffer1.fBuffer[aBuffer1.fOffset], rem1);
	bb.fLimit += rem1;
	memcpy(&bb.fBuffer[bb.fLimit], &aBuffer2.fBuffer[aBuffer2.fOffset], rem2);
	bb.fLimit += rem2;
	return bb;
}
void TByteBuffer::bb_put_uint64(uint64_t aValue)
{
	while (aValue >= 128)
	{
		if (fLimit>=fCapacity)
			throw std::out_of_range("writing outside buffered data in TByteBuffer::bb_put_uint64");
		fBuffer[fLimit++] = (byte)((aValue & 0x7F) | 0x80); // msb: signal more bytes are to follow
		aValue >>= 7;
	}
	fBuffer[fLimit++] = (byte)aValue; // aValue<128 (msb already 0)
}
void TByteBuffer::bb_put_bytes(const byte* aValue, int aValueSize)
{
	if (fLimit+aValueSize > fCapacity)
		throw std::out_of_range("writing outside buffered data in TByteBuffer::bb_put_bytes(byte)");
	memcpy(&fBuffer[fLimit], aValue, aValueSize);
	fLimit += aValueSize;
}
void TByteBuffer::bb_put_bytes(const char* aValue, int aValueSize)
{
	if (fLimit + aValueSize > fCapacity)
		throw std::out_of_range("writing outside buffered data in TByteBuffer::bb_put_bytes(char)");
	memcpy(&fBuffer[fLimit], aValue, aValueSize);
	fLimit += aValueSize;
}
TByteBuffer TByteBuffer::bb_bool(bool aValue) // unsigned varint
{
	TByteBuffer bb(1);
	bb.fBuffer[0] = (byte)aValue;
	bb.fLimit += 1;
	return bb;
}
TByteBuffer TByteBuffer::bb_byte(byte aValue) // unsigned varint
{
	TByteBuffer bb(1);
	bb.fBuffer[0] = aValue;
	bb.fLimit += 1;
	return bb;
}
TByteBuffer TByteBuffer::bb_uint16(uint16_t aValue) // fixed 16 bit (cannot be tagged)
{
	TByteBuffer bb(2);
	memcpy(bb.fBuffer, &aValue, 2);
	bb.fLimit += 2;
	return bb;
}
TByteBuffer TByteBuffer::bb_uint64(uint64_t aValue) // unsigned varint
{
	int len = TByteBuffer::bb_var_int_length(aValue);
	TByteBuffer bb(len);
	bb.bb_put_uint64(aValue);
	return bb;
}
TByteBuffer TByteBuffer::bb_uint32(uint32_t aValue) // unsigned varint
{
	int len = TByteBuffer::bb_var_int_length(aValue);
	TByteBuffer bb(len);
	while (aValue >= 128)
	{
		bb.fBuffer[bb.fLimit++] = (byte)((aValue & 0x7F) | 0x80); // msb: signal more bytes are to follow
		aValue >>= 7;
	}
	bb.fBuffer[bb.fLimit++] = (byte)aValue; // aValue<128 (msb already 0)
	return bb;
}
TByteBuffer TByteBuffer::bb_int64(int64_t aValue) // signed varint
{
	if (aValue < 0)
		return bb_uint64(((uint64_t)(-(aValue + 1)) << 1) | 1);
	else
		return bb_uint64((uint64_t)aValue << 1);
}
TByteBuffer TByteBuffer::bb_int32(int32_t aValue) // unsigned varint
{
	if (aValue < 0)
		return bb_uint32(((uint32_t)(-(aValue + 1)) << 1) | 1);
	else
		return bb_uint32((uint32_t)aValue << 1);
}
TByteBuffer TByteBuffer::bb_single(float aValue) // length delimited
{
	TByteBuffer bb(4);
	memcpy(bb.fBuffer, &aValue, 4);
	bb.fLimit += 4;
	return bb;
}
TByteBuffer TByteBuffer::bb_double(double aValue) // length delimited
{
	TByteBuffer bb(8);
	memcpy(bb.fBuffer, &aValue, 8);
	bb.fLimit += 8;
	return bb;
}
TByteBuffer TByteBuffer::bb_bytes(byte* aValue, int aValueSize) // length delimited
{
	TByteBuffer bb(TByteBuffer::bb_var_int_length(aValueSize) + aValueSize);
	bb.bb_put_uint64(aValueSize);
	bb.bb_put_bytes(aValue, aValueSize);
	return bb;
}
TByteBuffer TByteBuffer::bb_string(std::string aValue) // length delimited
{
	int valueSize = aValue.size();
	TByteBuffer bb(TByteBuffer::bb_var_int_length(valueSize) + valueSize);
	bb.bb_put_uint64(valueSize);
	bb.bb_put_bytes(aValue.data(), valueSize);
	return bb;
}
TByteBuffer TByteBuffer::bb_guid(GUID aValue) // length delimited
{
	int valueSize = sizeof(GUID);
	TByteBuffer bb(TByteBuffer::bb_var_int_length(valueSize) + valueSize);
	bb.bb_put_uint64(valueSize);
	bb.bb_put_bytes((byte*)&aValue, valueSize);
	return bb;
}
TByteBuffer TByteBuffer::bb_tag_int32(uint32_t aTag, int32_t aValue)
{
	return TByteBuffer::join(
		TByteBuffer::bb_uint32((aTag << 3 ) | wtVarInt),
		TByteBuffer::bb_int32(aValue));
}
TByteBuffer TByteBuffer::bb_tag_uint32(uint32_t aTag, uint32_t aValue)
{
	return TByteBuffer::join(
		TByteBuffer::bb_uint32((aTag << 3) | wtVarInt),
		TByteBuffer::bb_uint32(aValue));
}
TByteBuffer TByteBuffer::bb_tag_int64(uint32_t aTag, int64_t aValue)
{
	return TByteBuffer::join(
		TByteBuffer::bb_uint32((aTag << 3) | wtVarInt),
		TByteBuffer::bb_int64(aValue));
}
TByteBuffer TByteBuffer::bb_tag_uint64(uint32_t aTag, uint64_t aValue)
{
	return TByteBuffer::join(
		TByteBuffer::bb_uint32((aTag << 3) | wtVarInt),
		TByteBuffer::bb_uint64(aValue));
}
TByteBuffer TByteBuffer::bb_tag_bool(uint32_t aTag, bool aValue)
{
	return TByteBuffer::join(
		TByteBuffer::bb_uint32((aTag << 3) | wtVarInt),
		TByteBuffer::bb_bool(aValue));
}
TByteBuffer TByteBuffer::bb_tag_single(uint32_t aTag, float aValue)
{
	return TByteBuffer::join(
		TByteBuffer::bb_uint32((aTag << 3) | wt32Bit),
		TByteBuffer::bb_single(aValue));
}
TByteBuffer TByteBuffer::bb_tag_double(uint32_t aTag, double aValue)
{
	return TByteBuffer::join(
		TByteBuffer::bb_uint32((aTag << 3) | wt64Bit),
		TByteBuffer::bb_double(aValue));
}
TByteBuffer TByteBuffer::bb_tag_guid(uint32_t aTag, GUID aValue)
{
	return TByteBuffer::join(
		TByteBuffer::bb_uint32((aTag << 3) | wtLengthDelimited),
		TByteBuffer::bb_guid(aValue));
}
TByteBuffer TByteBuffer::bb_tag_string(uint32_t aTag, std::string aValue)
{
	return TByteBuffer::join(
		TByteBuffer::bb_uint32((aTag << 3) | wtLengthDelimited),
		TByteBuffer::bb_string(aValue));
}
TByteBuffer TByteBuffer::bb_tag_bytes(uint32_t aTag, byte* aValue, int aValueSize)
{
	return TByteBuffer::join(
		TByteBuffer::bb_uint32((aTag << 3) | wtLengthDelimited),
		TByteBuffer::bb_bytes(aValue, aValueSize));
}
void TByteBuffer::shiftLeftOneByte(byte aRightByte)
{
	int rem = remaining();
	if ( rem > 0)
	{
		if (rem > 1)
			memcpy(&fBuffer[fOffset], &fBuffer[fOffset + 1], rem - 1);
		fBuffer[fLimit - 1] = aRightByte;
	}
}


// TEventEntry

TEventEntry::TEventEntry(TConnection *aConnection, int32_t aEventID, std::string aEventName)
{
	fConnection = aConnection;
	fEventID = aEventID;
	fEventName = aEventName;
	fIsPublished = false;
	fIsSubscribed = false;
	fSubscribers = false;
	fPublishers = false;
}
void TEventEntry::signalSubscribe()
{
	fConnection->writeCommand(TByteBuffer::join(
		TByteBuffer::bb_tag_string(icehEventName, fEventName),
		TByteBuffer::bb_tag_uint32(icehSubscribe, fEventID)));
}
void TEventEntry::signalPublish()
{
	fConnection->writeCommand(TByteBuffer::join(
		TByteBuffer::bb_tag_string(icehEventName, fEventName),
		TByteBuffer::bb_tag_uint32(icehPublish, fEventID)));
}
void TEventEntry::signalUnSubscribe()
{
	fConnection->writeCommand(TByteBuffer::bb_tag_uint32(icehUnsubscribe, fEventID));
}
void TEventEntry::signalUnPublish()
{
	fConnection->writeCommand(TByteBuffer::bb_tag_uint32(icehUnpublish, fEventID));
}
TEventEntry* TEventEntry::subscribe()
{
	if (!fIsSubscribed)
	{
		signalSubscribe();
		fIsSubscribed = true;
	}
	return this;
}
TEventEntry* TEventEntry::publish()
{
	if (fIsPublished)
	{
		signalUnPublish();
		fIsPublished = false;
	}
	return this;
}
TEventEntry* TEventEntry::unSubscribe()
{
	if (fIsSubscribed)
	{
		signalUnSubscribe();
		fIsSubscribed = false;
	}
	return this;
}
TEventEntry* TEventEntry::unPublish()
{
	if (fIsPublished)
	{
		signalUnPublish();
		fIsPublished = false;
	}
	return this;
}
void TEventEntry::handleEvent(TByteBuffer &aBuffer)
{
	std::string eventString = "";
	int action = -1;
	std::string attribute = "";
	GUID streamID;
	TStreamCacheEntry sce;
	std::string streamName;
	TByteBuffer block;
	int eventInt;
	int objectID;
	std::ostream* stream;
	bool cancel;
	int storedOffset;
	while (aBuffer.remaining() > 0)
	{
		uint32_t fieldInfo = aBuffer.bb_read_uint32();
		switch (fieldInfo)
		{
			// int string
		case (icehIntString << 3) | wtVarInt:
			eventInt = aBuffer.bb_read_int32();
			__raise onIntString(*this, eventInt, eventString);
			break;
		case (icehIntStringPayload << 3) | wtLengthDelimited:
			eventString = aBuffer.bb_read_string();
			break;
			// string
		case (icehString << 3) | wtLengthDelimited:
			eventString = aBuffer.bb_read_string();
			__raise onString(*this, eventString);
			break;
			// change object
		case (icehChangeObjectAction << 3) | wtVarInt:
			action = aBuffer.bb_read_int32();
			break;
		case (icehChangeObjectAttribute << 3) | wtLengthDelimited:
			attribute = aBuffer.bb_read_string();
			break;
		case (icehChangeObject << 3) | wtVarInt:
			objectID = aBuffer.bb_read_int32();
			__raise onChangeObject(*this, action, objectID, attribute);
			break;
			// streams
		case (icehStreamHeader << 3) | wtLengthDelimited:
			streamName = aBuffer.bb_read_string();
			__raise onStreamCreate(*this, streamName, stream);
			if (stream)
				fStreamCache[streamID] = TStreamCacheEntry(stream, streamName); // todo: possible memory leak when stream header is passed twice for same id
			break;
		case (icehStreamBody << 3) | wtLengthDelimited:
			block = aBuffer.bb_read_bytes();
			try
			{
				sce = fStreamCache.at(streamID);
				sce.fStream->write((char*)block.getBuffer(), block.remaining());
			}
			catch (std::out_of_range*) {}
			break;
		case (icehStreamEnd << 3) | wtVarInt:
			cancel = aBuffer.bb_read_bool();
			try
			{
				sce = fStreamCache.at(streamID);
				__raise onStreamEnd(*this, sce.fStreamName, sce.fStream, cancel);
				if (sce.fStream != 0)
				{
					delete sce.fStream;
					sce.fStream = 0;
				}
				fStreamCache.erase(streamID);
			}
			catch (std::out_of_range*) {}
			break;
		case (icehStreamID << 3) | wtLengthDelimited:
			streamID = aBuffer.bb_read_guid();
			break;
		default:
			// save offset and skip it after calling onTag because we do not know if a handler is hooked
			storedOffset = aBuffer.getOffset();
			__raise onTag(*this, fieldInfo, aBuffer);
			aBuffer.setOffset(storedOffset);
			aBuffer.bb_read_skip((int)(fieldInfo & 7));
			break;
		}
	}
}
void TEventEntry::signalEvent(TByteBuffer &aEventPayload)
{
	publish();
	TByteBuffer commands[4] = {
		TByteBuffer::bb_byte(imbMagic),
		0, // place holder, will be defined after next 2 entries are known
		TByteBuffer::bb_uint16(fEventID),
		aEventPayload
	};
	// reset 'size' entry
	commands[1] = TByteBuffer::bb_int64(commands[2].remaining() + commands[3].remaining());
	fConnection->writePacket(TByteBuffer::join(commands, 4));
}
void TEventEntry::signalEvent(TByteBuffer &aEventPayload1, TByteBuffer &aEventPayload2)
{
	publish();
	TByteBuffer commands[5] = {
		TByteBuffer::bb_byte(imbMagic),
		0, // place holder, will be defined after next 2 entries are known
		TByteBuffer::bb_uint16(fEventID),
		aEventPayload1,
		aEventPayload2
	};
	// reset 'size' entry
	commands[1] = TByteBuffer::bb_int64(commands[2].remaining() + commands[3].remaining() + commands[4].remaining());
	fConnection->writePacket(TByteBuffer::join(commands, 5));
}
void TEventEntry::signalEvent(TByteBuffer &aEventPayload1, TByteBuffer &aEventPayload2, TByteBuffer &aEventPayload3)
{
	publish();
	TByteBuffer commands[6] = {
		TByteBuffer::bb_byte(imbMagic),
		0, // place holder, will be defined after next 2 entries are known
		TByteBuffer::bb_uint16(fEventID),
		aEventPayload1,
		aEventPayload2,
		aEventPayload3
	};
	// reset 'size' entry
	commands[1] = TByteBuffer::bb_int64(commands[2].remaining() + commands[3].remaining() + commands[4].remaining() + commands[5].remaining());
	fConnection->writePacket(TByteBuffer::join(commands, 6));
}
void TEventEntry::signalStream(const std::string &aStreamName, std::istream &aStream)
{
	GUID streamID;
	UuidCreate(&streamID);
	TByteBuffer bufferStreamID = TByteBuffer::bb_tag_guid(icehStreamID, streamID);
	// header
	signalEvent(bufferStreamID, TByteBuffer::bb_tag_string(icehStreamHeader, aStreamName));
	// body
	char buffer[imbMaxStreamBodyBuffer];
	aStream.read(buffer, imbMaxStreamBodyBuffer);
	int readSize = (int)aStream.gcount();
	while (readSize > 0)
	{
		signalEvent(bufferStreamID, TByteBuffer::bb_tag_bytes(icehStreamBody, (byte*)buffer, readSize));
		aStream.read(buffer, imbMaxStreamBodyBuffer);
		readSize = (int)aStream.gcount();
	}
	// end
	signalEvent(bufferStreamID, TByteBuffer::bb_tag_bool(icehStreamEnd, readSize != 0));
}
void TEventEntry::signalChangeObject(int32_t aAction, int32_t aObjectID, const std::string &aAttribute)
{
	signalEvent(
		TByteBuffer::bb_tag_int32(icehChangeObjectAction, aAction),
		TByteBuffer::bb_tag_string(icehChangeObjectAttribute, aAttribute),
		TByteBuffer::bb_tag_int32(icehChangeObject, aObjectID));
}
void TEventEntry::signalString(const std::string &aString)
{
	signalEvent(TByteBuffer::bb_tag_string(icehString, aString));

}
void TEventEntry::signalIntString(int aInt, const std::string &aString)
{
	signalEvent(
		TByteBuffer::bb_tag_string(icehIntStringPayload, aString),
		TByteBuffer::bb_tag_int32(icehIntString, aInt));

}
void TEventEntry::handleSubAndPub(uint32_t aCommand)
{
	switch (aCommand)
	{
	case icehSubscribe:
		fSubscribers = true;
		break;
	case icehPublish:
		fPublishers = true;
		break;
	case icehUnsubscribe:
		fSubscribers = false;
		break;
	case icehUnpublish:
		fPublishers = false;
		break;
	}
	__raise onSubAndPub(*this, aCommand);
}


// class TConnection

TConnection::TConnection(std::string aModelName, int aModelID,  std::string  aPrefix)
{
	fModelName = aModelName;
	fModelID = aModelID;
	fPrefix = aPrefix;
	emptyGUID(fUniqueClientID);
	emptyGUID(fHubID);
	InitializeCriticalSection(&fEventEntryListLock);
	fReaderThread = NULL;
}
TConnection::~TConnection(void)
{
	DeleteCriticalSection(&fEventEntryListLock);
}
std::string TConnection::getMonitorEventName()
{		
	if (isEmptyGUID(fHubID))
		return "";
	else
		return "Hubs." + GUIDToHex(fHubID) + ".Monitor";
}
std::string TConnection::getPrivateEventName()
{
	if (isEmptyGUID(fHubID))
		return "";
	else
		return "Clients." + GUIDToHex(fUniqueClientID) + ".Private";
}
void TConnection::waitForConnected(int aSpinCount)
{
	while (isEmptyGUID(fUniqueClientID) && aSpinCount != 0)
	{
		Sleep(100);
		aSpinCount--;
	}
}
void TConnection::handleCommand(TByteBuffer &aBuffer)
{
	TEventEntry* eventEntry = 0;
	std::string eventName = "";
	uint32_t localEventID = imbInvalidEventID;
	uint32_t remoteEventID = 0;
	// process tags
	while (aBuffer.remaining() > 0)
	{
		uint32_t fieldInfo = aBuffer.bb_read_uint32();
		switch (fieldInfo)
		{
		case (icehSubscribe << 3) | wtVarInt:
			remoteEventID = aBuffer.bb_read_uint32();
			try
			{
				eventEntry = fRemoteEventEntries.at(remoteEventID);
				if (eventEntry)
					eventEntry->handleSubAndPub(icehSubscribe);
			}
			catch (std::out_of_range) {}
			break;
		case (icehPublish << 3) | wtVarInt:
			remoteEventID = aBuffer.bb_read_uint32();
			try
			{
				eventEntry = fRemoteEventEntries.at(remoteEventID);
				if (eventEntry)
					eventEntry->handleSubAndPub(icehPublish);
			}
			catch (std::out_of_range) {}
			break;
		case (icehUnsubscribe << 3) | wtVarInt:
			eventName = "";
			remoteEventID = aBuffer.bb_read_uint32();
			try
			{
				eventEntry = fRemoteEventEntries.at(remoteEventID);
				if (eventEntry)
					eventEntry->handleSubAndPub(icehUnsubscribe);
			}
			catch (std::out_of_range) {}
			break;
		case (icehUnpublish << 3) | wtVarInt:
			eventName = "";
			remoteEventID = aBuffer.bb_read_uint32();
			try
			{
				eventEntry = fRemoteEventEntries.at(remoteEventID);
				if (eventEntry)
					eventEntry->handleSubAndPub(icehUnpublish);
			}
			catch (std::out_of_range) {}
			break;
		case (icehEventName << 3) | wtLengthDelimited:
			eventName = aBuffer.bb_read_string();
			break;
		case (icehEventID << 3) | wtVarInt:
			localEventID = aBuffer.bb_read_uint32();
			break;
		case (icehSetEventIDTranslation << 3) | wtVarInt:
			remoteEventID = aBuffer.bb_read_uint32();
			EnterCriticalSection(&fEventEntryListLock);
			if (localEventID < fLocalEventEntries.size())
				//if (!fRemoteEventEntries.insert(std::pair<int, TEventEntry*>(remoteEventID, fLocalEventEntries[(int)localEventID])))
				fRemoteEventEntries[remoteEventID] = fLocalEventEntries[(int)localEventID];
			else
				fRemoteEventEntries[remoteEventID] = 0;
			LeaveCriticalSection(&fEventEntryListLock);
			break;
		case (icehClose << 3) | wtVarInt:
			aBuffer.bb_read_bool();
			close(false);
			break;
		case (icehHubID << 3) | wtLengthDelimited:
			fHubID = aBuffer.bb_read_guid();
			break;
		case (icehUniqueClientID << 3) | wtLengthDelimited:
			fUniqueClientID = aBuffer.bb_read_guid();
			break;
		default:
			aBuffer.bb_read_skip((int)fieldInfo & 7);
			break;
		}
	}
}
void TConnection::close(bool aSendCloseCmd)
{
	if (getConnected())
	{
		__raise onDisconnect(*this);
		if (aSendCloseCmd)
			writeCommand(TByteBuffer::bb_tag_bool(icehClose, false));
		setConnected(false);
	}
}
void TConnection::signalConnectInfo(std::string aModelName, int aModelID)
{
	writeCommand(
		TByteBuffer::bb_tag_string(icehModelName, aModelName),
		TByteBuffer::bb_tag_int32(icehModelID, aModelID),
		TByteBuffer::bb_tag_uint32(icehState, icsClient),
		// bb_tag_bool(icehReconnectable, False),
		// bb_tag_string(icehEventNameFilter, ''),
		TByteBuffer::bb_tag_guid(icehUniqueClientID, fUniqueClientID)); // trigger
}
void TConnection::readPackets() // event reader thread loop
{
	// read from socket and process
	TByteBuffer packet(imbMinimumPacketSize);
	do
	{
		try
		{
			packet.setOffset(0);
			packet.setLimit(imbMinimumPacketSize);
			int receivedBytes = readBytes(packet.getBuffer(), packet.getOffset(), packet.getLimit());
			if (receivedBytes == imbMinimumPacketSize)
			{
				while (packet.getFirstByte() != imbMagic)
				{
					byte oneByte[1];
					if (readBytes(oneByte, 0, 1) == 1)
						packet.shiftLeftOneByte(oneByte[0]);
					else
					{
						close(false);
						break;
					}
				}
				packet.bb_read_skip_bytes(1);
				// we have magic ate the first byte
				int size = (int)packet.bb_read_int64();
				int extraBytesOffset = packet.getLimit();
				packet.setLimit(packet.getOffset() + abs(size));
				int extraBytesNeeded = packet.getLimit() - extraBytesOffset;
				if (extraBytesNeeded > 0)
				{
					receivedBytes = readBytes(packet.getBuffer(), extraBytesOffset, packet.getLimit());
					if (receivedBytes != extraBytesNeeded)
					{
						close(false);
						break;
					}
				}
				if (size > 0)
				{
					// event
					uint16_t eventID = packet.bb_read_uint16();
					try
					{
						fRemoteEventEntries.at(eventID)->handleEvent(packet);
					}
					catch (std::out_of_range){}
				}
				else
				{
					// command
					handleCommand(packet);
				}
			}
			else
			{
				close(false);
				break;
			}
		}
		catch (std::exception e)
		{
			if (getConnected())
				__raise onException(*this, e);
		}
	} while (getConnected());
}
unsigned int __stdcall TConnection::executeReaderThread(void *aConnection)
{
	// convert from static to normal method
	((TConnection *)aConnection)->readPackets();
	return 0;
}
void TConnection::startReaderThread()
{
	fReaderThread = _beginthreadex(NULL, 0, &executeReaderThread, this, 0, NULL);
}
TEventEntry* TConnection::subscribe(const std::string &aEventName, bool aUsePrefix)
{
	std::string longEventName = aEventName;
	if (aUsePrefix)
		longEventName = fPrefix + "." + longEventName;
	std::string upperLongEventName = toUpper(longEventName);
	EnterCriticalSection(&fEventEntryListLock);
	for (uint32_t i = 0; i < fLocalEventEntries.size(); i++)
	{
		if (toUpper(fLocalEventEntries[i]->getEventName()).compare(upperLongEventName) == 0)
		{
			LeaveCriticalSection(&fEventEntryListLock);
			return fLocalEventEntries[i]->subscribe();
		}
	}
	// if we come here we have not found an existing event entry
	TEventEntry* newEventEntry = new TEventEntry(this, (uint32_t)fLocalEventEntries.size(), longEventName);
	fLocalEventEntries.push_back(newEventEntry);
	LeaveCriticalSection(&fEventEntryListLock);
	return newEventEntry->subscribe();
}
TEventEntry* TConnection::publish(const std::string &aEventName, bool aUsePrefix)
{
	std::string longEventName = aEventName;
	if (aUsePrefix)
		longEventName = fPrefix + "." + longEventName;
	std::string upperLongEventName = toUpper(longEventName);
	EnterCriticalSection(&fEventEntryListLock);
	for (uint32_t i = 0; i < fLocalEventEntries.size(); i++)
	{
		if (toUpper(fLocalEventEntries[i]->getEventName()).compare(upperLongEventName) == 0)
		{
			LeaveCriticalSection(&fEventEntryListLock);
			return fLocalEventEntries[i]->publish();
		}
	}
	// if we come here we have not found an existing event entry
	TEventEntry* newEventEntry = new TEventEntry(this, (uint32_t)fLocalEventEntries.size(), longEventName);
	fLocalEventEntries.at(newEventEntry->getEventtID()) = newEventEntry;
	LeaveCriticalSection(&fEventEntryListLock);
	return newEventEntry->publish();
}
void TConnection::writeCommand(const TByteBuffer &aPayload)
{
	TByteBuffer commands[3] = {
		TByteBuffer::bb_byte(imbMagic),
		0, // place holder, will be defined after next 2 entries are known
		aPayload
	};
	// reset 'size' entry
	commands[1] = TByteBuffer::bb_int64(-(commands[2].remaining()));
	writePacket(TByteBuffer::join(commands, 3));
}
void TConnection::writeCommand(const TByteBuffer &aPayload1, const TByteBuffer &aPayload2)
{
	TByteBuffer commands[4] = {
		TByteBuffer::bb_byte(imbMagic),
		0, // place holder, will be defined after next 2 entries are known
		aPayload1,
		aPayload2
	};
	// reset 'size' entry
	commands[1] = TByteBuffer::bb_int64(-(commands[2].remaining() + commands[3].remaining()));
	writePacket(TByteBuffer::join(commands, 4));
}
void TConnection::writeCommand(const TByteBuffer &aPayload1, const TByteBuffer &aPayload2, const TByteBuffer &aPayload3)
{
	TByteBuffer commands[5] = {
		TByteBuffer::bb_byte(imbMagic),
		0, // place holder, will be defined after next 2 entries are known
		aPayload1,
		aPayload2,
		aPayload3,
	};
	// reset 'size' entry
	commands[1] = TByteBuffer::bb_int64(-(commands[2].remaining() + commands[3].remaining() + commands[4].remaining()));
	writePacket(TByteBuffer::join(commands, 5));
}
void TConnection::writeCommand(const TByteBuffer &aPayload1, const TByteBuffer &aPayload2, const TByteBuffer &aPayload3, const TByteBuffer &aPayload4)
{
	TByteBuffer commands[6] = {
		TByteBuffer::bb_byte(imbMagic),
		0, // place holder, will be defined after next 2 entries are known
		aPayload1,
		aPayload2,
		aPayload3,
		aPayload4
	};
	// reset 'size' entry
	commands[1] = TByteBuffer::bb_int64(-(commands[2].remaining() + commands[3].remaining() + commands[4].remaining() + commands[5].remaining()));
	writePacket(TByteBuffer::join(commands, 6));
}
void TConnection::writeCommand(const TByteBuffer &aPayload1, const TByteBuffer &aPayload2, const TByteBuffer &aPayload3, const TByteBuffer &aPayload4, const TByteBuffer &aPayload5)
{
	TByteBuffer commands[7] = {
		TByteBuffer::bb_byte(imbMagic),
		0, // place holder, will be defined after next 2 entries are known
		aPayload1,
		aPayload2,
		aPayload3,
		aPayload4,
		aPayload5
	};
	// reset 'size' entry
	commands[1] = TByteBuffer::bb_int64(-(commands[2].remaining() + commands[3].remaining() + commands[4].remaining() + commands[5].remaining() + commands[6].remaining()));
	writePacket(TByteBuffer::join(commands, 7));
}
void TConnection::writeCommand(const TByteBuffer &aPayload1, const TByteBuffer &aPayload2, const TByteBuffer &aPayload3, const TByteBuffer &aPayload4, const TByteBuffer &aPayload5, const TByteBuffer &aPayload6)
{
	TByteBuffer commands[8] = {
		TByteBuffer::bb_byte(imbMagic),
		0, // place holder, will be defined after next 2 entries are known
		aPayload1,
		aPayload2,
		aPayload3,
		aPayload4,
		aPayload5,
		aPayload6
	};
	// reset 'size' entry
	commands[1] = TByteBuffer::bb_int64(-(commands[2].remaining() + commands[3].remaining() + commands[4].remaining() + commands[5].remaining() + commands[6].remaining() + commands[7].remaining()));
	writePacket(TByteBuffer::join(commands, 8));
}


// initialize en finalize winsock

class WinsockInitialization
{
public:
	WinsockInitialization(void)
	{
		// initialize winsock
		WSADATA wsaData;
		WSAStartup(MAKEWORD(2, 2), &wsaData);
	};
	~WinsockInitialization(void)
	{
		// clean up winsock
		WSACleanup();
	};
};

WinsockInitialization winsockInitialization;


// TSocketConnection

TSocketConnection::TSocketConnection(
	std::string aModelName, int aModelID, std::string aPrefix,
	std::string aRemoteHost, int aRemotePort) : TConnection(aModelName, aModelID, aPrefix)
{
	//locks
	InitializeCriticalSection(&fWritePacketLock);
	// fields
	fSocket = INVALID_SOCKET;
	//fLogEvent = NULL;
	
	fRemoteHost = aRemoteHost;
	fRemotePort = aRemotePort;
	setConnected(true);
}
TSocketConnection::~TSocketConnection()
{
	setConnected(false);
	// cleanup locks
	DeleteCriticalSection(&fWritePacketLock);
}
bool TSocketConnection::getConnected()
{
	return fSocket != INVALID_SOCKET;
}
void TSocketConnection::setConnected(bool aValue)
{
	if (aValue)
	{
		if (!getConnected())
		{
			std::string remotePortStr = intToString(fRemotePort);
			PADDRINFOA aistart = NULL;
			int res = getaddrinfo(fRemoteHost.c_str(), remotePortStr.c_str(), NULL, &aistart);
			if (res == 0)
			{
				// loop through addresses returned by getaddrinfo
				PADDRINFOA ai = aistart;
				do
				{
					fAddrFamily = ai->ai_family;
					fSocket = socket(fAddrFamily, SOCK_STREAM, IPPROTO_TCP);
					if (fSocket != INVALID_SOCKET)
					{
						if (connect(fSocket, ai->ai_addr, ai->ai_addrlen) != SOCKET_ERROR)
						{
							// connected!
							ai = NULL;
						}
						else
						{
							closesocket(fSocket);
							fSocket = INVALID_SOCKET;
							ai = ai->ai_next;
						}
					}
					else
						ai = ai->ai_next;
				} while (ai);
				freeaddrinfo(aistart);
				if (fSocket != INVALID_SOCKET)
				{
					startReaderThread();
					// send connect info
					signalConnectInfo(fModelName, fModelID);
					// wait for unique client id as a signal that we are connected
					waitForConnected();
				}
			}
		}
	}
	else
	{
		if (getConnected())
		{
			SOCKET localsocket = fSocket;
			fSocket = INVALID_SOCKET;
			closesocket(localsocket);
		}
	}
}
void TSocketConnection::writePacket(TByteBuffer &aPacket, bool aCallCloseOnError)
{
	EnterCriticalSection(&fWritePacketLock);
	try
	{
		if (aPacket.getCapacity() < imbMinimumPacketSize)
			aPacket.setCapacity(imbMinimumPacketSize); // make sure memory is intialized with zeros
		send(fSocket, (char*)aPacket.getBuffer(), max(aPacket.getLimit(), imbMinimumPacketSize), 0);
	}
	catch (...)
	{
		if (aCallCloseOnError)
			close(false);
	}
	LeaveCriticalSection(&fWritePacketLock);
}
int TSocketConnection::readBytes(byte aBuffer[], int aOffset, int aLimit)
{
	int totalBytesRead = 0;
	int bytesRead;
	while (aOffset < aLimit && getConnected())
	{
		bytesRead = recv(fSocket, (char*)&aBuffer[aOffset], aLimit - aOffset, 0); // MSG_WAITALL);
		if (bytesRead > 0)
		{
			aOffset += bytesRead;
			totalBytesRead += bytesRead;
		}
		else
			aOffset = aLimit;
	}
	return totalBytesRead;
}

// TTLSConnection
// todo: NOT implement yet
