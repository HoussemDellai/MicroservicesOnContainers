using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Basket.Api.Models
{
    public class BasketContext : DbContext
    {
        public BasketContext (DbContextOptions<BasketContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<BasketItem> BasketItem { get; set; }
    }
}
