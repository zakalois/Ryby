using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Ryby_a_ulovky.Models;

namespace Ryby_a_ulovky.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Ryba> Ryby { get; set; }
        public DbSet<Lokalita> Lokality { get; set; }
        public DbSet<Ulovek> Ulovky { get; set; }
        public DbSet<Aktualita> Aktuality { get; set; }
    }
}