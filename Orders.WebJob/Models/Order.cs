using System.Collections.Generic;

namespace Orders.WebJob.Models
{
    public class Order
    {
        public Order()
        {
            ProductsId = new List<int>();
        }

        public int Id { get; set; }
        
        public List<int> ProductsId { get; set; }

        public double TotalPrice { get; set; }
    }
}
