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
            try
            {
                FulfillmentRequest request = new FulfillmentRequest();
                if (args.Length == 1)
                {
                    request.created_at = DateTime.Now.ToString("s", System.Globalization.CultureInfo.InvariantCulture);
                    request.order_id = args[0].Split('|')[0];
                    request.status = args[0].Split('|')[1].Replace('_', ' ');
                    request.notify_customer = args[0].Split('|')[2] == "1" ? true : false;
                    //request.JobStatusId = Convert.ToInt32(args[0].Split('|')[3]);
                    request.tracking_number = args[0].Split('|')[3].Replace('_', ' ');
                    request.tracking_company = args[0].Split('|')[4].Replace('_', ' ');
                    string response = PostFulfillment(request);
                    Console.WriteLine(response);
                }
                else
                {
                    Console.WriteLine("No se asignaron los parámetros correctamente");
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
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
        public static string PostFulfillment(FulfillmentRequest Request)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://instax-mexico.myshopify.com/admin/orders/" + Request.order_id + "/fulfillments.json");
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
                            created_at = Request.created_at,
                            status = Request.status,
                            notify_customer = Request.notify_customer,
                            tracking_number = Request.tracking_number,
                            tracking_company = Request.tracking_company
                        }
                    });
                    streamWriter.Write(json);
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
