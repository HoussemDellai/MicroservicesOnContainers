using Microsoft.AspNetCore.Mvc;

namespace Frontend.Mvc
{
    [IgnoreAntiforgeryToken(Order = 1001)]
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }
    }
}
