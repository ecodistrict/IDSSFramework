program TestClientMinimum;

{$APPTYPE CONSOLE}

{$R *.res}

uses
  imb.minimum,
  System.Classes,
  System.SysUtils;

procedure HandleChangeObject(aEventEntry: TEventEntry; aAction, aObjectID: Integer; const aAttribute: string);
begin
  if (aAction=actionChange) and (aObjectID=2345) and (aAttribute='an attribute')
  then WriteLn('OK received change object '+aEventEntry.EventName+' '+aAction.ToString+' '+aObjectID.ToString+' '+aAttribute)
  else WriteLn('## received change object '+aEventEntry.EventName+' '+aAction.ToString+' '+aObjectID.ToString+' '+aAttribute);
end;

procedure HandleString(aEventEntry: TEventEntry; const aString: string);
begin
  if aString='string command'
  then WriteLn('OK received string '+aEventEntry.EventName+' '+aString)
  else WriteLn('## received string '+aEventEntry.EventName+' '+aString);
end;

procedure HandleIntString(aEventEntry: TEventEntry; aInt: Integer; const aString: string);
begin
  if (aInt=1234) and (aString='int string payload')
  then WriteLn('OK received int string '+aEventEntry.EventName+' '+aInt.ToString+' '+aString)
  else WriteLn('## received int string '+aEventEntry.EventName+' '+aInt.ToString+' '+aString);
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
    connection := TSocketConnection.Create('delphi minimum client');
    try
      WriteLn('connected');

      WriteLn('private event name: ', connection.privateEventName);
      WriteLn('monitor event name: ', connection.monitorEventName);

      connection.onDisconnect := procedure(aConnection: TConnection) begin WriteLn('disconnected..') end;
      connection.onException := procedure(aConnection: TConnection; aException: Exception) begin writeLn('## exception '+aException.Message) end;

      event := connection.Subscribe('test event');

      event.onString.Add(HandleString);
      event.onIntString.Add(HandleIntString);
      event.onStreamCreate := HandleStreamCreate;
      event.onStreamEnd := HandleStreamEnd;
      event.onChangeObject.Add(HandleChangeObject);

      event.signalString('string command');
      event.signalIntString(1234, 'int string payload');
      stream := TFileStream.Create('test.jpg', fmOpenRead); // todo: put in file name (with path) of existing file
      try
        event.SignalStream('a stream name', stream);
      finally
        stream.Free;
      end;
      event.signalChangeObject(actionChange, 2345, 'an attribute');
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
