using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshopify.Entities
{
    public class Address
    {
        public string name { get; set; } = string.Empty;
        public string address1 { get; set; } = string.Empty;
        public string address2 { get; set; } = string.Empty;
        public string province { get; set; } = string.Empty;
        public string zip { get; set; } = string.Empty;
        public string country { get; set; } = string.Empty;
        public string city { get; set; } = string.Empty;
        public string phone { get; set; } = string.Empty;
    }
}
