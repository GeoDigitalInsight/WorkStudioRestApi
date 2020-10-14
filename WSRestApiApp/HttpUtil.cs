using GeoDigital.IO;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace WSRestApiApp
{
    public delegate void AddMessageCallback(String msg);
    public class HttpUtil
    {
        //Will be a post if postPayload is not blank otherwise it will be a get
        public static String Http(String sUrl, String sUsername, String sPassword, String postPayload, AddMessageCallback AMessageCallback)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            try
            {
                //Build the web request that will act as a "web service client"
                AMessageCallback(sUrl);
                HttpWebRequest loHttp = (HttpWebRequest)WebRequest.Create(sUrl);
                loHttp.Timeout = 5 * 60 * 1000;
                loHttp.Credentials = new NetworkCredential(sUsername, sPassword);

                if (postPayload != "")
                {
                    // set request body
                    loHttp.ContentType = "application/json";
                    loHttp.Method = "POST";
                    AMessageCallback(postPayload);
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
                String retVal = ASCIIEncoding.ASCII.GetString(memSt.ToArray());
                AMessageCallback(retVal);
                return retVal;
            }
            catch (WebException exp)
            {
                throw new Exception(String.Format("Error processing request: {0}, Response: {1}", exp.Message, GetResponseContent(exp.Response)));
            }
        }

        public static string GetResponseContent(WebResponse resp)
        {
            if (resp == null) return "";
            using (StreamReader reader = new StreamReader(resp.GetResponseStream())) return reader.ReadToEnd();
        }
    }
}
