import socket
import threading
import time
import struct  # conversion of values to bytes and visa-versa
import uuid  # guid support
import ssl
import os
import sys  # for detecting python version

# ecodistrict defaults
DEFAULT_REMOTE_HOST = 'vps17642.public.cloudvps.com'  # 'localhost'
DEFAULT_PREFIX = 'ecodistrict'  # 'nl.imb'

# python 2 compatibility stuf
class ConnectionAbortedError(OSError):
    pass

BYTEORDER = 'little'
MAGIC_BYTE = 0xFE
DEFAULT_ENCODING = 'utf-8'
MINIMUM_PACKET_SIZE = 16
MAXIMUM_PAYLOAD_SIZE = 10 * 1024 * 1024
DEFAULT_STREAM_BODY_BUFFER_SIZE = 16 * 1024
SOCK_SELECT_TIMEOUT = 1  # seconds

INVALID_EVENT_ID = 0xFFFFFFFF

DEFAULT_REMOTE_SOCKET_PORT = 4004
DEFAULT_REMOTE_TLS_PORT = 4443

MSG_WAITALL = 8
# socket.MSG_WAITALL, seems not available in windows python socket lib but is supported on windows above xp

# change object event actions
ACTION_NEW = 0
ACTION_DELETE = 1
ACTION_CHANGE = 2


# tags

# imb command tags

icehRemark = 1  # <string>

# subscribe/publish
icehSubscribe = 2  # <uint32: varint>
icehPublish = 3  # <uint32: varint>
icehUnsubscribe = 4  # <uint32: varint>
icehUnpublish = 5  # <uint32: varint>
icehSetEventIDTranslation = 6  # <uint32: varint>
icehEventName = 7  # <string>
icehEventID = 8  # <uint32: varint>

# connection
icehUniqueClientID = 11  # <guid>
icehHubID = 12  # <guid>
icehModelName = 13  # <string>
icehModelID = 14  # <int32: varint> ?
icehReconnectable = 15  # <bool: varint>
icehState = 16  # <uint32: varint>
icehEventNameFilter = 17  # <string>
icehNoDelay = 18  # <bool: varint>
icehClose = 21  # <bool: varint>
icehReconnect = 22  # <guid>


# standard event tags

# basic event tags
icehIntString = 1  # <varint>
icehIntStringPayload = 2  # <string>
icehString = 3  # <string>
icehChangeObject = 4  # <int32: varint>
icehChangeObjectAction = 5  # <int32: varint>
icehChangeObjectAttribute = 6  # <string>

# stream
icehStreamHeader = 7  # <string> filename
icehStreamBody = 8  # <bytes>
icehStreamEnd = 9  # <bool> true: ok, false: cancel
icehStreamID = 10  # <id: bytes/string>


# connection states
icsUninitialized = 0
icsInitialized = 1
icsClient = 2
icsHub = 3
icsEnded = 4
icsTimer = 10
icsGateway = 100


""""
google protocol buffers implementation for IMB4
- serialisation of basic types to buffer: bb_* functions
- tag-prefixed serialisation of basic types to buffer: bb_tag_* functions
- reading of serialised basic types from buffer: ByteBuffer class
ByteBuffer class can also be used to build a protocol buffer
"""

# wire types
WT_VARINT = 0               # int32, int64, uint32, uint64, sint32, sint64, bool, enum
WT_64BIT = 1                # double or fixed int64/uint64
WT_LENGTH_DELIMITED = 2     # string, bytes, embedded messages, packed repeated fields
WT_START_GROUP = 3          # deprecated
WT_END_GROUP = 4            # deprecated
WT_32BIT = 5                # float (single) or fixed int32/uint32


def bb_uint(value):
    """
    convert unsigned integer as proto buf varint format into byte array
    :param value: unsigned integer
    :return: proto buf
    """
    buffer = bytearray()
    while value >= 128:
        buffer.append((value & 0x7F) | 0x80)
        value >>= 7
    buffer.append(value & 0x7F)
    return buffer


def bb_int(value):
    """
    convert signed integer as proto buf varint format into byte array
    :param value: signed integer
    :return: proto buf
    """
    if value < 0:
        return bb_uint(((-(value+1)) << 1) | 1)
    else:
        return bb_uint(value << 1)


def bb_string(value):
    """
    convert string as proto buf variable length; length of string as varint followed by utf8 encoded
    string contents into byte array
    :param value: string
    :return: proto buf
    """
    encoded = value.encode('utf-8')
    return bytearray().join([bb_uint(len(encoded)), encoded])


def bb_double(value):
    """
    convert double value as proto buf 64bit stored value
    :param value: double float value
    :return: proto buf
    """
    return struct.pack('<d', value)


def bb_single(value):
    """
    convert single value as proto buf 32bit stored value
    :param value: single float value
    :return: proto buf
    """
    return struct.pack('<f', value)


def bb_word(value):
    """
    convert word value (unsigned 2 byte integer) as proto buf 16 bit stored value (unofficial format!)
    :param value: unsigned 2 byte integer
    :return: proto buf
    """
    return struct.pack('<H', value)


def bb_bool(value):
    """
    convert boolean value as unsigned int proto buf varint format into byte array
    :param value: boolean
    :return: proto buf
    """
    if value:
        return bytearray([1])
    else:
        return bytearray([0])


def bb_guid(value):
    """
    convert guid (uuid) to variable length proto buf value; first the length as unsigned int followed by bytes of guid
    :param value: guid (uuid)
    :return: proto buf
    """
    guid_buffer = value.bytes
    return bytearray().join([bb_uint(len(guid_buffer)), guid_buffer])


