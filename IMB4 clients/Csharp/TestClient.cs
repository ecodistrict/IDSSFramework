using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using IMB;

namespace TestClient
{
    class Program
    {
        static void showHelp()
        {
            Console.WriteLine("Options");
            Console.WriteLine("   ? for help");
            Console.WriteLine("   Q or escape to quit");
            Console.WriteLine("   S to send test events");
            Console.WriteLine();
        }

        static void Main(string[] args)
        {
            TConnection connection = new TSocketConnection("C# test model", 7);
            try
            {
                Console.WriteLine("connected");

                Console.WriteLine("private event name: "+connection.privateEventName);
                Console.WriteLine("monitor event name: "+connection.monitorEventName);

                connection.onDisconnect += (aConnection) =>
                    {
                        Console.WriteLine("disconnected..");
                    };
                connection.onException += (aConnection, aException) =>
                    {
                        Console.WriteLine("## Exception: "+aException.Message);
                    };
                
                TEventEntry eventEntry = connection.subscribe("test event");

                eventEntry.onString += (aEventEntry, aString) =>
                    {
                        if (aString.CompareTo("string command") == 0)
                            Console.WriteLine("OK received string " + aEventEntry.eventName + " " + aString);
                        else
                            Console.WriteLine("## received string " + aEventEntry.eventName + " " + aString);
                    };
                eventEntry.onIntString += (aEventEntry, aInt, aString) =>
                    {
                        if (aInt==1234 && aString.CompareTo("int string payload") == 0)
                            Console.WriteLine("OK received int string " + aEventEntry.eventName + " " + aInt.ToString() + " " + aString);
                        else
                            Console.WriteLine("## received int string " + aEventEntry.eventName + " " + aInt.ToString() + " " + aString);
                    };
                eventEntry.onChangeObject += (aEventEntry, aAction, aObjectID, aAttribute) =>
                    {
                        if (aAction==TEventEntry.actionChange && aObjectID==2345 && aAttribute.CompareTo("an attribute") == 0)
                        Console.WriteLine("OK received change object " + aEventEntry.eventName + " " + aAction.ToString() + " " + aObjectID.ToString() + " " + aAttribute);
                    else
                        Console.WriteLine("## received change object " + aEventEntry.eventName + " " + aAction.ToString() + " " + aObjectID.ToString() + " " + aAttribute);
                    };
                eventEntry.onStreamCreate += (aEventEntry, aStreamName) =>
                    {
                        if (aStreamName == "a stream name")
                            Console.WriteLine("OK received stream create " + aEventEntry.eventName + " " + aStreamName);
                        else
                            Console.WriteLine("## received stream create " + aEventEntry.eventName + " " + aStreamName);
                        return File.Create("out.cscharp.dmp");
                    };
                eventEntry.onStreamEnd += (aEventEntry, aStreamName, aStream, aCancel) =>
                {
                    if (aStreamName == "a stream name" && !aCancel)
                        Console.WriteLine("OK received stream end " + aEventEntry.eventName + " " + aStreamName + " " + aCancel.ToString());
                    else
                        Console.WriteLine("## received stream end " + aEventEntry.eventName + " " + aStreamName + " " + aCancel.ToString());
                };

                showHelp();

                ConsoleKeyInfo key;
                do
                {
                    key = Console.ReadKey();
                    switch (key.KeyChar)
                    {
                        case 's':
                        case 'S':
                            eventEntry.signalString("string command");
                            eventEntry.signalIntString(1234, "int string payload");
                              FileStream stream = File.OpenRead("test.jpg"); // todo: use path of existing file 
                              try
                              {
                                eventEntry.signalStream("a stream name", stream);
                              }
                              finally
                              {
                                stream.Close();
                              }
                              eventEntry.signalChangeObject(TEventEntry.actionChange, 2345, "an attribute");
                              Console.WriteLine("sent events..");
                            break;
                        case '?':
                            showHelp();
                            break;
                    }
                }
                while (key.KeyChar != 'q' && key.KeyChar != 'Q' && key.KeyChar != (char)27);
            }
            finally
            {
                connection.close();
            }
        }
    }
}
