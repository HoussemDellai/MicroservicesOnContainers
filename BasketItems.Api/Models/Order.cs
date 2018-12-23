using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.Api.Models
{
    public class Order
    {
        public Order()
        {
            ProductsId = new List<int>();
        }

        public int Id { get; set; }
        
        public List<int> ProductsId { get; set; }

        //public double TotalPrice { get; set; }
    }
}
