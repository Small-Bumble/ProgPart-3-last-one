using ContractMonthlyClaimSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ContractMonthlyClaimSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Claim> Claims { get; set; }
    }
}