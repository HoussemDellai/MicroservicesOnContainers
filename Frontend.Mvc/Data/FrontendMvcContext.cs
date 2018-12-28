using Microsoft.EntityFrameworkCore;

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
