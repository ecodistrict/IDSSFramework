program TestClientMinimum;

{$APPTYPE CONSOLE}

{$R *.res}

uses
  imb4,
  WinApi.Windows,
  System.Classes,
  System.SysUtils;

const
  ccEsc = #27;

function CheckKeyPressed2: Boolean;
var
  InputRecord:TInputRecord;
  NumRead:DWORD;
  Console:THandle;
begin
  Console  :=  GetStdHandle(STD_INPUT_HANDLE);
  Result  :=  False;
  while PeekConsoleInput(Console, InputRecord, 1, NumRead) and (NumRead>0) and not Result do
  begin
    if (InputRecord.EventType<>KEY_EVENT) or not InputRecord.Event.KeyEvent.bKeyDown
    then ReadConsoleInput(Console, InputRecord, 1, NumRead) // remove event from buffer by reading it; we only want key events
    else Result  :=  True;
  end;
end;

function KeyPressed2: Char;
var
  InputRecord:TInputRecord;
  Console:THandle;
  NumRead:DWORD;
begin
  Console := GetStdHandle(STD_INPUT_HANDLE);
  // init input record with all zeros
  FillChar(InputRecord, SizeOf(InputRecord), 0);
  // read from buffer, wait till one key down is provided
  repeat until ReadConsoleInput(Console, InputRecord, 1, NumRead) AND (InputRecord.EventType=KEY_EVENT) and InputRecord.Event.KeyEvent.bKeyDown;
  if (InputRecord.EventType=KEY_EVENT) and InputRecord.Event.KeyEvent.bKeyDown
  then Result  :=  InputRecord.Event.KeyEvent.UnicodeChar
  else Result  :=  #$00;
end;

procedure ShowHelp();
begin
  WriteLn('Options');
  WriteLn('   S send events');
  WriteLn('   H send heartbeat');
  WriteLn('   ? for help');
  WriteLn('   Q or escape to quit');
  WriteLn;
end;

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
  Result := TFileStream.Create('c:\temp\out.delphi-min.dmp', fmCreate);
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
  eventM: TEventEntry;
  eventD: TEventEntry;
  stream: TStream;
  key: Char;
begin
  try
    //connection := TSocketConnection.Create('delphi minimum client');
    connection := TTLSConnection.Create(
      'client-eco-district.crt', 'client-eco-district.key', '&8dh48klosaxu90OKH', 'root-ca-imb.crt',
      'delphi minimum client');
    try
      if connection.connected
      then WriteLn('connected')
      else WriteLn('## connected');

      WriteLn('private event name: ', connection.privateEventName);
      WriteLn('monitor event name: ', connection.monitorEventName);

      connection.onDisconnect := procedure(aConnection: TConnection) begin WriteLn('disconnected..') end;
      connection.onException := procedure(aConnection: TConnection; aException: Exception) begin writeLn('## exception '+aException.Message) end;


      event := connection.Subscribe('test event');
      eventM := connection.publish('ecodistrict.modules-test', false);
      eventD := connection.publish('ecodistrict.dashboard-test', false);

      event.onString.Add(HandleString);
      event.onIntString.Add(HandleIntString);
      event.onStreamCreate := HandleStreamCreate;
      event.onStreamEnd := HandleStreamEnd;
      event.onChangeObject.Add(HandleChangeObject);

      ShowHelp();
      key := #0;
      repeat
        if CheckKeyPressed2 then
        begin
          key := KeyPressed2;
          case key of
            'S', 's':
              begin
                event.signalString('string command');
                event.signalIntString(1234, 'int string payload');
//                stream := TFileStream.Create('c:\temp\test.jpg', fmOpenRead);
//                try
//                  event.SignalStream('a stream name', stream);
//                finally
//                  stream.Free;
//                end;
                event.signalChangeObject(actionChange, 2345, 'an attribute');
                eventM.signalString('string command');
                eventD.signalString('string command');
                WriteLn('sent events..');
              end;
            '?':ShowHelp();
            'h', 'H':
              begin
                // send heartbeat
                connection.signalHeartBeat();//('een heartbeat');
              end;
          end;
        end
        else
        begin
          Sleep(100);
        end;
      until (key='Q') or (key='q') or (key=ccEsc);

    finally
      connection.Free;
    end;
  except
    on E: Exception do
      Writeln(E.ClassName, ': ', E.Message);
  end;
end.
