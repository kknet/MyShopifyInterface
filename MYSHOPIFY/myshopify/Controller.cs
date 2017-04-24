using myshopify.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace myshopify
{
    class Controller
    {
        public bool ExtraerRegistros(long NumeroOrden, string Opcion, ref int NumeroRegistros)
        {
            NumeroRegistros = 0;
            string json = GET(ConfigurationManager.AppSettings["UrlOrdenes"].ToString());
            Orders Response = JsonConvert.DeserializeObject<Orders>(json);
            List<Order> lista = new List<Order>();
            if(Opcion == "PENDIENTE")
                lista = (from r in (Response.orders.OrderBy(o => o.order_number).ToList()) where Convert.ToInt64(r.id) == NumeroOrden select r).ToList();
            else
                lista = (from r in (Response.orders.OrderBy(o => o.order_number).ToList()) where Convert.ToInt64(r.id) > NumeroOrden select r).ToList();
            List<ZIMAGINE> RegistrosSAP = new List<ZIMAGINE>();
            ZIMAGINE modelo;
            transactions_response _t = new transactions_response();
            int POSICION = 0;
            foreach (var orden in lista)
            {
                _t = JsonConvert.DeserializeObject<transactions_response>(GET(ConfigurationManager.AppSettings["UrlTransacciones"].ToString().Replace("xxx", orden.id)));
                POSICION = 0;
                foreach (var item in orden.line_items)
                {
                    modelo = new ZIMAGINE();
                    modelo.FEORD = Convert.ToDateTime(orden.created_at).ToString("yyyyMMdd");
                    modelo.HORDE = Convert.ToDateTime(orden.created_at).ToString("hhmmss");
                    modelo.ORDID = orden.order_number;
                    modelo.POSNR = "" + (POSICION + 10);
                    modelo.JOBID = orden.id;
                    modelo.TIEND = ConfigurationManager.AppSettings["Tienda"].ToString();
                    modelo.NUMTI = ConfigurationManager.AppSettings["NumeroTienda"].ToString();
                    modelo.DESPR = item.name;
                    modelo.PRVPU = item.price;
                    modelo.UNIVE = item.quantity;
                    modelo.TASIM = orden.tax_lines.Count > 0 ? orden.tax_lines[0].rate : "0";
                    modelo.MONVE = orden.total_price;
                    modelo.WAERK = orden.currency;
                    modelo.REFPA = _t.transactions.Count > 0 ? _t.transactions[0].authorization : "";
                    modelo.NOMEN = orden.customer.first_name + " " + orden.customer.last_name;
                    modelo.NOMCT = orden.customer.first_name;
                    modelo.APECT = orden.customer.last_name;
                    modelo.DIREC = orden.shipping_address.address1;
                    modelo.DIRE1 = orden.shipping_address.address2;
                    modelo.SUBEN = orden.shipping_address.city;
                    modelo.EDOEN = orden.shipping_address.province;
                    modelo.PSTLZ = orden.shipping_address.zip;
                    modelo.PAISE = orden.shipping_address.country;
                    modelo.MAILC = orden.customer.email;
                    modelo.TELNU = orden.shipping_address.phone;
                    modelo.MATNR = item.sku;
                    if (modelo.MONVE != string.Empty)
                        if (Convert.ToDecimal(modelo.MONVE) > 0)
                            RegistrosSAP.Add(modelo);
                }
                foreach (var item in orden.shipping_lines)
                {
                    modelo = new ZIMAGINE();
                    modelo.FEORD = Convert.ToDateTime(orden.created_at).ToString("yyyyMMdd");
                    modelo.HORDE = Convert.ToDateTime(orden.created_at).ToString("hhmmss");
                    modelo.ORDID = orden.order_number;
                    modelo.POSNR = "" + (POSICION + 10);
                    modelo.JOBID = orden.id;
                    modelo.TIEND = ConfigurationManager.AppSettings["Tienda"].ToString();
                    modelo.NUMTI = ConfigurationManager.AppSettings["NumeroTienda"].ToString();

                    modelo.DESPR = "N/A";
                    modelo.PRVPU = item.price;
                    modelo.UNIVE = "1";

                    modelo.TASIM = orden.tax_lines.Count > 0 ? orden.tax_lines[0].rate : "0";
                    modelo.MONVE = orden.total_price;
                    modelo.WAERK = orden.currency;
                    modelo.REFPA = _t.transactions.Count > 0 ? _t.transactions[0].authorization : "";
                    modelo.NOMEN = orden.customer.first_name + " " + orden.customer.last_name;
                    modelo.NOMCT = orden.customer.first_name;
                    modelo.APECT = orden.customer.last_name;
                    modelo.DIREC = orden.shipping_address.address1;
                    modelo.DIRE1 = orden.shipping_address.address2;
                    modelo.SUBEN = orden.shipping_address.city;
                    modelo.EDOEN = orden.shipping_address.province;
                    modelo.PSTLZ = orden.shipping_address.zip;
                    modelo.PAISE = orden.shipping_address.country;
                    modelo.MAILC = orden.customer.email;
                    modelo.TELNU = orden.shipping_address.phone;
                    modelo.MATNR = "949930010";
                    if (modelo.MONVE != string.Empty)
                        if (Convert.ToDecimal(modelo.MONVE) > 0)
                            RegistrosSAP.Add(modelo);
                }
            }
            foreach (var registro in RegistrosSAP)
            {
                if (!Sap.ZINSTAX(registro))
                {
                    throw new Exception("Error al intentar insertar la orden: " + registro.ORDID + ", El proceso se detuvo en la orden mencionada.");
                }
                else
                {
                    Console.WriteLine("registro insertado");
                    NumeroRegistros++;
                }
            }
            return true;
        }
        /// <summary>
        /// Permite hacer un get al API de shopify
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
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
    }
}
