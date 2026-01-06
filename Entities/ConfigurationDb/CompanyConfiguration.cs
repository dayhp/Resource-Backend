using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.ConfigurationDb
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.HasData(
                new Company
                {
                    Id = Guid.NewGuid(),
                    Name = "Tech Solutions Ltd.",
                    Address = "123 Tech Avenue, Silicon Valley, CA",
                    Country = "USA"
                },
                new Company
                {
                    Id = Guid.NewGuid(),
                    Name = "Global Innovations Inc.",
                    Address = "456 Innovation Road, New York, NY",
                    Country = "USA"
                }
            );
        }
    }
}
