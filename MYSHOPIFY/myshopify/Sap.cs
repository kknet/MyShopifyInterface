using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshopify
{
    public class Sap
    {
        /// <summary>
        /// Método que permite inicializar las configuraciones para consumir los RFCs
        /// </summary>
        /// <returns></returns>
        private static RfcDestination InicializarRFC()
        {
            RfcConfigParameters Parametros = new RfcConfigParameters();
            Parametros.Add(RfcConfigParameters.AppServerHost, ConfigurationManager.AppSettings["AppServerHost"].ToString());
            Parametros.Add(RfcConfigParameters.Name, ConfigurationManager.AppSettings["Name"].ToString());
            Parametros.Add(RfcConfigParameters.SystemNumber, ConfigurationManager.AppSettings["SystemNumber"].ToString());
            Parametros.Add(RfcConfigParameters.Client, ConfigurationManager.AppSettings["Client"].ToString());
            Parametros.Add(RfcConfigParameters.User, ConfigurationManager.AppSettings["User"].ToString());
            Parametros.Add(RfcConfigParameters.Password, ConfigurationManager.AppSettings["Password"].ToString());
            Parametros.Add(RfcConfigParameters.PoolSize, ConfigurationManager.AppSettings["PoolSize"].ToString());
            return RfcDestinationManager.GetDestination(Parametros);
        }
        /// <summary>
        /// Permite convertir Una tabla de SAP a un DataTable
        /// </summary>
        /// <param name="lrfcTable"></param>
        /// <returns></returns>
        private static DataTable GetDataTableFromRFCTable(IRfcTable lrfcTable)
        {
            DataTable loTable = new DataTable();
            for (int liElement = 0; liElement < lrfcTable.ElementCount; liElement++)
            {
                RfcElementMetadata metadata = lrfcTable.GetElementMetadata(liElement);
                loTable.Columns.Add(metadata.Name);
            }
            foreach (IRfcStructure row in lrfcTable)
            {
                DataRow ldr = loTable.NewRow();
                for (int liElement = 0; liElement < lrfcTable.ElementCount; liElement++)
                {
                    RfcElementMetadata metadata = lrfcTable.GetElementMetadata(liElement);
                    ldr[metadata.Name] = row.GetString(metadata.Name);
                }
                loTable.Rows.Add(ldr);
            }
            return loTable;
        }
        /// <summary>
        /// Invocación de la RFC ZINSTAX
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        public static bool ZINSTAX(Entities.ZIMAGINE modelo)
        {
            bool success = false;
            try
            {
                RfcDestination rfc = InicializarRFC();
                IRfcFunction Funcion = rfc.Repository.CreateFunction("ZINSTAX");
                Funcion.SetValue("FEORD", modelo.FEORD);
                Funcion.SetValue("HORDE", modelo.HORDE);
                Funcion.SetValue("ORDID", modelo.ORDID);
                //Funcion.SetValue("POSNR", modelo.POSNR);
                Funcion.SetValue("JOBID", modelo.JOBID);
                Funcion.SetValue("TIEND", modelo.TIEND);
                Funcion.SetValue("NUMTI", modelo.NUMTI);
                Funcion.SetValue("DESPR", modelo.DESPR);
                Funcion.SetValue("PRVPU", modelo.PRVPU);
                Funcion.SetValue("UNIVE", modelo.UNIVE);
                Funcion.SetValue("TASIM", modelo.TASIM);
                Funcion.SetValue("MONVE", modelo.MONVE);
                Funcion.SetValue("WAERK", modelo.WAERK);
                Funcion.SetValue("REFPA", modelo.REFPA);
                //Funcion.SetValue("NOMEN", modelo.NOMEN);
                Funcion.SetValue("NOMCT", modelo.NOMCT);
                Funcion.SetValue("APECT", modelo.APECT);
                Funcion.SetValue("DIREC", modelo.DIREC);
                Funcion.SetValue("DIRE1", modelo.DIRE1);
                Funcion.SetValue("SUBEN", modelo.SUBEN);
                Funcion.SetValue("EDOEN", modelo.EDOEN);
                Funcion.SetValue("PSTLZ", modelo.PSTLZ);
                Funcion.SetValue("PAISE", modelo.PAISE);
                Funcion.SetValue("MAILC", modelo.MAILC);
                Funcion.SetValue("TELNU", modelo.TELNU);
                Funcion.SetValue("MATNR", modelo.MATNR);
                Funcion.Invoke(rfc);

                success = Funcion.GetString("bande") == "1" ? true : false;
            }
            catch(Exception exc)
            {
                Console.WriteLine("Error en el consumo de la RFC ZINSTAX: " + exc.Message);
            }
            return success;
        }
    }
}
