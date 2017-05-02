using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ShopifyTracking
{
    class Program
    {
        static void Main(string[] args)
        {
            string Response = POST(@"https://instax-mexico.myshopify.com/admin/orders/5145744705/fulfillments.json");
            Console.WriteLine(Response);
            Console.ReadKey();
        }
        public static string GET(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["Api_username"].ToString(), ConfigurationManager.AppSettings["Api_password"].ToString());
            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    string errorText = reader.ReadToEnd();
                }
                throw;
            }
        }
        /// <summary>
        /// Allows to send post messages to shopify
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string POST(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["Api_username"].ToString(), ConfigurationManager.AppSettings["Api_password"].ToString());
            try
            {
                request.Method = "POST";
                request.ServicePoint.Expect100Continue = false;
                request.Timeout = 20000;
                request.ContentType = "application/json";
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    string json = new JavaScriptSerializer().Serialize(new
                    {
                        fulfillment = new
                        {
                            tracking_number = "123456789010",
                            tracking_url = "http://fedex.com",
                            notify_customer = true
                        }
                    });
                    streamWriter.Write(json);
                    //string json = "{\"fulfillment\":\"{\"tracking_number\":\"123456789010\",\"tracking_url\":\"www.google.com\",\"tracking_company\":\"FedEx\",\"notify_customer\":\"true\"}\"}";
                    //streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                
                var httpResponse = (HttpWebResponse)request.GetResponse();
                string result;
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                return result;
            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    string errorText = reader.ReadToEnd();
                    return errorText;
                }
            }
        }
    }
}
