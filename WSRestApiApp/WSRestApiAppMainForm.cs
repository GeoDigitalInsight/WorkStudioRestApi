using GeoDigital.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
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
            UriBuilder url = new UriBuilder(tbWSServer.Text);
            url.Path = "DDOProtocol/CREATEEDITSESSION";
            sb.AppendLine(url.Host);
            sb.AppendLine(url.ToString());

            //Use Api Util to generate the proper guid
            String jobGuid = WSRestApiUtil.NewGuid();
            String instanceGuid = WSRestApiUtil.NewGuid(); 

            JObject jsObj = new JObject();
            jsObj["SessionType"] = "TEditJobHeaderSession";
            jsObj["jobGUID"] = jobGuid;
            jsObj["InstanceGUID"] = instanceGuid;


            String ser = JsonConvert.SerializeObject(jsObj, Formatting.Indented);
            sb.AppendLine(ser);

            ser = HttpUtil.Http(url.ToString(), tbUserName.Text, tbPassword.Text, ser);

            jsObj = JsonConvert.DeserializeObject<JObject>(ser);

            //Format the json to be "pretty"
            sb.AppendLine(JsonConvert.SerializeObject(jsObj, Formatting.Indented));

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

    public class HttpUtil
    {
        //Will be a post if postPayload is not blank otherwise it will be a get
        public static String Http(String sUrl, String sUsername, String sPassword, String postPayload)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            try
            {
                //Build the web request that will act as a "web service client"
                HttpWebRequest loHttp = (HttpWebRequest)WebRequest.Create(sUrl);
                loHttp.Timeout = 5 * 60 * 1000;
                loHttp.Credentials = new NetworkCredential(sUsername, sPassword);

                if (postPayload != "")
                {
                    // set request body
                    loHttp.ContentType = "application/json";
                    loHttp.Method = "POST";
                    using (StreamWriter writer = new StreamWriter(loHttp.GetRequestStream())) writer.Write(postPayload);
                }
                else
                {
                    loHttp.Method = "GET";
                }

                ServicePointManager.ServerCertificateValidationCallback += delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                };

                //Get the response and load it into a buffer stream
                HttpWebResponse loWebResponse = (HttpWebResponse)loHttp.GetResponse();

                //Copy the contents asynchronously for fastest performance
                MemoryStream memSt = new MemoryStream();
                AsyncStreamCopier aCopy = new AsyncStreamCopier(loWebResponse.GetResponseStream(), memSt);
                aCopy.StartAndWaitForCompletion();

                //Send the response conent back to the caller
                loWebResponse.Close();
                return ASCIIEncoding.ASCII.GetString(memSt.ToArray());
            }
            catch (Exception exp)
            {
                return exp.Message;
            }
        }
    }
}
