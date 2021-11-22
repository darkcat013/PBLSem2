using Construx.App.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Construx.App.Data.Configurations
{

    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {

            builder.HasOne(r => r.Representative)
                .WithOne(c => c.Company)
                .HasForeignKey<Company>(r => r.RepresentativeId);

            }
    }
}

