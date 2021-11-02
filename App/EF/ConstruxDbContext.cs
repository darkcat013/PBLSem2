using App.Domain.Entities;
using App.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.EF
{
    public class ConstruxDbContext : IdentityDbContext<User, Role, int, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        public ConstruxDbContext(DbContextOptions<ConstruxDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Bookmark> Bookmarks { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Representative> Representatives { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(ConstruxDbContext).Assembly);

            builder.Entity<User>().ToTable("AspNetUsers");
            builder.Entity<UserClaim>().ToTable("AspNetUserClaims");
            builder.Entity<UserLogin>().ToTable("AspNetUserLogins");
            builder.Entity<UserToken>().ToTable("AspNetUserTokens");
            builder.Entity<Role>().ToTable("AspNetRoles");
            builder.Entity<RoleClaim>().ToTable("AspNetRoleClaims");
            builder.Entity<UserRole>().ToTable("AspNetUserRoles");
        }
    }
}
