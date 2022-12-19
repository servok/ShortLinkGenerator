using Microsoft.EntityFrameworkCore;

namespace ApiShortLinkGenerator.Models
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<Link> Links { get; set; }
    }
}
