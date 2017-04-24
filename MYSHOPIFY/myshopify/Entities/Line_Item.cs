using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshopify.Entities
{
    public class Line_Item
    {
        public string sku { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string price { get; set; } = string.Empty;
        public string quantity { get; set; } = string.Empty;
    }
}
