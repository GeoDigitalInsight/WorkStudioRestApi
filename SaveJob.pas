const
{$INCLUDE 'Included.pas'}
procedure RunScript;
var
  lRestObj: TDataObj;
  lRespCont, lRespType: String;
  lSessionID: String;
begin
  lRestObj:=TDataObj.Create;
  try
//{
//  "Protocol": "CREATEEDITSESSION",
//  "SessionType": "TEditJobHeaderSession",
//  "jobGUID": "{06734E8A-D485-4A93-8067-8843D13F9786}",
//  "InstanceGUID": "test"
//}


    lRestObj.AsFrame.NewSlot('PROTOCOL').AsSymbol:='CREATEEDITSESSION';
    lRestObj.AsFrame.NewSlot('SessionType').AsString:='TEditJobHeaderSession';
    lRestObj.AsFrame.NewSlot('jobGUID').AsSymbol:='{06734E8A-D485-4A93-8067-8843D13F9786}';
    lRestObj.AsFrame.NewSlot('InstanceGUID').AsString:='{06734E8A-D485-4A93-8067-8843D13F9786}';
    lRespType:=''; lRespCont:='';
    Util.HttpRequestAuthPost_ToString(cUrl, cUserName, cPassword, 'application/json', lRestObj.PrintToJSONReadable, lRespType, lRespCont);
    Util.AddMessageForEachLine(lRespCont, abStatus);
    lRestObj.Clear;
    lRestObj.JSON:=lRespCont;
    Util.AddMessageForEachLine(lRestObj.PrintToJSONReadable, abStatus);
    lSessionID:=lRestObj.AsFrame.NewSlot('SessionID').AsString;
    Util.AddMessage(lSessionID, abStatus);
    lRestObj.Clear;
    lRestObj.AsFrame.NewSlot('PROTOCOL').AsSymbol:='CLOSEEDITSESSION';
    lRestObj.AsFrame.NewSlot('SessionID').AsString:=lSessionID;
//    lRestObj.AsFrame.NewSlot('SessionType').AsString:='TEditJobHeaderSession';
//    lRestObj.AsFrame.NewSlot('jobGUID').AsSymbol:='{06734E8A-D485-4A93-8067-8843D13F9786}';
//    lRestObj.AsFrame.NewSlot('InstanceGUID').AsString:='{06734E8A-D485-4A93-8067-8843D13F9786}';
    lRespType:=''; lRespCont:='';
    Util.HttpRequestAuthPost_ToString(cUrl, cUserName, cPassword, 'application/json', lRestObj.PrintToJSONReadable, lRespType, lRespCont);
    Util.AddMessageForEachLine(lRespCont, abStatus);
  finally
    lRestObj.Free;
  end;
end;

procedure OnHaltScript;
begin
  Util.AddMessage('Script has been halted', abError);
end;