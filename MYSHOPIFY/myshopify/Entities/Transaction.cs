using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshopify.Entities
{
    public class Transaction
    {
        public string id { get; set; } = string.Empty;
        public string order_id { get; set; } = string.Empty;
        public string authorization { get; set; } = string.Empty;
    }
}
