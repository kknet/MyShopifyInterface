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
            using (myshopifyInterfaceEntities db = new myshopifyInterfaceEntities())
            {
                var Tiendas = (from t in db.tiendas where t.bitActiva select t).ToList();
                foreach(var tienda in Tiendas)
                {
                    string json = GET(tienda.vchUrlOrdenes, tienda.vchUsername, tienda.vchPassword);
                    Orders Response = JsonConvert.DeserializeObject<Orders>(json);
                    List<Order> lista = new List<Order>();
                    if (Opcion == "PENDIENTE")
                    {
                        lista = (from r in (Response.orders.OrderBy(o => o.name).ToList()) where Convert.ToInt64(r.name) == NumeroOrden select r).ToList();
                        SendToSap(lista, tienda.vchUrlTransacciones, tienda.vchNombreTienda, tienda.tienda_id, tienda.vchUsername, tienda.vchPassword, ref NumeroRegistros);
                    }
                    else
                    {
                        var q = (from o in Response.orders
                                 join l in db.ordenes on new { id = Convert.ToInt64(o.id), tienda_id = tienda.tienda_id } equals new { id = l.order_id,  tienda_id = l.tienda_id } into ls
                                 from l in ls.DefaultIfEmpty()
                                 where l == null
                                 select o).ToList();
                        lista = q;
                        ordenes _o;
                        DateTime now = DateTime.Now;
                        foreach (var elemento in lista)
                        {
                            _o = new ordenes();
                            _o.datFechaEnviada = now;
                            _o.order_id = Convert.ToInt64(elemento.id);
                            _o.tienda_id = tienda.tienda_id;
                            db.ordenes.Add(_o);
                        }
                        SendToSap(lista, tienda.vchUrlTransacciones, tienda.vchNombreTienda, tienda.tienda_id, tienda.vchUsername, tienda.vchPassword, ref NumeroRegistros);
                        db.SaveChanges();
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// Envia las órdenes a sap
        /// </summary>
        /// <param name="lista"></param>
        private void SendToSap(List<Order> lista, string url_transacciones, string Tienda, string NumeroTienda, string username, string password, ref int NumeroRegistros)
        {
            NumeroRegistros = 0;
            List<ZIMAGINE> RegistrosSAP = new List<ZIMAGINE>();
            ZIMAGINE modelo;
            transactions_response _t = new transactions_response();
            int POSICION = 0;
            foreach (var orden in lista)
            {
                _t = JsonConvert.DeserializeObject<transactions_response>(GET(url_transacciones.Replace("xxx", orden.id),username, password));
                POSICION = 0;
                foreach (var item in orden.line_items)
                {
                    modelo = new ZIMAGINE();
                    modelo.FEORD = Convert.ToDateTime(orden.created_at).ToString("yyyyMMdd");
                    modelo.HORDE = Convert.ToDateTime(orden.created_at).ToString("hhmmss");
                    modelo.ORDID = orden.name;
                    POSICION += 10;
                    modelo.POSNR = "" + POSICION;
                    modelo.JOBID = orden.id;
                    modelo.TIEND = Tienda;
                    modelo.NUMTI = NumeroTienda;
                    modelo.DESPR = item.name;
                    modelo.PRVPU = item.price;
                    modelo.UNIVE = item.quantity;
                    modelo.TASIM = orden.tax_lines.Count > 0 ? (Convert.ToDecimal(orden.tax_lines[0].rate) * 100).ToString() : "0";
                    modelo.MONVE = (Convert.ToDecimal(item.price) * Convert.ToDecimal(item.quantity)).ToString();
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
                    modelo.ORDID = orden.name;
                    POSICION += 10;
                    modelo.POSNR = "" + POSICION;
                    modelo.JOBID = orden.id;
                    modelo.TIEND = Tienda;
                    modelo.NUMTI = NumeroTienda;
                    modelo.DESPR = "N/A";
                    modelo.PRVPU = item.price;
                    modelo.UNIVE = "1";
                    modelo.TASIM = orden.tax_lines.Count > 0 ? (Convert.ToDecimal(orden.tax_lines[0].rate) * 100).ToString() : "0";
                    modelo.MONVE = item.price;
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
                foreach (var item in orden.discount_codes)
                {
                    modelo = new ZIMAGINE();
                    modelo.FEORD = Convert.ToDateTime(orden.created_at).ToString("yyyyMMdd");
                    modelo.HORDE = Convert.ToDateTime(orden.created_at).ToString("hhmmss");
                    modelo.ORDID = orden.name;
                    POSICION += 10;
                    modelo.POSNR = "" + POSICION;
                    modelo.JOBID = orden.id;
                    modelo.TIEND = Tienda;
                    modelo.NUMTI = NumeroTienda;
                    modelo.DESPR = item.code;
                    modelo.PRVPU = item.amount;
                    modelo.UNIVE = "1";
                    modelo.TASIM = "0";
                    modelo.MONVE = "-" + item.amount; //Monto de venta negativo
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
                    modelo.PROCO = item.code;
                    modelo.MATNR = "949950010"; //Código de material para descuentos
                    if (modelo.MONVE != string.Empty)
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
                    NumeroRegistros++;
                }
            }
        }
        /// <summary>
        /// Permite hacer un get al API de shopify
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GET(string url, string username, string password)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Credentials = new NetworkCredential(username, password);
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
