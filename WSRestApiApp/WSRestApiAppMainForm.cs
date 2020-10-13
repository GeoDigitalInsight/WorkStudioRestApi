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

        private void btPerformTest_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                UriBuilder url = new UriBuilder(tbWSServer.Text);
                url.Path = "DDOProtocol/CREATEEDITSESSION";
                sb.AppendLine(url.Host);
                sb.AppendLine(url.ToString());

                //Use Api Util to generate the proper guid
                String jobGuid = WSRestApiUtil.NewGuid();
                String instanceGuid = WSRestApiUtil.NewGuid();

                JObject reqObj = new JObject();
                reqObj["SessionType"] = "TEditJobHeaderSession";
                reqObj["jobGUID"] = jobGuid;
                reqObj["InstanceGUID"] = instanceGuid;


                String ser = JsonConvert.SerializeObject(reqObj, Formatting.Indented);
                sb.AppendLine(ser);

                ser = HttpUtil.Http(url.ToString(), tbUserName.Text, tbPassword.Text, ser);

                JObject resObj = JsonConvert.DeserializeObject<JObject>(ser);
                //Format the json to be "pretty"
                sb.AppendLine(JsonConvert.SerializeObject(resObj, Formatting.Indented));

                //Lookup the session id.
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
                sb.AppendLine(ser);


            }
            catch (Exception exp)
            {
                sb.Append(exp.Message);
            }
            textBox1.Text = sb.ToString();
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
    }

    public class Pref
    {
        public String WSServer { get; set; } = "";
        public String UserName { get; set; } = "";
        public String Password { get; set; } = "";
    }

}
