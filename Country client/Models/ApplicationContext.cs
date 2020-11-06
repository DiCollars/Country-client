using Client.Models;
using Microsoft.EntityFrameworkCore;

namespace Country_client.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Region> Regions { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
