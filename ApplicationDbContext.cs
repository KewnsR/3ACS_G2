using Microsoft.EntityFrameworkCore;
using HumanRepProj.Models; // Ensure this namespace is correct

namespace HumanRepProj.Data // Ensure this namespace is correct
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> Logins { get; set; }
    }
}
