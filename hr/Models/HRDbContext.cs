using Microsoft.EntityFrameworkCore;
using hr.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace hr.Models
{
    public class HRDbContext: IdentityDbContext
    {
        public HRDbContext(DbContextOptions<HRDbContext> options):base(options)
        {

        }

        public DbSet<Contacts> Contacts { get; set; }

        public DbSet<events>? events { get; set; }

        public DbSet<Discounts>? Discounts { get; set; }

        public DbSet<Courses>? Courses { get; set; }

        public DbSet<News>? News { get; set; }

        public DbSet<Honoring>? Honoring { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Publications>? Publications { get; set; }
        public DbSet<HomePage> HomePage { get; set; }
        public DbSet<Videos>? Videos { get; set; }
    }
}
