import os
import imb4

STREAM_INPUT_FILENAME = 'test.jpg'  # todo: use file name of existing file
STREAM_OUTPUT_FILENAME = 'out.python.dmp'

# define handlers for a string event, the creation of a stream and the end of a received stream
def handle_string_event(event_entry, command):
    if command == 'string command':
        print('OK received string', event_entry.event_name, command)
    else:
        print('## received string', event_entry.event_name, command)


def handle_stream_create(event_entry, stream_name):
    if stream_name == 'a stream name':
        print('OK received stream create', event_entry.event_name, stream_name)
    else:
        print('## received stream create', event_entry.event_name, stream_name)
    return open(STREAM_OUTPUT_FILENAME, 'wb')


def handle_stream_end(event_entry, stream, stream_name, cancel):
    if stream and stream_name == 'a stream name' and not cancel:
        print('OK received stream end', event_entry.event_name, stream_name, cancel)
    else:
        print('## received stream end', event_entry.event_name, stream_name, cancel)


def handle_disconnect(_):
    print('disconnected..')


# print the current folder to more easily locate stream input/output files
print('current folder', os.getcwd())

connection = imb4.TConnection(imb4.DEFAULT_REMOTE_HOST, imb4.DEFAULT_REMOTE_PORT, 'python test client', 1)
# test_connection = imb4.TConnection('192.168.1.100', imb4.DEFAULT_REMOTE_PORT, 'python test client', 1)

print('private event name', connection.private_event_name)
print('monitor event name', connection.monitor_event_name)

connection.on_disconnect = handle_disconnect

# compact statement to subscribe to int-string event and add handler
event = connection.subscribe('test event',
                             on_string_event=handle_string_event,
                             on_stream_create=handle_stream_create,
                             on_stream_end=handle_stream_end
                             )

print('subscribed to', event.event_name)

input('waiting on imb commands; press return to send events.. ')

if connection.connected:
 
    # signal string command event
    event.signal_string_event('string command')

    # signal stream
    s = open(STREAM_INPUT_FILENAME, 'rb')
    try:
        event.signal_stream('a stream name', s)
    finally:
        s.close()

    input('waiting on imb commands; press return to quit.. ')

connection.close()