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
                    request.Tienda = args[0].Split('|')[5];
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
        /// <summary>
        /// Allows to send post messages to shopify
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string PostFulfillment(FulfillmentRequest Request)
        {
            var urlTracking = string.Empty;
            string username = string.Empty;
            string password = string.Empty;
            using (myshopifyInterfaceEntities db = new myshopifyInterfaceEntities())
            {
                if (db.tiendas.Any(i => i.tienda_id == Request.Tienda && i.bitActiva))
                {
                    var obj = db.tiendas.First(i => i.tienda_id == Request.Tienda);
                    urlTracking = obj.vchUrlTracking;
                    username = obj.vchUsername;
                    password = obj.vchPassword;
                }
                else
                    return "No existe la tienda: " + Request.Tienda;
            }
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlTracking.Replace("xxx", Request.order_id));
            request.Credentials = new NetworkCredential(username, password);
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