def bb_tag_uint(tag, value):
    """
    build proto buf tagged unsigned integer value
    :param tag: code to identify value
    :param value: unsigned integer
    :return: proto buf tagged value
    """
    return bytearray().join([bb_uint((tag << 3) | WT_VARINT), bb_uint(value)])


def bb_tag_int(tag, value):
    """
    build proto buf tagged signed integer value
    :param tag: code to identify value
    :param value: signed integer
    :return: proto buf tagged value
    """
    return bytearray().join([bb_uint((tag << 3) | WT_VARINT), bb_int(value)])


def bb_tag_string(tag, value):
    """
    build proto buf tagged string value
    :param tag: code to identify value
    :param value: string
    :return: proto buf tagged value
    """
    return bytearray().join([bb_uint((tag << 3) | WT_LENGTH_DELIMITED), bb_string(value)])


def bb_tag_double(tag, value):
    """
    build proto buf tagged double (float) value
    :param tag: code to identify value
    :param value: double float
    :return: proto buf tagged value
    """
    return bytearray().join([bb_uint((tag << 3) | WT_64BIT), bb_double(value)])


def bb_tag_single(tag, value):
    """
    build proto buf tagged single (float) value
    :param tag: code to identify value
    :param value: single float
    :return: proto buf tagged value
    """
    return bytearray().join([bb_uint((tag << 3) | WT_32BIT), bb_single(value)])


def bb_tag_bool(tag, value):
    """
    build proto buf tagged boolean value coded as unsigned integer
    :param tag: code to identify value
    :param value: boolean
    :return: proto buf tagged value
    """
    return bytearray().join([bb_uint((tag << 3) | WT_VARINT), bb_bool(value)])


def bb_tag_guid(tag, value):
    """
    build proto buf tagged guid (uuid) value
    :param tag: code to identify value
    :param value: guid (uuid)
    :return: proto buf tagged value
    """
    return bytearray().join([bb_uint((tag << 3) | WT_LENGTH_DELIMITED), bb_guid(value)])


def bb_tag_bytes(tag, value):
    """
    build proto buf tagged bytes value
    :param tag: code to identify value
    :param value: raw bytes of data
    :return: proto buf tagged value
    """
    return bytearray().join([bb_uint((tag << 3) | WT_LENGTH_DELIMITED), bb_uint(len(value)), value])


def bb_decode_tag_and_wire_tye(value):
    """
    decode the tag and wire type from the varint as unsigned integer specified in value
    :param value: varint as unsigned integer containing encoded tag and wire type
    :return: tuple of decoded tag and decoded wire type (tag, wire type)
    """
    return value >> 3, value & 0x7


def bb_decode_double(buffer):
    return struct.unpack_from('<d', buffer)


def bb_decode_single(buffer):
    return struct.unpack_from('<f', buffer)


def bb_decode_string(buffer):
    return buffer.decode('utf-8')


def bb_decode_guid(buffer):
    return uuid.UUID(buffer)


