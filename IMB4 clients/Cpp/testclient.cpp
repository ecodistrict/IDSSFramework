#include "stdafx.h"
#include <iostream>
#include <fstream>
#include "imb4.h"

[event_receiver(native)]
class TEventHandlers
{
public:
	void handleDisconnect(TConnection &aConnection)
	{
		std::cout << "disconnected.." << std::endl;
	};
	void handleException(TConnection &aConnection, const std::exception &aException)
	{
		std::cout << "## Exception: " << aException.what() << std::endl;
	};
	void handleString(TEventEntry &aEvent, const std::string &aString)
	{
		if (aString.compare("string command") == 0)
			std::cout << "OK received string " << aEvent.getEventName() << " " << aString << std::endl;
		else
			std::cout << "## received string " << aEvent.getEventName() << " " << aString << std::endl;
	};
	void handleStreamCreate(TEventEntry &aEvent, const std::string &aStreamName, std::ostream* &aStream)
	{
		if (aStreamName.compare("a stream name") == 0)
			std::cout << "OK received stream create " << aEvent.getEventName() << " " << aStreamName << std::endl;
		else
			std::cout << "## received stream create " << aEvent.getEventName() << " " << aStreamName << std::endl;
		aStream = new std::ofstream("c:/temp/out.cpp.dmp", std::ios_base::binary | std::ios_base::out | std::ios_base::trunc);
	};
	void handleStreamEnd(TEventEntry &aEvent, const std::string &aStreamName, std::ostream* &aStream, bool aCancel)
	{
		if (aStreamName == "a stream name" && !aCancel)
			std::cout << "OK received stream end " << aEvent.getEventName() << " " << aStreamName << " " << aCancel << std::endl;
		else
			std::cout << "## received stream end " << aEvent.getEventName() << " " << aStreamName << " " << aCancel << std::endl;
		//aStream->flush();
	};
};




int _tmain(int argc, _TCHAR* argv[])
{
	TSocketConnection connection("C++ test client");

	std::cout << "connected" << std::endl;

	std::cout << "private event name: " << connection.getPrivateEventName() << std::endl;
	std::cout << "monitor event name: " << connection.getMonitorEventName() << std::endl;

	TEventHandlers eventHandlers;
	
	// link event handlers on connection
	__hook(&TConnection::onDisconnect, &connection, &TEventHandlers::handleDisconnect, &eventHandlers);
	__hook(&TConnection::onException, &connection, &TEventHandlers::handleException, &eventHandlers);
	
	// subscribe
	TEventEntry* eventEntry = connection.subscribe("test event");

	// link event handlers on event entry
	__hook(&TEventEntry::onString, eventEntry, &TEventHandlers::handleString, &eventHandlers);
	__hook(&TEventEntry::onStreamCreate, eventEntry, &TEventHandlers::handleStreamCreate, &eventHandlers);
	__hook(&TEventEntry::onStreamEnd, eventEntry, &TEventHandlers::handleStreamEnd, &eventHandlers);
	
	std::cout << "Ready to receive events./nPress return to signal events.." << std::endl;
	getchar();

	eventEntry->signalString("string command");
	
	std::ifstream stream;
	stream.open("c:/temp/test.jpg", std::ios_base::binary | std::ios_base::in);
	eventEntry->signalStream("a stream name", stream);
	stream.close();
	
	std::cout << "Press return to finish.." << std::endl;
	getchar();

}
