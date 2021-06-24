using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Drawing.Imaging;
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
        //In this example, a command named TestUIAction with a TDataObj in/out
        //parameter named Data needs to be setup in WorkStudio
        //A UIAction named TestUIAction needs to be attached to the command
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

            JObject resObj;
            JObject reqObj;

            reqObj = new JObject();
            //The name of the UIAction to execute.  See the ReadMe.md file to see better
            //definition around how to set this up.
            reqObj["UIActionName"] = "TestUIAction";
            JArray actParams = new JArray();
            //The parameters array.
            reqObj["Parameters"] = actParams;
            JObject actParam;
            actParam = new JObject();
            actParams.Add(actParam);
            //Set to TObjectParameter
            actParam["_ClassName"] = "TObjectParameter";
            //The name of the object that shows up in the script.  See the ReadMe.md file to see better
            //definition around how to set this up.
            actParam["ParamName"] = "Data";
            //Leave blank
            actParam["Description"] = "";
            //Set to false
            actParam["Required"] = false;
            //Set to false
            actParam["UserParam"] = false;
            actParam["ParameterType"] = "ptInput";
            //A TDataObj type tells WorkStudio server to use an "object type" instance.  TDataObj is the most flexible
            //and would be recommended for integrations.
            actParam["ObjectClass"] = "TDataObj";

            JObject dataPayload = new JObject();
            //The paramvalue is the payload that is whatever is desired to be sent to the WorkStudio Action script to be manipulated.
            actParam["ParamValue"] = dataPayload;

            dataPayload["attachguid"] = "{2664EA5B-D36C-4E47-AF4B-DF93C6EB87E9}";
            dataPayload["filename"] = "TransformerSheet.pdf";
            dataPayload["bloburi"] = "sftp://server.com/directory/{2664EA5B-D36C-4E47-AF4B-DF93C6EB87E9}";

            //Demonstrate how we could get a binary payload (image, pdf, word document, etc.) to the WorkStudio server to be able to be 
            //manipulated from a script.  We have included GeoDigitalLog.png as a resource in the project to demonstrate this.  The same
            //logic could easily be modified to directly use a file on a hard drive.
            using (MemoryStream mem = new MemoryStream())
            {
                Resources.GeoDigitalLogo.Save(mem, ImageFormat.Png);
                mem.Seek(0, SeekOrigin.Begin);
                String base64Str = Convert.ToBase64String(mem.ToArray());
                dataPayload["LogoPngBase64"] = base64Str;
            }



            resObj = HttpUtil.Http(url.ToString(), userName, passWord, reqObj, AddLine);

            if (String.Equals(WSRestApiUtil.GetJSONValue<String>(resObj, "Protocol"), "OK", StringComparison.CurrentCultureIgnoreCase))
            {
                //Todo: Show how to get a value back that was modified by the command on the server 

            }
            else
            {
                //An error occurred
                AddLine($"An error occured while attempting to execute the UIAction: {WSRestApiUtil.GetJSONValue<String>(resObj, "errorMessage")}");
            }
        }

        private void Execute(object sender, EventArgs e)
        {
            String userName = tbUserName.Text;
            String password = tbPassword.Text;
            UriBuilder url = new UriBuilder(tbUri.Text);
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
                tbUri.Text = p.WSServer;
                tbUserName.Text = p.UserName;
                tbPassword.Text = p.Password;
            }
            WSUriTextChanged(null, null);
        }

        private void WSRestApiAppMainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Pref p = new Pref();
            p.WSServer = tbUri.Text;
            p.UserName = tbUserName.Text;
            p.Password = tbPassword.Text;
            File.WriteAllText(PrefPath, JsonConvert.SerializeObject(p, Formatting.Indented));
        }

        private void SchemeHostPortTextChanged(object sender, EventArgs e)
        {

        }

        private void WSUriTextChanged(object sender, EventArgs e)
        {
            //Parse the uri and put the proper values int he scheme, host, port boxes
            tbScheme.TextChanged -= SchemeHostPortTextChanged;
            tbHost.TextChanged -= SchemeHostPortTextChanged;
            tbPort.TextChanged -= SchemeHostPortTextChanged;
            try
            {
                UriBuilder uri = new UriBuilder(tbUri.Text);
                tbScheme.Text = uri.Scheme;
                tbHost.Text = uri.Host;
                tbPort.Text = Convert.ToString(uri.Port);
            }
            finally
            {
                tbScheme.TextChanged += SchemeHostPortTextChanged;
                tbHost.TextChanged += SchemeHostPortTextChanged;
                tbPort.TextChanged += SchemeHostPortTextChanged;
            }
        }

    }

    public class Pref
    {
        public String WSServer { get; set; } = "";
        public String UserName { get; set; } = "";
        public String Password { get; set; } = "";
    }

}
