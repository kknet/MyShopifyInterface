using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshopify.Entities
{
    public class ZINSTAX_MODEL
    {
        public string ORDID { get; set; } = string.Empty; // - ID de de Orden
        public string IDTRA { get; set; } = string.Empty; //- ID del Trabajo
        public string FEORD { get; set; } = string.Empty; //- Fecha de la Orden(formato dd.mm.aaaa)
        public string HORDE { get; set; } = string.Empty; //- Hora de la Orden(formato hh:mm:ss)
        public string TIEND { get; set; } = string.Empty; //- Tienda
        public string NUMTI { get; set; } = string.Empty; //- Número de la tienda
        public string MATNR { get; set; } = string.Empty; //- Número de material
        public string ARKTX { get; set; } = string.Empty; //- Texto del material
        public string PREUN { get; set; } = string.Empty; //- Precio Unitario
        public string UNIVE { get; set; } = string.Empty; //- Unidades Vendidas
        public string TAIMP { get; set; } = string.Empty; //- Tasa de Impuesto
        public string MONVE { get; set; } = string.Empty; //- Monto de Venta
        public string REFPA { get; set; } = string.Empty; //- Referencia de Pago
        public string NOMCT { get; set; } = string.Empty; //- Nombre del Cliente
        public string STREET { get; set; } = string.Empty; //- Calle de facturación
        public string HOUSE_NUM1 { get; set; } = string.Empty; //- Número exterior de facturación
        public string HOUSE_NUM2 { get; set; } = string.Empty; //- Número interior de facturación
        public string CITY2 { get; set; } = string.Empty; //- Colonia de facturación
        public string POST_CODE1 { get; set; } = string.Empty; //- Código Postal de facturación
        public string CITY1 { get; set; } = string.Empty; //- Población de facturación
        public string REGION { get; set; } = string.Empty; //- Clave de la entidad federativa de facturación
        public string COUNTRY { get; set; } = string.Empty; //- Clave de país de facturación
        public string EMAIL { get; set; } = string.Empty; //- Correo electrónico
        public string TEL_NUMBER { get; set; } = string.Empty; //- Teléfono
        public string STREETD { get; set; } = string.Empty; //- Calle de entrega
        public string HOUSE_NUM1D { get; set; } = string.Empty; //- Número Exterior de entrega
        public string HOUSE_NUM2D { get; set; } = string.Empty; //- Número Interior de entrega
        public string CITY2D { get; set; } = string.Empty; //- Población de entrega
        public string POST_CODE2D { get; set; } = string.Empty; //- Código Postal de entrega
        public string CITY1D { get; set; } = string.Empty; //- Población de entrega
        public string REGIOND { get; set; } = string.Empty; //- Clave de la entidad federativa de entrega
        public string COUNTRYD { get; set; } = string.Empty; //- Clave de país de entrega
    }
}
