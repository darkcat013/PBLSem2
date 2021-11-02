using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.EF.Configurations
{
    public class RepresentativeConfiguration : IEntityTypeConfiguration<Representative>
    {
        public void Configure(EntityTypeBuilder<Representative> builder)
        {
            builder.HasOne(r => r.User)
                .WithOne(u => u.Representative)
                .HasForeignKey<Representative>(r => r.UserId);

            builder.HasOne(r => r.Company)
                .WithOne(c => c.Representative)
                .HasForeignKey<Representative>(r => r.CompanyId);
        }
    }
}