class ByteBuffer(object):
    """
    ByteBuffer is a class to write and read a google protocol buffer
    there is a cursor position for reading from the given buffer
    writing to the buffer appends to the given buffer
    the internal buffer is a byte array
    """
    def __init__(self):
        """
        initialize the buffer to zero length with the read cursor at the start
        """
        self._buffer = bytearray()  # buffer itself
        self._cursor = 0  # zero based read cursor into buffer
        # self._count = self.size;

    @property
    def buffer(self):
        """
        retrieve internal buffer
        :return: internal buffer
        """
        return self._buffer

    @buffer.setter
    def buffer(self, value):
        """
        set the internal buffer
        """
        self._buffer = value
        self._cursor = 0
        # self._count = self.size

    @property
    def size(self):
        """
        get the length of the internal buffer
        :return: length of the internal buffer in bytes
        """
        return len(self._buffer) if self._buffer else 0

    @size.setter
    def size(self, value):
        cnt = value - self.size
        if cnt < 0:
            self._buffer = self._buffer[:value]
        elif cnt > 0:
            while cnt > 0:
                self._buffer.append(0)
                cnt -= 1

    # @property
    # def count(self):
    #     """
    #     get the current reading cursor position
    #     :return: reading position into the internal buffer in bytes
    #     """
    #     return self._count

    @property
    def cursor(self):
        """
        get the current reading cursor position
        :return: reading position into the internal buffer in bytes
        """
        return self._cursor

    @cursor.setter
    def cursor(self, value):
        """
        set the reading cursor (without any specific checking)
        :param value: new position of the reading cursor (in bytes)
        """
        self._cursor = value

    def clear(self):
        """
        reset the internal buffer to empty and reset the reading cursor to the start of the buffer
        """
        self._buffer.clear()
        self._cursor = 0
        # self._count = 0;

    @property
    def read_available(self):
        """
        determine how bytes can be read from the buffer in regard to the current read cursor
        :return: bytes available in buffer to read
        """
        return self.size - self._cursor

    @read_available.setter
    def read_available(self, value):
        if self.size - self._cursor != value:
            self.size = self._cursor + value

    # add values

    def add_uint(self, value):
        """
        add unsigned integer as varint to the buffer
        :param value: unsigned integer
        """
        while value > 127:
            self._buffer.append((value & 0xFF) | 0x80)
            value >>= 7
        self._buffer.append(value & 0xFF)

    def add_int(self, value):
        """
        add signed integer as varint to the buffer
        :param value: signed integer
        """
        if value < 0:
            self.add_uint(((-(value+1)) << 1) | 1)
        else:
            self.add_uint(value << 1)

    def add_string(self, value):
        """
        add string utf8 encoded to the buffer prefixed by the length as unsigned varint of the encoded string in bytes
        :param value: string
        """
        encoded = value.encode('utf-8')
        self.add_uint(len(encoded))
        self._buffer.extend(encoded)

    def add_double(self, value):
        """
        add double (float) to the buffer
        :param value: double (float)
        """
        self._buffer.extend(struct.pack('<d', value))

    def add_single(self, value):
        """
        add single (float) to the buffer
        :param value: single (float)
        """
        self._buffer.extend(struct.pack('<f', value))

    def add_word(self, value):
        """
        add word (unsigned 2 byte integer) to the buffer
        :param value: word (unsigned 2 byte integer)
        """
        self._buffer.extend(struct.pack('<H', value))

    def add_bool(self, value):
        """
        add boolean as unsigned varint to the buffer
        :param value: boolean
        """
        if value:
            self._buffer.append(1)
        else:
            self._buffer.append(0)

    def add_byte(self, value):
        """
        add byte to the buffer
        :param value: byte
        """
        self._buffer.append(value)

    def add_guid(self, value):
        """
        add guid (uuid) to the buffer prefixed by the length as unsigned varint of the guid in bytes
        :param value: guid (uuid)
        """
        guid_buffer = value.bytes
        self.add_uint(len(guid_buffer))
        self._buffer.extend(guid_buffer)

    # add fields with tag

    def add_field_uint(self, tag, value):
        self.add_uint((tag << 3) | WT_VARINT)
        self.add_uint(value)

    def add_field_int(self, tag, value):
        self.add_uint((tag << 3) | WT_VARINT)
        self.add_int(value)

    def add_field_string(self, tag, value):
        self.add_uint((tag << 3) | WT_LENGTH_DELIMITED)
        self.add_string(value)

    def add_field_double(self, tag, value):
        self.add_uint((tag << 3) | WT_64BIT)
        self.add_double(value)

    def add_field_single(self, tag, value):
        self.add_uint((tag << 3) | WT_32BIT)
        self.add_single(value)

    def add_field_bool(self, tag, value):
        self.add_uint((tag << 3) | WT_VARINT)
        self.add_bool(value)

    def add_field_guid(self, tag, value):
        self.add_uint((tag << 3) | WT_LENGTH_DELIMITED)
        self.add_guid(value)

    # read values from internal buffer on cursor location

    def read_uint(self):
        """
        read unsigned integer as varint from internal buffer at cursor location
        advances cursor with read number of bytes
        :return: unsigned integer
        """
        _shift = 0
        _value = 0
        _b = self._buffer[self._cursor]
        self._cursor += 1
        while _b > 127:
            _b &= 0x7F
            if _shift > 0:
                _b <<= _shift
            _shift += 7
            _value |= _b
            _b = self._buffer[self._cursor]
            self._cursor += 1
        if _shift > 0:
            _b <<= _shift
        _value |= _b
        return _value

    def read_int(self):
        """
        read signed integer as varint from internal buffer at cursor location
        advances cursor with read number of bytes
        :return: signed integer
        """
        ui = self.read_uint()
        if (ui & 1) == 1:
            return -((ui >> 1)+1)
        else:
            return ui >> 1

    def read_string(self):
        """
        read string from internal buffer at cursor location
        advances cursor with read number of bytes
        :return: string
        """
        _len = self.read_uint()
        s = self._buffer[self._cursor:self._cursor + _len].decode('utf-8')
        self._cursor += _len
        return s

    def read_double(self):
        """
        read double (float) from internal buffer at cursor location
        advances cursor with read number of bytes
        :return: double (float)
        """
        b = struct.unpack_from('<d', self._buffer, self._cursor)
        self._cursor += 8
        return b[0]  # un-tuple

    def read_single(self):
        """
        read single (float) from internal buffer at cursor location
        advances cursor with read number of bytes
        :return: single (float)
        """
        b = struct.unpack_from('<f', self._buffer, self._cursor)
        self._cursor += 4
        return b[0]  # un-tuple

    def read_bool(self):
        """
        read boolean as unsigned int from internal buffer at cursor location
        advances cursor with read number of bytes
        :return: boolean
        """
        b = self._buffer[self._cursor]
        self._cursor += 1
        return b != 0

    def read_guid(self):
        """
        read guid (uuid) from internal buffer at cursor location
        advances cursor with read number of bytes
        :return: guid (uuid)
        """
        _len = self.read_uint()
        b = bytes(self._buffer[self._cursor:self._cursor + _len])
        guid = uuid.UUID(bytes_le=b)
        self._cursor += _len
        return guid

    def read_bytes(self, number_of_bytes):
        """
        read specific number of bytes from the internal buffer
        advances cursor with read number of bytes
        :param number_of_bytes: number of bytes to read from internal buffer
        :return: bytes read from the internal buffer
        """
        b = self._buffer[self._cursor:self._cursor + number_of_bytes]
        self._cursor += number_of_bytes
        return b

    def read_word(self):
        b = struct.unpack_from('<H', self._buffer, self._cursor)
        self._cursor += 2
        return b[0]  # un-tuple

    def read_field_tag_and_wire_type(self):
        """
        read field header from internal buffer and decode the field tag value and wire type of value to follow
        :return: tag, wire_type
        """
        tag_and_wire_type = self.read_uint()
        return tag_and_wire_type >> 3, tag_and_wire_type & 0x7

    def read_field(self):
        """
        read field based on tag and wire type
        :return: tag, wire_type, bytes array with bytes belonging to this field or varint as unsigned integer
        """
        tag_and_wire_type = self.read_uint()
        tag = tag_and_wire_type >> 3
        wire_type = tag_and_wire_type & 0x7
        if wire_type == WT_VARINT:
            return tag, wire_type, self.read_uint()
        elif wire_type == WT_64BIT:
            return tag, wire_type, self.read_bytes(8)
        elif wire_type == WT_32BIT:
            return tag, wire_type, self.read_bytes(4)
        elif wire_type == WT_LENGTH_DELIMITED:
            _len = self.read_uint()
            return tag, wire_type, self.read_bytes(_len)
        else:
            raise Exception('invalid wire type in ByteBuffer.read_field: ', wire_type)

    def read_skip(self, wire_type):
        if wire_type == WT_VARINT:
            self.read_uint()
        elif wire_type == WT_64BIT:
            self._cursor += 8
        elif wire_type == WT_32BIT:
            self._cursor += 4
        elif wire_type == WT_LENGTH_DELIMITED:
            _len = self.read_uint()
            self._cursor += _len

        else:
            raise Exception('invalid wire type in ByteBuffer.read_skip: ', wire_type)

    def read_field_info(self):
        """
        read field info, based on tag and wire type
        :return: combined tag and wire_type
        """
        return self.read_uint()


