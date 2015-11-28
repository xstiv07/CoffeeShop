using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RestBucksAPIWrapper
{
    public static class BaseClass
    {
        public static string CallApi(String apiUrl, string requestMethod, string requestXML = null)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
            request.ContentType = "application/xml";
            request.Accept = "application/json";
            request.Method = requestMethod;

            if (requestMethod == "POST" || requestMethod == "PUT")
            {
                byte[] bytes;
                bytes = Encoding.ASCII.GetBytes(requestXML);
                request.ContentLength = bytes.Length;
                request.Timeout = Timeout.Infinite;
                request.KeepAlive = true;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
            };

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            StreamReader sr = new StreamReader(response.GetResponseStream());
            String temp = sr.ReadToEnd();
            return temp;
        }
    }
}
