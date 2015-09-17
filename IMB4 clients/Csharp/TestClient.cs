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
            Console.WriteLine(": Options");
            Console.WriteLine("   ? for help");
            Console.WriteLine("   Q or escape to quit");
            Console.WriteLine("   S to send test events");
			Console.WriteLine("   h enable heartbeat");
			Console.WriteLine("   H disable heartbeat");
            Console.WriteLine();
        }

        static void Main(string[] args)
        {
            TConnection connection = new TTLSConnection("client-eco-district.pfx", "&8dh48klosaxu90OKH", "root-ca-imb.crt", true, "C# test model");
            try
            {
                Console.WriteLine("connected");

                Console.WriteLine("private event name: " + connection.privateEventName);
                Console.WriteLine("monitor event name: " + connection.monitorEventName);

                // connect an event handler for a disconnect
                connection.onDisconnect += (aConnection) =>
                    {
                        Console.WriteLine("disconnected..");
                    };

                // connect an event handler for an exception in processing events and commands
                connection.onException += (aConnection, aException) =>
                    {
                        Console.WriteLine("## Exception: " + aException.Message);
                    };

                // subscribe to an event
                TEventEntry eventEntry = connection.subscribe("test event");

                // add an event handler for string events
                eventEntry.onString += (aEventEntry, aString) =>
                    {
                        if (aString.CompareTo("string command") == 0)
                            Console.WriteLine("OK received string " + aEventEntry.eventName + " " + aString);
                        else
                            Console.WriteLine("## received string " + aEventEntry.eventName + " " + aString);
                    };

                // add an event handler for stream create events
                eventEntry.onStreamCreate += (aEventEntry, aStreamName) =>
                    {
                        if (aStreamName == "a stream name")
                            Console.WriteLine("OK received stream create " + aEventEntry.eventName + " " + aStreamName);
                        else
                            Console.WriteLine("## received stream create " + aEventEntry.eventName + " " + aStreamName);
                        return File.Create("out.cscharp.dmp");
                    };

                // add an event handler for stream end events
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
                            // send a basic string event
                            eventEntry.signalString(": string command");

                            // send a stream
                            FileStream stream = File.OpenRead("test.jpg"); // todo: use path of existing file 
                            try
                            {
                                eventEntry.signalStream("a stream name", stream);
                            }
                            finally
                            {
                                stream.Close();
                            }

                            Console.WriteLine(": sent events..");
                            break;
                        case 'h':
							connection.setHeartBeat(1000); // enable heartbeat every second
							Console.WriteLine(": heartbeat enabled..");
							break;
						case 'H':
							connection.setHeartBeat(0); // disable heartbeat (<=0)
							Console.WriteLine(": heartbeat disabled..");
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