class TEventEntry:
    """Represents an event in the IMB framework

    The :class:`TEventEntry` object represents an event (a named
    communication channel) in the IMB framework. It is created by a
    :class:`TConnection` instance and can then be used to

    *   subscribe/unsubscribe to the event,
    *   publish/unpublish the event,
    *   send signals, and
    *   setup handlers for incoming signals.

    Note:
        You should not create TEventEntry instances yourself.
        Instead, call ::func:`TConnection.subscribe` or :func:`TConnection.publish`.
    """

    def __init__(self, event_id, event_name, connection,
                 on_string_event, on_int_string_event, on_change_object, on_stream_create, on_stream_end, on_tag, on_sub_and_pub):
        """Constructor for `TEventEntry`.

        You should not create TEventEntry instances yourself.
        Instead, call :func:`TConnection.subscribe` or :func:`TConnection.publish`.

        Args:
            event_id (int): The TConnection's id for the event.
            name (str): The name used to identify the event.
            connection (imb.TConnection): The TConnection instance to be used.
        """
        self._event_id = event_id
        self._event_name = event_name
        self._connection = connection
        self._is_subscribed = False  # are we subscribed to this event
        self._is_published = False  # are we publishing on this event
        self._subscribers = False  # are there subscribers on the framework on this event
        self._publishers = False  # are there publishers on the framework on  this event
        # handlers
        self._on_string_event = on_string_event
        self._on_int_string_event = on_int_string_event
        self._on_change_object = on_change_object
        self._on_stream_create = on_stream_create
        self._on_stream_end = on_stream_end
        self._on_tag = on_tag
        self._on_sub_and_pub = on_sub_and_pub
        # stream cache
        self._stream_cache = {}

    @property
    def event_name(self):
        return self._event_name

    @property
    def event_id(self):
        return self._event_id

    @property
    def connection(self):
        return self._connection

    @property
    def subscribers(self):
        return self._subscribers

    @property
    def publishers(self):
        return self._publishers

    # handlers
    @property
    def on_string_event(self):
        return self._on_string_event

    @on_string_event.setter
    def on_string_event(self, value):
        self._on_string_event = value

    @property
    def on_int_string_event(self):
        return self._on_int_string_event

    @on_int_string_event.setter
    def on_int_string_event(self, value):
        self._on_int_string_event = value

    @property
    def on_change_object(self):
        return self._on_change_object

    @on_change_object.setter
    def on_change_object(self, value):
        self._on_change_object = value

    @property
    def on_stream_create(self):
        return self._on_stream_create

    @on_stream_create.setter
    def on_stream_create(self, value):
        self._on_stream_create = value

    @property
    def on_stream_end(self):
        return self._on_stream_end

    @on_stream_end.setter
    def on_stream_end(self, value):
        self._on_stream_end = value

    @property
    def on_tag(self):
        return self._on_tag

    @on_tag.setter
    def on_tag(self, value):
        self._on_tag = value

    @property
    def on_sub_and_pub(self):
        return self._on_sub_and_pub

    @on_sub_and_pub.setter
    def on_sub_and_pub(self, value):
        self._on_sub_and_pub = value

    def subscribe(self):
        """Subscribe the owning TConnection from this event."""
        if not self._is_subscribed:
            self.connection.signal_subscribe(self.event_id, self.event_name)
            self._is_subscribed = True

    def unsubscribe(self):
        """Unsubscribe the owning connection from this event."""
        if self._is_subscribed:
            self.connection.signal_unsubscribe(self.event_name)
            self._is_subscribed = False

    def publish(self):
        """Publish this event with the owning TConnection"""
        if not self._is_published:
            self.connection.signal_publish(self.event_id, self.event_name)
            self._is_published = True

    def unpublish(self):
        """Unpublish this event with the owning TConnection."""
        if self._is_published:
            self.connection.signal_unpublish(self.event_name)
            self._is_published = False

    def handle_event(self, payload):
        command_payload = ''
        action = -1
        attribute = ''
        stream_id = None
        while payload.read_available > 0:
            field_info = payload.read_field_info()
            if field_info == icehIntString << 3 | WT_VARINT:
                command = payload.read_int()
                if callable(self._on_int_string_event):
                    self._on_int_string_event(self, command, command_payload)
            elif field_info == icehIntStringPayload << 3 | WT_LENGTH_DELIMITED:
                command_payload = payload.read_string()
            elif field_info == icehString << 3 | WT_LENGTH_DELIMITED:
                command = payload.read_string()
                if callable(self._on_string_event):
                    self._on_string_event(self, command)
            elif field_info == icehChangeObjectAction << 3 | WT_VARINT:
                action = payload.read_int()
            elif field_info == icehChangeObjectAttribute << 3 | WT_LENGTH_DELIMITED:
                attribute = payload.read_string()
            elif field_info == icehChangeObject << 3 | WT_VARINT:
                object_id = payload.read_int()
                if callable(self._on_change_object):
                    self._on_change_object(self, action, object_id, attribute)
            elif field_info == icehStreamHeader << 3 | WT_LENGTH_DELIMITED:
                stream_name = payload.read_string()
                if callable(self._on_stream_create):
                    stream = self._on_stream_create(self, stream_name)
                    self._stream_cache[stream_id] = (stream, stream_name)
            elif field_info == icehStreamBody << 3 | WT_LENGTH_DELIMITED:
                stream = self._stream_cache[stream_id][0]
                if stream:
                    cnt = payload.read_uint()
                    stream.write(payload.read_bytes(cnt))
                else:
                    payload.read_skip(WT_LENGTH_DELIMITED)
            elif field_info == icehStreamEnd << 3 | WT_VARINT:
                cancel = payload.read_bool()
                stream, stream_name = self._stream_cache[stream_id]
                if callable(self._on_stream_end):
                    self._on_stream_end(self, stream, stream_name, cancel)
                stream.close()
                del self._stream_cache[stream_id]
            elif field_info == icehStreamID << 3 | WT_LENGTH_DELIMITED:
                stream_id = payload.read_guid()
            else:
                if callable(self._on_tag):
                    self._on_tag(self, field_info, payload)
                else:
                    payload.read_skip(field_info & 7)

    def handle_sub_and_pub(self, command):
        if command == icehSubscribe:
            self._subscribers = True
        elif command == icehPublish:
            self._publishers = True
        elif command == icehUnsubscribe:
            self._subscribers = False
        elif command == icehUnpublish:
            self._publishers = False
        if callable(self._on_sub_and_pub):
            self._on_sub_and_pub(self, command)

    def signal_event(self, event_payload):
        """Send a message on the event.

        Args:
            event_payload (bytes or similar): The payload to send.
        """
        if not self._is_published:
            self.publish()
        self.connection.signal_event(self.event_id, event_payload)

    def signal_int_string_event(self, command, command_payload):
        self.signal_event(bytearray().join([
            bb_tag_string(icehIntStringPayload, command_payload),
            bb_tag_int(icehIntString, command)]))

    def signal_string_event(self, command):
        self.signal_event(bytearray().join([
            bb_tag_string(icehString, command)]))

    def signal_change_object(self, action, object_id, attribute=''):
        self.signal_event(bytearray().join([
            bb_tag_int(icehChangeObjectAction, action),
            bb_tag_string(icehChangeObjectAttribute, attribute),
            bb_tag_int(icehChangeObject, object_id)]))

    def signal_stream(self, name, stream, chunk_size=DEFAULT_STREAM_BODY_BUFFER_SIZE):
        stream_id = uuid.uuid4()
        buffer_stream_id = bb_tag_guid(icehStreamID, stream_id)
        self.signal_event(
            bytearray().join([
                buffer_stream_id,
                bb_tag_string(icehStreamHeader, name)]))
        chunk = stream.read(chunk_size)
        while len(chunk) > 0:
            self.signal_event(bytearray().join([
                buffer_stream_id,
                bb_tag_bytes(icehStreamBody, chunk)]))
            chunk = stream.read(chunk_size)
        self.signal_event(bytearray().join([
            buffer_stream_id,
            bb_tag_bool(icehStreamEnd, False)]))


