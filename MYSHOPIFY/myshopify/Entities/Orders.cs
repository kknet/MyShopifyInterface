using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshopify.Entities
{
    public class Orders
    {
        public List<Order> orders { get; set; } = new List<Order>();
    }
}
