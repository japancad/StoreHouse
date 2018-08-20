using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StoreHouse.Models;

namespace StoreHouse.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<StoreHouse.Models.Product> Product { get; set; }

        public DbSet<StoreHouse.Models.Partner> Partner { get; set; }

        public DbSet<StoreHouse.Models.Import> Import { get; set; }

        public DbSet<StoreHouse.Models.Export> Export { get; set; }

        
    }
}
