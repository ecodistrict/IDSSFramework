var imb = require('./imb4');

var imbConnection = new imb.TIMBConnection(imb.imbDefaultHostname, imb.imbDefaultPort, 10, "node.js client", imb.imbDefaultPrefix, false);

imbConnection.on("onUniqueClientID", function (aUniqueClientID, aHubID) {
    console.log('private event name: ' + imbConnection.privateEventName);
    console.log('monitor event name: ' + imbConnection.monitorEventName);
});

imbConnection.on("onDisconnect", function (obj) {
    console.log("disonnected");
});

var event = imbConnection.subscribe("test event");

// add handlers for string events, creation of a stream and the end of a stream
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

// send a string event
event.signalString('string command');

// send a stream
event.signalStream('a stream name', require('fs').createReadStream('test.jpg')); // todo: use file name of existing file

console.log('sent events..')

var readline = require('readline'), rl = readline.createInterface(process.stdin, process.stdout);

rl.setPrompt('>');
rl.prompt();

rl.on('line', function (line) {
    switch (line.trim()) {
        case 'help':
        case '?':
            console.log('   s: send string');
            console.log('   S: send stream');
            console.log('   quit or q: exit application');
            console.log('   help or ?: this text');
            break;
        case 'quit':
        case 'q':
            rl.close();
            break;
        case 's':
            console.log('   sending string');
            event.signalString('string command');
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