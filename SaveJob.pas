const
{$INCLUDE 'Included.pas'}

function ExcludeTrailingUriDelimiter(AUri: String): String;
begin
  Result:=AUri;
  while (Length(Result)>0) and (Copy(Result, Length(Result), 1)='/') do
  begin
    Result:=Copy(Result, 1, Length(Result)-1);

  end;
end;

function Exp(APath: String): String;
begin
  Result:=APath;
  Result:=StringReplace(Result, '$(HostUri)', ExcludeTrailingUriDelimiter(cUrl), 3);

  //Recurse if there are more replacements
  if (not SameText(APath, Result)) and (Pos('$(', Result)>0) then Result:=Exp(Result);
end;

procedure RunScript;
var
  lRestObj: TDataObj;
  lRespCont, lRespType: String;
  lSessionID: String;
  lJobGuid: String;
  lInstanceGuid: String;
begin
  lJobGuid:=Util.CreateGuid;//'{34DF6757-3D3C-4758-8B19-B123BB8063D}'
  lInstanceGuid:=Util.CreateGuid;
  lRestObj:=TDataObj.Create;
  try
//{
//  "Protocol": "CREATEEDITSESSION",
//  "SessionType": "TEditJobHeaderSession",
//  "jobGUID": "{06734E8A-D485-4A93-8067-8843D13F9786}",
//  "InstanceGUID": "test"
//}


    lRestObj.AsFrame.NewSlot('SessionType').AsString:='TEditJobHeaderSession';
    lRestObj.AsFrame.NewSlot('jobGUID').AsSymbol:=lJobGuid;
    lRestObj.AsFrame.NewSlot('InstanceGUID').AsString:=lInstanceGuid;
    lRespType:=''; lRespCont:='';
    Util.HttpRequestAuthPost_ToString(Exp('$(HostUri)/ddoprotocol/CREATEEDITSESSION'), cUserName, cPassword, 'application/json', lRestObj.PrintToJSONReadable, lRespType, lRespCont);
    Util.AddMessageForEachLine(lRespCont, abStatus);
    lRestObj.Clear;
    lRestObj.JSON:=lRespCont;
    Util.AddMessageForEachLine(lRestObj.PrintToJSONReadable, abStatus);
    lSessionID:=lRestObj.AsFrame.NewSlot('SessionID').AsString;
    Util.AddMessage(lSessionID, abStatus);

    lRestObj.Clear;
    //lRestObj.AsFrame.NewSlot('JobGUID').AsString := '{06734E8A-D485-4A93-8067-8843D13F9786}';
    lRestObj.AsFrame.NewSlot('SessionID').AsString := lSessionID;
    lRestObj.AsFrame.NewSlot('CustomGroupType').AsString := 'JOB';
    lRestObj.AsFrame.NewSlot('RecalculateFieldProperties').AsBoolean := true;
    lRestObj.AsFrame.NewSlot('RecalculateTabVisibility').AsBoolean := true
    lRestObj.AsFrame.NewSlot('OnChangeFieldName').AsString:='wo';
    with lRestObj.AsFrame.NewSlot('JobData').AsFrame do
    begin
      NewSlot('JobGuid').AsString:=lJobGuid;
      NewSlot('WO').AsString:='Rick Test: '+FormatDateTime('yyyymmdd.hhnnss.zzz', Now);
      NewSlot('JobTitle').AsString:='Job Title';
      NewSlot('JobType').AsString:='Electrical';    //Required
      NewSlot('JobStatus').AsString:='A';           //Required
    end;
    Util.HttpRequestAuthPost_ToString(Exp('$(HostUri)/ddoprotocol/EDITSESSIONJOBVISUALFIELDUPDATE'), cUserName, cPassword, 'application/json', lRestObj.PrintToJSONReadable, lRespType, lRespCont);
    Util.AddMessageForEachLine(lRespCont, abStatus);


    lRestObj.Clear;
    lRestObj.AsFrame.NewSlot('SessionID').AsString:=lSessionID;
    lRespType:=''; lRespCont:='';
    Util.HttpRequestAuthPost_ToString(Exp('$(HostUri)/ddoprotocol/SAVEEDITSESSIONJOB'), cUserName, cPassword, 'application/json', lRestObj.PrintToJSONReadable, lRespType, lRespCont);
    Util.AddMessageForEachLine(lRespCont, abStatus);

    lRestObj.Clear;
    lRestObj.AsFrame.NewSlot('SessionID').AsString:=lSessionID;
    lRespType:=''; lRespCont:='';
    Util.HttpRequestAuthPost_ToString(Exp('$(HostUri)/ddoprotocol/CLOSEEDITSESSION'), cUserName, cPassword, 'application/json', lRestObj.PrintToJSONReadable, lRespType, lRespCont);
    Util.AddMessageForEachLine(lRespCont, abStatus);
  finally
    lRestObj.Free;
  end;
end;

procedure OnHaltScript;
begin
  Util.AddMessage('Script has been halted', abError);
end;