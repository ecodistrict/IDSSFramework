program TestClientMinimum;

{$APPTYPE CONSOLE}

{$R *.res}

uses
  imb4,
  System.Classes,
  System.SysUtils;

procedure HandleString(aEventEntry: TEventEntry; const aString: string);
begin
  if aString='string command'
  then WriteLn('OK received string '+aEventEntry.EventName+' '+aString)
  else WriteLn('## received string '+aEventEntry.EventName+' '+aString);
end;

function HandleStreamCreate(aEventEntry: TEventEntry; const aName: string): TStream;
begin
  if aName='a stream name'
  then WriteLn('OK received stream create '+aEventEntry.EventName+' '+aName)
  else WriteLn('## received stream create '+aEventEntry.EventName+' '+aName);
  Result := TFileStream.Create('out.delphi-min.dmp', fmCreate);
end;

procedure HandleStreamEnd(aEventEntry: TEventEntry; const aName: string; var aStream: TStream; aCancel: Boolean);
begin
  if (aName='a stream name') and not aCancel
  then WriteLn('OK received stream end '+aEventEntry.EventName+' '+aName+' '+aCancel.ToString)
  else WriteLn('## received stream end '+aEventEntry.EventName+' '+aName+' '+aCancel.ToString);
end;

var
  connection: TConnection;
  event: TEventEntry;
  stream: TStream;
begin
  try
    connection := TTLSConnection.Create(
      'client-eco-district.crt', 'client-eco-district.key', '&8dh48klosaxu90OKH', 'root-ca-imb.crt',
      'delphi minimum client');
    try
      WriteLn('connected');

      WriteLn('private event name: ', connection.privateEventName);
      WriteLn('monitor event name: ', connection.monitorEventName);

	  // add event handlers for a disconnect or an exception during processing of events or commands
      connection.onDisconnect := procedure(aConnection: TConnection) begin WriteLn('disconnected..') end;
      connection.onException := procedure(aConnection: TConnection; aException: Exception) begin writeLn('## exception '+aException.Message) end;

	  // subscribe to an event
      event := connection.Subscribe('test event');

	  // add an event handler for string events
      event.onString.Add(HandleString);
      
	  // add an event handlers for handling a stream create and stream end
	  event.onStreamCreate := HandleStreamCreate;
      event.onStreamEnd := HandleStreamEnd;

	  // send a basic string event
      event.signalString('string command');
      
	  // send a stream
	  stream := TFileStream.Create('test.jpg', fmOpenRead); // todo: put in file name (with path) of existing file
      try
        event.SignalStream('a stream name', stream);
      finally
        stream.Free;
      end;

      WriteLn('sent events..');

      ReadLn;

    finally
      connection.Free;
    end;
  except
    on E: Exception do
      Writeln(E.ClassName, ': ', E.Message);
  end;
end.
