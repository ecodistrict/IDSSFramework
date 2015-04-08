var imb = require('./imb4');

var imbConnection = new imb.TIMBConnection(imb.imbDefaultHostname, imb.imbDefaultPort, 10, "node.js client", imb.imbDefaultPrefix, false);

console.log('private event name: ' + imbConnection.privateEventName);
console.log('monitor event name: ' + imbConnection.monitorEventName);

var event = imbConnection.subscribe("test event");

event.onChangeObject = function (aEventEntry, aAction, aObjectID, aAttribute) {
    if (aAction == imb.actionChange & aObjectID == 2345 & aAttribute == 'an attribute')
        console.log("OK received change object " + aEventEntry.eventName + " " + aAction + " " + aObjectID + " " + aAttribute);
    else
        console.log("## received change object " + aEventEntry.eventName + " " + aAction + " " + aObjectID + " " + aAttribute);
}
event.onIntString = function (aEventEntry, aInt, aString) {
    if (aInt == 1234 && aString == 'int string payload')
        console.log("OK received int string " + aEventEntry.eventName + " " + aInt + " " + aString);
    else
        console.log("## received int string " + aEventEntry.eventName + " " + aInt + " " + aString);
}
event.onString = function (aEventEntry, aString) {
    if (aString == 'string command')
        console.log("OK received string " + aEventEntry.eventName + " " + aString);
    else
        console.log("## received string " + aEventEntry.eventName + " " + aString);
}
event.onStreamCreate = function (aEventEntry, aStreamName) {
    if (aStreamName == 'a stream name')
        console.log('OK received stream create ' + aEventEntry.eventName + ' ' + aStreamName)
    else
        console.log('## received stream create ' + aEventEntry.eventName + ' ' + aStreamName);
    return require('fs').createWriteStream('out.node.js.dmp');
}
event.onStreamEnd = function (aEventEntry, aStream, aStreamName, aCancel) {
    if (aStreamName == 'a stream name' && !aCancel)
        console.log('OK received stream end ' + aEventEntry.eventName + ' ' + aStreamName + ' ' + aCancel)
    else
        console.log('## received stream end ' + aEventEntry.eventName + ' ' + aStreamName + ' ' + aCancel);
}


event.signalString('string command');
event.signalIntString(1234, 'int string payload');
event.signalStream('a stream name', require('fs').createReadStream('test.jpg')); // todo: use file name of existing file
event.signalChangeObject(imb.actionChange, 2345, 'an attribute');

console.log('sent events..')

var readline = require('readline'), rl = readline.createInterface(process.stdin, process.stdout);

rl.setPrompt('>');
rl.prompt();

rl.on('line', function (line) {
    switch (line.trim()) {
        case 'help':
        case '?':
            console.log('   i: send int-string');
            console.log('   s: send string');
            console.log('   c: send change object');
            console.log('   S: send stream');
            console.log('   quit or q: exit application');
            console.log('   help or ?: this text');
            break;
        case 'quit':
        case 'q':
            rl.close();
            break;
        case 'i':
            console.log('   sending int string');
            event.signalIntString(1234, 'int string payload');
            break;
        case 's':
            console.log('   sending string');
            event.signalString('string command');
            break;
        case 'c':
            console.log('   sending change object');
            event.signalChangeObject(imb.actionChange, 2345, 'an attribute');
            break;
        case 'S':
            console.log('   sending Stream')
            event.signalStream('a stream name', require('fs').createReadStream('c:/temp/test.jpg'));
            break;
        default:
            console.log('## not avalid command, type help or ? for help');
            break;
    }
    rl.prompt();
}).on('close', function () {
    // close socket
    //socket.end();
    imbConnection.disconnect();
    console.log('test ended..');
    process.exit(0);
});