class TConnection:
    def __init__(self, host, port, secure, model_name, model_id, prefix=DEFAULT_PREFIX, reconnectable=False):
        # init fields
        self._event_entries = []
        self._event_remote_to_local = {}
        self._unique_client_id = uuid.UUID('{00000000-0000-0000-0000-000000000000}')
        self._hub_id = None
        self._packets_dropped = 0
        self._connected = False
        self._reconnectable = reconnectable
        self._prefix = prefix
        self._model_name = model_name
        self._model_id = model_id
        self._connected = False
        self._on_disconnect = None
        # handlers
        self._on_sub_and_pub = None

        # setup the socket
        if (secure):
            _working_folder = os.getcwd()
            self._socket = ssl.wrap_socket(
                socket.socket(socket.AF_INET, socket.SOCK_STREAM),
                keyfile=_working_folder+'\\client-eco-district-nopass.key',
                certfile=_working_folder+'\\client-eco-district.crt',
                cert_reqs=ssl.CERT_REQUIRED,
                ssl_version=ssl.PROTOCOL_TLSv1_2,
                ca_certs=_working_folder+'\\root-ca-imb.crt')
        else:
            self._socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self._socket.connect((host, port))
        self._connected = True

        # start reader thread
        self._reader_thread = threading.Thread(target=self._read_commands, name='IMB reader thread')
        self._reader_thread.start()

        # send client information
        self.signal_connect_info()

        spin_count = 20
        while self._unique_client_id == uuid.UUID('{00000000-0000-0000-0000-000000000000}') and spin_count > 0:
            time.sleep(0.1)
            spin_count -= 1

    def __del__(self):
        self.close(False)

    def close(self, send_close_command=True):
        if self._connected:
            if callable(self._on_disconnect):
                # noinspection PyCallingNonCallable
                self._on_disconnect(self)
            if send_close_command:
                self.signal_close()
            self._connected = False
            self._socket.shutdown(socket.SHUT_RDWR)
            self._socket.close()

    # def __enter__(self):
    # return self
    #
    # def __exit__(self, type, value, traceback):
    #     self.close(False)

    @property
    def connected(self):
        return self._connected

    @property
    def on_disconnect(self):
        return self._on_disconnect

    @on_disconnect.setter
    def on_disconnect(self, value):
        self._on_disconnect = value

    @property
    def prefix(self):
        return self._prefix

    @property
    def commands_dropped(self):
        return self._packets_dropped

    @property
    def private_event_name(self):
        if self._unique_client_id == uuid.UUID('{00000000-0000-0000-0000-000000000000}'):
            return ''
        else:
            return 'Clients.' + self._unique_client_id.hex.upper() + '.Private'

    @property
    def monitor_event_name(self):
        if self._hub_id == uuid.UUID('{00000000-0000-0000-0000-000000000000}') or self._hub_id == None:
            return ''
        else:
            return 'Hubs.' + self._hub_id.hex.upper() + '.Monitor'

    def _read_bytes(self, num_bytes):
        """
        work-a-round code when MSG_WAITALL is not working
        :param num_bytes: number of bytes that has to be read before returning (or connection error occurs)
        :return: bytes read from socket
        """
        buffer = bytearray()
        while num_bytes > 0:
            b = self._socket.recv(num_bytes)
            if len(b) > 0:
                num_bytes -= len(b)
                buffer.extend(b)
            else:
                raise socket.error('socket connection error in reading bytes')
        return buffer

    def handle_disconnect(self):
        if self._connected:
            self._connected = False
            if callable(self._on_disconnect):
                # noinspection PyCallingNonCallable
                self._on_disconnect(self)
            self._socket.shutdown(socket.SHUT_RDWR)
            self._socket.close()

    def _handle_event(self, buffer):
        try:
            # read event id (word, 2 bytes unsigned integer)
            remote_event_id = buffer.read_word()
            event_entry = self._event_entries[self._event_remote_to_local[remote_event_id]]
            event_entry.handle_event(buffer)
        except Exception as e:
            print('## exception handling imb event: ', e)

    def handle_sub_and_pub(self, command, local_event_id, event_name):
        if local_event_id != INVALID_EVENT_ID:
            event_entry = self._event_entries[local_event_id]
            event_entry.handle_sub_and_pub(command)
        if callable(self._on_sub_and_pub):
            # noinspection PyCallingNonCallable
            self._on_sub_and_pub(command, local_event_id, event_name)

    def set_no_delay(self, no_delay_value):
        if no_delay_value:
            self._socket.setsockopt(socket.IPPROTO_TCP, socket.TCP_NODELAY, 1)
        else:
            self._socket.setsockopt(socket.IPPROTO_TCP, socket.TCP_NODELAY, 0)

    def _handle_command(self, buffer):
        try:
            event_name = ''
            local_event_id = INVALID_EVENT_ID
            # remote_event_id = INVALID_EVENT_ID
            while buffer.read_available > 0:
                field_info = buffer.read_field_info()
                if field_info == icehSubscribe << 3 | WT_VARINT:
                    remote_event_id = buffer.read_uint()
                    if remote_event_id in self._event_remote_to_local:
                        local_event_id = self._event_remote_to_local[remote_event_id]
                    else:
                        local_event_id = INVALID_EVENT_ID
                    self.handle_sub_and_pub(icehSubscribe, local_event_id, event_name)
                elif field_info == icehPublish << 3 | WT_VARINT:
                    remote_event_id = buffer.read_uint()
                    if remote_event_id in self._event_remote_to_local:
                        local_event_id = self._event_remote_to_local[remote_event_id]
                    else:
                        local_event_id = INVALID_EVENT_ID
                    self.handle_sub_and_pub(icehPublish, local_event_id, event_name)
                elif field_info == icehUnsubscribe << 3 | WT_VARINT:
                    remote_event_id = buffer.read_uint()
                    event_name = ''
                    if remote_event_id in self._event_remote_to_local:
                        local_event_id = self._event_remote_to_local[remote_event_id]
                    else:
                        local_event_id = INVALID_EVENT_ID
                    self.handle_sub_and_pub(icehUnsubscribe, local_event_id, event_name)
                elif field_info == icehUnpublish << 3 | WT_VARINT:
                    remote_event_id = buffer.read_uint()
                    event_name = ''
                    if remote_event_id in self._event_remote_to_local:
                        local_event_id = self._event_remote_to_local[remote_event_id]
                    else:
                        local_event_id = INVALID_EVENT_ID
                    self.handle_sub_and_pub(icehUnpublish, local_event_id, event_name)
                elif field_info == icehEventName << 3 | WT_LENGTH_DELIMITED:
                    event_name = buffer.read_string()
                elif field_info == icehEventID << 3 | WT_VARINT:
                    local_event_id = buffer.read_uint()
                elif field_info == icehSetEventIDTranslation << 3 | WT_VARINT:
                    remote_event_id = buffer.read_uint()
                    self._event_remote_to_local[remote_event_id] = local_event_id
                elif field_info == icehNoDelay << 3 | WT_VARINT:
                    no_delay_value = buffer.read_uint()
                    self.set_no_delay(no_delay_value)
                elif field_info == icehClose << 3 | WT_VARINT:
                    buffer.read_bool()
                    self.close(False)
                elif field_info == icehHubID << 3 | WT_LENGTH_DELIMITED:
                    self._hub_id = buffer.read_guid()
                elif field_info == icehUniqueClientID << 3 | WT_LENGTH_DELIMITED:
                    self._unique_client_id = buffer.read_guid()
                else:
                    buffer.read_skip(field_info & 7)
        except Exception as e:
            print('## exception handling imb command: ', e)

    def _read_commands(self):
        while self._connected:
            # noinspection PyBroadException
            try:
                header = self._read_bytes(MINIMUM_PACKET_SIZE)  # self._socket.recv(MINIMUM_PACKET_SIZE, MSG_WAITALL)
                if len(header) == MINIMUM_PACKET_SIZE:
                    if header[0] != MAGIC_BYTE:
                        # we have an abnormal packet because first byte is not magic byte
                        # find magic byte
                        h = 1
                        while (h < MINIMUM_PACKET_SIZE) and (header[h] != MAGIC_BYTE):
                            h += 1
                        if h < MINIMUM_PACKET_SIZE:
                            # rebuild header form magic byte on
                            old_header = header[h:]
                            extra_bytes = self._read_bytes(h)  # self._socket.recv(h, MSG_WAITALL)
                            if len(extra_bytes) == h:
                                header = bytearray().join([old_header, extra_bytes])
                            else:
                                header = None
                                self.handle_disconnect()
                        else:
                            header = None  # discard header all together
                    if header:
                        # normal packet processing
                        buffer = ByteBuffer()
                        buffer.buffer = bytearray(header)
                        buffer.cursor = 1
                        size = buffer.read_int()
                        ra = buffer.read_available
                        if size > 0:
                            # event
                            if size <= MAXIMUM_PAYLOAD_SIZE:
                                if size > ra:
                                    # read extra bytes in payload that are not already read into the buffer
                                    extra_bytes_to_read = size - ra
                                    extra_bytes = self._read_bytes(extra_bytes_to_read)  # self._socket.recv(extra_bytes_to_read, MSG_WAITALL)
                                    if len(extra_bytes) == extra_bytes_to_read:
                                        buffer.buffer.extend(extra_bytes)
                                        self._handle_event(buffer)
                                    else:
                                        self.handle_disconnect()
                                else:
                                    if ra > size:
                                        buffer.read_available = size
                                    self._handle_event(buffer)
                            else:
                                self._packets_dropped += 1
                        else:
                            # command
                            size = -size
                            if size <= MAXIMUM_PAYLOAD_SIZE:
                                if size > ra:
                                    # read extra bytes in payload that are not already read into the buffer
                                    extra_bytes_to_read = size - ra
                                    extra_bytes = self._read_bytes(extra_bytes_to_read)  # self._socket.recv(extra_bytes_to_read, MSG_WAITALL)
                                    if len(extra_bytes) == extra_bytes_to_read:
                                        buffer.buffer.extend(extra_bytes)
                                        self._handle_command(buffer)
                                    else:
                                        self.handle_disconnect()
                                else:
                                    if ra > size:
                                        buffer.read_available = size
                                    self._handle_command(buffer)
                            else:
                                self._packets_dropped += 1
                else:
                    self.handle_disconnect()
                # todo: have to determine which exceptions should cause a disconnect
            except OSError:
                self.handle_disconnect()
            except ConnectionAbortedError:
                self.handle_disconnect()
            except Exception:
                self.handle_disconnect()

    def write_packet(self, packet):
        if len(packet) < MINIMUM_PACKET_SIZE:
            packet.extend(b'\x00' * (MINIMUM_PACKET_SIZE - len(packet)))
        self._socket.sendall(packet)

    def signal_event(self, event_id, payload):
        event_id_buffer = bb_word(event_id)
        self.write_packet(bytearray().join([
            bytearray([MAGIC_BYTE]),  # magic
            bb_int(len(event_id_buffer) + len(payload)),  # size > 0 for event
            event_id_buffer,  # event id
            payload]))  # events

    def signal_connect_info(self, event_name_filter=''):
        payload = bytearray().join([
            bb_tag_string(icehModelName, self._model_name),
            bb_tag_int(icehModelID, self._model_id),
            bb_tag_uint(icehState, icsClient),
            bb_tag_bool(icehReconnectable, self._reconnectable),
            bb_tag_string(icehEventNameFilter, event_name_filter),
            bb_tag_guid(icehUniqueClientID, self._unique_client_id)])
        self.write_packet(bytearray().join([
            bytearray([MAGIC_BYTE]),  # magic
            bb_int(-len(payload)),  # size < 0 for command
            payload]))  # commands

    def signal_state(self, state=icsClient):
        payload = bytearray().join([
            bb_tag_uint(icehState, state)])
        self.write_packet(bytearray().join([
            bytearray([MAGIC_BYTE]),  # magic
            bb_int(-len(payload)),  # size < 0 for command
            payload]))  # commands

    def signal_close(self):
        payload = bytearray().join([
            bb_tag_bool(icehClose, False)])
        self.write_packet(bytearray().join([
            bytearray([MAGIC_BYTE]),  # magic
            bb_int(-len(payload)),  # size < 0 for command
            payload]))  # commands

    def signal_reconnect(self, unique_client_id):
        payload = bytearray().join([
            bb_tag_guid(icehReconnect, unique_client_id)])
        self.write_packet(bytearray().join([
            bytearray([MAGIC_BYTE]),  # magic
            bb_int(-len(payload)),  # size < 0 for command
            payload]))  # commands

    def signal_subscribe(self, event_id, event_name):
        payload = bytearray().join([
            bb_tag_string(icehEventName, event_name),
            bb_tag_uint(icehSubscribe, event_id)])
        self.write_packet(bytearray().join([
            bytearray([MAGIC_BYTE]),  # magic
            bb_int(-len(payload)),  # size < 0 for command
            payload]))  # commands

    def signal_unsubscribe(self, event_id):
        payload = bytearray().join([
            bb_tag_uint(icehUnsubscribe, event_id)])
        self.write_packet(bytearray().join([
            bytearray([MAGIC_BYTE]),  # magic
            bb_int(-len(payload)),  # size < 0 for command
            payload]))  # commands

    def signal_publish(self, event_id, event_name):
        payload = bytearray().join([
            bb_tag_string(icehEventName, event_name),
            bb_tag_uint(icehPublish, event_id)])
        self.write_packet(bytearray().join([
            bytearray([MAGIC_BYTE]),  # magic
            bb_int(-len(payload)),  # size < 0 for command
            payload]))  # commands

    def signal_unpublish(self, event_id):
        payload = bytearray().join([
            bb_tag_uint(icehUnpublish, event_id)])
        self.write_packet(bytearray().join([
            bytearray([MAGIC_BYTE]),  # magic
            bb_int(-len(payload)),  # size < 0 for command
            payload]))  # commands

    # event handling

    def _get_event_entry(self, event_name, use_prefix=True, create=False,
                         on_string_event=None, on_int_string_event=None, on_change_object=None,
                         on_stream_create=None, on_stream_end=None, on_tag=None, on_sub_and_pub=None):
        """
        Get an TEventEntry object that can be used for communication.
        Do not call this directly; use subscribe or publish to link to an
        TEventEntry instance.

        Args:
            event_name (str): The name of the event.
            use_prefix (bool, optional): If `True`, the event name will be prefixed with
                the connection `prefix` + `.`, so you get something like
                ` my_prefix.my_event_name`.
            create (bool, optional): If `True` (the default), the event will be
                created if it doesn't exist yet. If `False`, and the event doesn't exist
                yet, this function will return `None`.

        Returns:
            An TEventEntry object or None.
        """
        if use_prefix:
            event_name = self._prefix + '.' + event_name
        event_name_lower = event_name.lower()

        # Look for the event among the existing ones
        for event_entry in self._event_entries:
            if event_entry.event_name.lower() == event_name_lower:
                if on_string_event:
                    event_entry.on_string_event = on_string_event
                if on_int_string_event:
                    event_entry.on_int_string_event = on_int_string_event
                if on_change_object:
                    event_entry.on_change_object = on_change_object
                if on_stream_create:
                    event_entry.on_stream_create = on_stream_create
                if on_stream_end:
                    event_entry.on_stream_end = on_stream_end
                if on_tag:
                    event_entry.on_tag = on_tag
                if on_sub_and_pub:
                    event_entry.on_sub_and_pub = on_sub_and_pub
                return event_entry

        # todo: add step to reuse not subscribed and not published events

        # If we have come this far, the event has not been created yet
        if create:
            event_id = len(self._event_entries)
            event = TEventEntry(event_id, event_name, self,
                                on_string_event, on_int_string_event, on_change_object,
                                on_stream_create, on_stream_end, on_tag, on_sub_and_pub)
            self._event_entries.append(event)
            return event
        else:
            return None

    def subscribe(self, event_name, use_prefix=True,
                  on_string_event=None, on_int_string_event=None, on_change_object=None,
                  on_stream_create=None, on_stream_end=None, on_tag=None, on_sub_and_pub=None):
        """subscribe to the given event

        Args:
            event_name (str): See docs for :func:`Client.get_event`.
            use_prefix (bool, optional): See docs for :func:`Client.get_event`.

        Returns:
            A subscribed TEventEntry object.

        Example:
        >>> def handle_event(event_entry, command, command_payload):
        ...     print('received event', command, command_payload)  # show received command
        ...     event_entry.signal_int_string_event(9, 'payload for command 9')  # response

        >>> connection = TConnection(DEFAULT_REMOTE_HOST, DEFAULT_REMOTE_SOCKET_PORT, 'model name', 1, DEFAULT_PREFIX)
        >>> event_entry = connection.subscribe('my_event', on_int_string_event=handle_event)
        >>> event_entry.signal_event(bb_tag_string(100, 'a string payload'))
        """

        event = self._get_event_entry(event_name, use_prefix, True,
                                      on_string_event, on_int_string_event, on_change_object,
                                      on_stream_create, on_stream_end, on_tag, on_sub_and_pub)
        event.subscribe()
        return event

    def unsubscribe(self, event_name, use_prefix=True):
        """Ensure that the client is not subscribed to an event.

        Args:
            event_name (str): See docs for :func:`Client.get_event`.
            use_prefix (bool, optional): See docs for :func:`Client.get_event`.

        Returns:
            The TEventEntry object, if it already existed. Otherwise `None`.
        """
        event = self._get_event_entry(event_name, use_prefix=use_prefix)
        if event:
            event.unsubscribe()
        return event

    def publish(self, event_name, use_prefix=True,
                on_string_event=None, on_int_string_event=None, on_change_object=None,
                on_stream_create=None, on_stream_end=None, on_tag=None, on_sub_and_pub=None):
        """Get an TEventEntry object and ensure it is published.

        Args:
            event_name (str): See docs for :func:`Client.get_event`.
            use_prefix (bool, optional): See docs for :func:`Client.get_event`.

        Returns:
            A published TEventEntry object.

        Example

        """
        event = self._get_event_entry(event_name, use_prefix, True,
                                      on_string_event, on_int_string_event, on_change_object,
                                      on_stream_create, on_stream_end, on_tag, on_sub_and_pub)
        event.publish()
        return event

    def unpublish(self, event_name, use_prefix=True):
        """Ensure that the client is not publishing an event.

        Args:
            event_name (str): See docs for :func:`Client.get_event`.
            use_prefix (bool, optional): See docs for :func:`Client.get_event`.

        Returns:
            The TEventEntry object, if it already existed. Otherwise `None`.
        """
        event = self._get_event_entry(event_name, use_prefix=use_prefix)
        if event:
            event.unpublish()
        return event
