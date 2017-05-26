using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshopify.Entities
{
    public class Order
    {
        public string id { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string closed_at { get; set; } = string.Empty;
        public string created_at { get; set; } = string.Empty;
        public string updated_at { get; set; } = string.Empty;
        public string number { get; set; } = string.Empty;
        public string order_number { get; set; } = string.Empty;
        public List<Line_Item> line_items { get; set; } = new List<Line_Item>();
        public string currency { get; set; } = string.Empty;
        public Address billing_address { get; set; } = new Address();
        public Customer customer { get; set; } = new Customer();
        public Address shipping_address { get; set; } = new Address();
        public List<Shippin_Line> shipping_lines { get; set; } = new List<Shippin_Line>();
        public string financial_status { get; set; } = string.Empty;
        public string fulfillment_status { get; set; } = string.Empty;
        public string note { get; set; } = string.Empty;
        public string subtotal_price { get; set; } = string.Empty;
        public string total_price { get; set; } = string.Empty;
        public List<Discount_Code> discount_codes { get; set; } = new List<Discount_Code>();
        public List<Tax_Line> tax_lines { get; set; } = new List<Tax_Line>();
    }
}
