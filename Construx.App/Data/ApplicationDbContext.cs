using Construx.App.Domain.Entities;
using Construx.App.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Construx.App.Data
{
    public class ApplicationDbContext : IdentityDbContext<User,IdentityRole<int>,int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<BookmarkService> BookmarkServices { get; set; }
        public DbSet<BookmarkCompany> BookmarkCompanies { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Representative> Representatives { get; set; }
        public DbSet<CompanyStatus> CompanyStatuses { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<PlanPart> PlanParts { get; set; }
        public DbSet<PlanPartStatus> PlanPartsStatuses { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

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
