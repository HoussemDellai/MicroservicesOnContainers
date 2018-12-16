using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Models
{
    public class CatalogContext : DbContext
    {
        public CatalogContext (DbContextOptions<CatalogContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Product> Product { get; set; }
    }
}
