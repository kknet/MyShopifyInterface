//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace myshopify
{
    using System;
    using System.Collections.Generic;
    
    public partial class ordenes
    {
        public long order_id { get; set; }
        public Nullable<System.DateTime> datFechaEnviada { get; set; }
        public string tienda_id { get; set; }
    
        public virtual tiendas tiendas { get; set; }
    }
}
