using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Reflection;
using System.Text;
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

        private void btUpdateJob_Click(object sender, EventArgs e)
        {
            try
            {
                UriBuilder url = new UriBuilder(tbWSServer.Text);
                url.Path = "DDOProtocol/CREATEEDITSESSION";

                //Use Api Util to generate the proper guid format
                String jobGuid = WSRestApiUtil.NewGuid();
                String instanceGuid = WSRestApiUtil.NewGuid();

                JObject reqObj = new JObject();
                reqObj["SessionType"] = "TEditJobHeaderSession";
                reqObj["jobGUID"] = jobGuid;
                reqObj["InstanceGUID"] = instanceGuid;


                String ser = JsonConvert.SerializeObject(reqObj, Formatting.Indented);

                ser = HttpUtil.Http(url.ToString(), tbUserName.Text, tbPassword.Text, ser, AddLine);

                JObject resObj = JsonConvert.DeserializeObject<JObject>(ser);
                //Format the json to be "pretty"

                //Lookup the session id.  We cannot rely on case.
                String sessionID = resObj.GetValue("SessionID", StringComparison.OrdinalIgnoreCase)?.Value<string>();

                reqObj = new JObject();
                reqObj["SessionID"] = sessionID;
                reqObj["CustomGroupType"] = "JOB";
                reqObj["RecalculateFieldProperties"] = true;
                reqObj["RecalculateTabVisibility"] = true;
                reqObj["OnChangeFieldName"] = sessionID;
                JObject jobData = new JObject();
                reqObj["JobData"] = jobData;

                jobData["JobGuid"] = jobGuid;
                jobData["WO"] = "Test: "+DateTime.Now.ToString();
                jobData["JobTitle"] = "Job Title";
                jobData["JobType"] = "Electrical";
                jobData["JobStatus"] = "A";

                ser = JsonConvert.SerializeObject(reqObj, Formatting.Indented);
                url.Path = "DDOProtocol/EDITSESSIONJOBVISUALFIELDUPDATE";
                ser = HttpUtil.Http(url.ToString(), tbUserName.Text, tbPassword.Text, ser, AddLine);
                resObj = JsonConvert.DeserializeObject<JObject>(ser);
                //Format the json to be "pretty"




                reqObj = new JObject();
                reqObj["SessionID"] = sessionID;

                ser = JsonConvert.SerializeObject(reqObj, Formatting.Indented);
                url.Path = "DDOProtocol/SAVEEDITSESSIONJOB";
                ser = HttpUtil.Http(url.ToString(), tbUserName.Text, tbPassword.Text, ser, AddLine);
                resObj = JsonConvert.DeserializeObject<JObject>(ser);
                //Format the json to be "pretty"


                reqObj = new JObject();
                reqObj["SessionID"] = sessionID;

                ser = JsonConvert.SerializeObject(reqObj, Formatting.Indented);
                url.Path = "DDOProtocol/CLOSEEDITSESSION";
                ser = HttpUtil.Http(url.ToString(), tbUserName.Text, tbPassword.Text, ser, AddLine);
                resObj = JsonConvert.DeserializeObject<JObject>(ser);
                //Format the json to be "pretty"


            }
            catch (Exception exp)
            {
                AddLine(exp.Message);
            }
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

        private void btUpdateUnit_Click(object sender, EventArgs e)
        {

        }
    }

    public class Pref
    {
        public String WSServer { get; set; } = "";
        public String UserName { get; set; } = "";
        public String Password { get; set; } = "";
    }

}
