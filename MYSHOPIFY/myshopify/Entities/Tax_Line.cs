using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshopify.Entities
{
    public class Tax_Line
    {
        public string title { get; set; } = string.Empty;
        public string price { get; set; } = string.Empty;
        public string rate { get; set; } = string.Empty;
    }
}
