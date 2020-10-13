using GeoDigital.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace WSRestApiApp
{
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
