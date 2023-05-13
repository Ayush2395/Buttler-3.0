using Buttler.Domain.Entities;
using Buttler.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Buttler.Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Foods> Foods { get; set; }
        public DbSet<OrderMaster> OrderMasters { get; set; }
        public DbSet<OrderItems> OrderItems { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Tables> Tables { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
