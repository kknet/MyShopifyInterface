using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshopify.Entities
{
    public class transactions_response
    {
        public List<Transaction> transactions { get; set; } = new List<Transaction>();
    }
}
