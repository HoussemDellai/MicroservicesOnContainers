using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Frontend.Mvc;

namespace Frontend.Mvc.Models
{
    public class FrontendMvcContext : DbContext
    {
        public FrontendMvcContext (DbContextOptions<FrontendMvcContext> options)
            : base(options)
        {
        }

        public DbSet<Frontend.Mvc.Product> Product { get; set; }

        public DbSet<Frontend.Mvc.BasketItem> BasketItem { get; set; }
    }
}
