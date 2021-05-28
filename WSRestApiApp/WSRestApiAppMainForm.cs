using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WSRestApiApp
{
    public partial class WSRestApiAppMainForm : Form
    {
        delegate void ThreadProcType();
        delegate void ThreadProcCaller(ThreadProcType AProc);
        private void ThreadProc(ThreadProcType AProc)
        {
            if (InvokeRequired)
            {
                ThreadProcCaller d = new ThreadProcCaller(ThreadProc);
                Invoke(d, new object[] { AProc });
            }
            else
            {
                AProc();
            }

        }

        public void AddLine(String msg)
        {
            ThreadProc(
                delegate ()
                {
                    textBox1.AppendText(msg + Environment.NewLine);
                });
        }

        public WSRestApiAppMainForm()
        {
            InitializeComponent();
        }
        public static string PrefPath
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.Combine(Path.ChangeExtension(path, ".json"));
            }
        }

        private void UpdateUnit(UriBuilder url, String userName, String passWord)
        {
            JObject resObj;
            JObject reqObj;



            url.Path = "DDOProtocol/GETUNITLIST";
            reqObj = new JObject();
            resObj = HttpUtil.Http(url.ToString(), userName, passWord, reqObj, AddLine);

            JArray jsObj = WSRestApiUtil.GetJSONValue<JArray>(resObj, "UnitList");

            foreach (JToken jt in jsObj)
            {
                JObject en = jt as JObject;
                String unitName = WSRestApiUtil.GetJSONValue<String>(en, "Unit");
                AddLine(unitName);

                //If we found the unit then populate a new reqObj with the unit
                if (unitName == "WL2")
                {
                    AddLine("Found it");
                    break;
                }
            }

        }

        //Routine that gives a flow of how to update a job and deal with anomolies that can occur with the job
        private void UpdateJob(UriBuilder url, String userName, String passWord)
        {
            JObject resObj;
            JObject reqObj;
            //Use Api Util to generate the proper guid format
            String jobGuid = WSRestApiUtil.NewGuid();
            String instanceGuid = WSRestApiUtil.NewGuid();

            //Create a new edit session around the job.  On the WorkStudio Server multiple edit sessions
            //can exist at the same time.  The edit session provides a means of only allowing one edit 
            //to a job at a time.
            url.Path = "DDOProtocol/CREATEEDITSESSION";
            reqObj = new JObject();
            //Required field
            reqObj["SessionType"] = "TEditJobHeaderSession";
            //Required field
            reqObj["jobGUID"] = jobGuid;
            //Required field
            reqObj["InstanceGUID"] = instanceGuid;
            resObj = HttpUtil.Http(url.ToString(), userName, passWord, reqObj, AddLine);

            //Lookup the edit session id. The session id will be used to interact with the 
            //edit session and job.
            //Note: WorkStudio does not always gaurantee that case of properties will be honored.
            String sessionID = WSRestApiUtil.GetJSONValue<string>(resObj, "SessionID");

            //Update the data contained inside of the job in memory on the server.  This call is used to update 
            //each field that is updated it also will call all of the needed OnChange() scripts needing to be fired.  
            //Fields that do not need to be updated can be excluded from this call.
            reqObj = new JObject();
            //Required field
            reqObj["SessionID"] = sessionID;
            //Required field
            reqObj["InstanceGUID"] = instanceGuid;
            //Required field: The type of data we are updating.
            reqObj["CustomGroupType"] = "JOB";
            //Required field: Used when there is a UI that controls how an end user interacts with the data in fields.
            //The recaculation will determine the visibility, ability to be editted, etc. of each field.  This is 
            //most likely not needed to be true when a user interface is not involved on the calling end.
            reqObj["RecalculateFieldProperties"] = true;
            //Required field:  Used when there is a UI that shows the visibility of tabs for the data.
            //This is most likely not needed to be true when a user interface is not involved on the 
            //calling end.
            reqObj["RecalculateTabVisibility"] = true;
            //Required field:  The name of the field being updated that is needing an OnChange script to be 
            //fired.  An example would be a field that is used to populate another field that is changed
            //when the OnChange script is fired.
            reqObj["OnChangeFieldName"] = sessionID;
            //Required field: A JSON object that will contain the fields that are looking to be updated in the specified
            //WorkStudio Job.  Fields in the first level are what have been known as "Static Fields".  These fields will
            //exist for all instances of WorkStudio for all GeoDigital customers.
            JObject jobData = new JObject();
            reqObj["JobData"] = jobData;
            //Required if this is a new job.
            //Optional if this is an existing job.  Existing value will be maintained if omitted.
            jobData["JobGuid"] = jobGuid;
            //Required if this is a new job.
            //Optional if this is an existing job.  Existing value will be maintained if omitted.
            jobData["WO"] = "Test: " + DateTime.Now.ToString();
            //Required if this is a new job.
            //Optional if this is an existing job.  Existing value will be maintained if omitted.
            jobData["JobTitle"] = "Job Title";
            //Required if this is a new job.
            //Optional if this is an existing job.  Existing value will be maintained if omitted.
            jobData["JobType"] = "Electrical";
            //Required if this is a new job.
            //NOT USED if this is an existing job.  Use the job transition protocols in order to change
            //job status on existing jobs.
            jobData["JobStatus"] = "A";
            //Optional field.  This object contains all of the custom fields that are particular to the 
            //WorkStudio instance.  Static fields are the same for every customer and custom fields
            //can be different for every customer depending on their workflow and use cases for the software.
            //This field might be optional, however, it is most likely thing thing that will be used most
            //in this API.
            JObject customData = new JObject();
            jobData["CustomData"] = customData;
            //Optional field.  The field that is desired to be updated.  Omitting any fields
            //will result in the field being maintained.
            customData["MyField"] = "hello world";
            url.Path = "DDOProtocol/EDITSESSIONJOBVISUALFIELDUPDATE";
            resObj = HttpUtil.Http(url.ToString(), userName, passWord, reqObj, AddLine);

            //Save the job that is referenced and held in memory for the edit session to the databases.  
            //Simply calling EDITSESSIONJOBVISUALFIELDUPDATE only makes changes to the job in memory.
            //The job is actually written to the database by calling SAVEEDITSESSIONJOB.
            reqObj = new JObject();
            //Required field
            reqObj["SessionID"] = sessionID;
            //Required field
            reqObj["InstanceGUID"] = instanceGuid;
            url.Path = "DDOProtocol/SAVEEDITSESSIONJOB";
            resObj = HttpUtil.Http(url.ToString(), userName, passWord, reqObj, AddLine);

            //Cleanup the edit session that was created in a prior step.  This call will close the edit session
            //if there are no other holders of the particular edit session.  If there is another 
            //edit session, it will be held open until closed by the other instance.  WorkStudio Server
            //will handle these details.  The caller is just responsible for calling CLOSEEDITSESSION.
            reqObj = new JObject();
            //Required field
            reqObj["SessionID"] = sessionID;
            //Required field
            reqObj["InstanceGUID"] = instanceGuid;
            url.Path = "DDOProtocol/CLOSEEDITSESSION";
            resObj = HttpUtil.Http(url.ToString(), userName, passWord, reqObj, AddLine);
        }

        //Routine that gives a flow of how to execute a custom UI Action in WorkStudio
        //In this example, a command named PickListSavePicklistInfo with a TDataObj in/out
        //parameter named Data needs to be setup in WorkStudio
        //A UIAction named PickListSavePicklistInfo needs to be attached to the command
        //mentioned above.
        //The following is an example of the script that can be used to the command
        //to access the payload being sent up via the EXECUTEUIACTION: 
        //function OnExecute: String;
        //var
        //  lData: TDataObj;
        //  lTable: TDDOTable;
        //  lFieldCount: Integer;
        //begin
        //  Result:='';
        //  lData:=ScriptObjects.FindObjectByName('Data');
        //  lData.WriteToFile('c:\debug\mytest.ddo');
        //  //Standard TDataObj manipulation can be performed here.
        //  lFieldCount:=lData.AsFrame.NewSlot('PickList').AsFrame.NewSlost('Heading').AsArray.Count;
        //end;
        private void ExecuteUIAction(UriBuilder url, String userName, String passWord)
        {
            url.Path = "DDOProtocol/EXECUTEUIACTION";
            JObject reqObj = JsonConvert.DeserializeObject<JObject>(Resources.EXECUTEUIACTION);
            JObject resObj = HttpUtil.Http(url.ToString(), userName, passWord, reqObj, AddLine);

            if (String.Equals(WSRestApiUtil.GetJSONValue<String>(resObj, "Protocol"), "OK", StringComparison.CurrentCultureIgnoreCase))
            {
                //Todo: Show how to get a value back that was modified by the command on the server 

            }
            else
            {
                //An error occurred
            }
        }

        private void Execute(object sender, EventArgs e)
        {
            String userName = tbUserName.Text;
            String password = tbPassword.Text;
            UriBuilder url = new UriBuilder(tbWSServer.Text);
            Thread thd = new Thread(new ThreadStart(
                delegate
                {
                    try
                    {
                        if (sender == btUpdateJob) UpdateJob(url, userName, password);
                        if (sender == btUpdateUnit) UpdateUnit(url, userName, password);
                        if (sender == btExecuteUIAction) ExecuteUIAction(url, userName, password);
                    }
                    catch (Exception exp)
                    {
                        AddLine(exp.Message);
                    }

                    ThreadProc(
                        delegate ()
                        {
                            btUpdateJob.Enabled = true;
                            btUpdateUnit.Enabled = true;
                            btExecuteUIAction.Enabled = true;
                            btHalt.Enabled = false;
                        });
                }));

            btUpdateJob.Enabled = false;
            btUpdateUnit.Enabled = false;
            btExecuteUIAction.Enabled = false;
            btHalt.Enabled = true;
            thd.Start();
        }

        private void WSRestApiAppMainForm_Load(object sender, EventArgs e)
        {
            if (File.Exists(PrefPath))
            {
                Pref p = JsonConvert.DeserializeObject<Pref>(File.ReadAllText(PrefPath));
                tbWSServer.Text = p.WSServer;
                tbUserName.Text = p.UserName;
                tbPassword.Text = p.Password;
            }
        }

        private void WSRestApiAppMainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Pref p = new Pref();
            p.WSServer = tbWSServer.Text;
            p.UserName = tbUserName.Text;
            p.Password = tbPassword.Text;
            File.WriteAllText(PrefPath, JsonConvert.SerializeObject(p, Formatting.Indented));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.Indented;

                writer.WriteStartObject();
                writer.WritePropertyName("GeoDigitalBin");
                writer.WriteRawValue("\"\\/Base64(VGhpcyBpcyBhIHRlc3Q=)\\/\"");
                writer.WritePropertyName("PSU");
                writer.WriteValue("500W");
                writer.WritePropertyName("Drives");
                writer.WriteStartArray();
                writer.WriteValue("DVD read/writer");
                writer.WriteComment("(broken)");
                writer.WriteValue("500 gigabyte hard drive");
                writer.WriteValue(42);
                writer.WriteEnd();
                writer.WriteEndObject();
            }
            AddLine(sb.ToString());


            JObject reqObj = new JObject();
            //Required field
            reqObj["GeoDigitalBin"] = @"\/Base64(VGhpcyBpcyBhIHRlc3Q=)\/";
            reqObj["NewtonSoftBin"] = Encoding.UTF8.GetBytes("This is a test");
            JsonSerializerSettings jss = new JsonSerializerSettings();
            jss.Converters.Add(new GeoDigitalJsonConverter());
            jss.Formatting = Formatting.Indented;
            String str = JsonConvert.SerializeObject(reqObj, jss);
            //String str = JsonConvert.SerializeObject(reqObj);
            AddLine(str);

            //JObject reqObj2 = JsonConvert.DeserializeObject<JObject>(str, new GeoDigitalJsonConverter());
            //str = JsonConvert.SerializeObject(reqObj2, new GeoDigitalJsonConverter());
            //AddLine(str);

            //AddLine("***Test**");
            //String str = "{\"GeoDigitalBin\":\"\\/Base64(VGhpcyBpcyBhIHRlc3Q =)\\/\"}";
            //AddLine(str);
            //object reqObj3 = JsonConvert.DeserializeObject<JObject>(str,  new GeoDigitalJsonConverter());



            //AddLine(Convert.ToString(reqObj3["GeoDigitalBin"])); 
            //str = JsonConvert.SerializeObject(reqObj3);
            //AddLine(str);
        }
    }

    public class GeoDigitalJsonConverter : JsonConverter
    {


        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JToken t = JToken.FromObject(value);
            if (value.GetType() == typeof(byte[]))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("\"\\/Base64(");

                sb.Append(Convert.ToBase64String(value as byte[], Base64FormattingOptions.None));
                sb.Append(")\\/\"");
                writer.WriteRawValue(sb.ToString());
            }
             else if (t.Type == JTokenType.Object)
            {
                writer.WriteStartObject();
                WriteJson(writer, value, serializer);
                writer.WriteEndObject();
            }
            else
            {
                
                //base.WriteJson(writer, value, serializer);
                t.WriteTo(writer);
                /*
                JObject o = (JObject)t;
                IList<string> propertyNames = o.Properties().Select(p => p.Name).ToList();

                o.AddFirst(new JProperty("Keys", new JArray(propertyNames)));

                o.WriteTo(writer);
                */
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {

            if (reader.TokenType == JsonToken.String)
            {
                reader.Read();
                var values = new List<object>();
                while (reader.TokenType != JsonToken.EndArray)
                {
                    values.Add(reader.Value);
                    reader.Read();
                }

                reader.Read();
                source = values.ToArray();
            }


            //object source;

            ////[42, 23]
            //if (reader.TokenType == JsonToken.StartArray)
            //{
            //    reader.Read();
            //    var values = new List<object>();
            //    while (reader.TokenType != JsonToken.EndArray)
            //    {
            //        values.Add(reader.Value);
            //        reader.Read();
            //    }

            //    reader.Read();
            //    source = values.ToArray();
            //}
            //else if (reader.TokenType == JsonToken.StartObject)//{"subtrahend": 23, "minuend": 42}
            //{
            //    reader.Read();
            //    var values = new JObject();
            //    while (reader.TokenType != JsonToken.EndObject)
            //    {
            //        if (reader.TokenType != JsonToken.PropertyName)
            //            throw new FormatException("Expected a property name, got: " + reader.TokenType);
            //        string propertyName = reader.Value.ToString();
            //        reader.Read();

            //        values[propertyName] = new JValue(reader.Value);
            //        reader.Read();
            //    }

            //    source = values;
            //}
            //else
            //    throw new FormatException("Expected start of object or start of array");

            //reader.Read();
            //return source;
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanConvert(Type objectType)
        {
            //return (objectType == typeof(byte[])) || (objectType == typeof(JObject));
            return (objectType == typeof(byte[]));
        }
    }

    public class Pref
    {
        public String WSServer { get; set; } = "";
        public String UserName { get; set; } = "";
        public String Password { get; set; } = "";
    }

}
