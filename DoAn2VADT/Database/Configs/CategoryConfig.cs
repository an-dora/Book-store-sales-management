using DoAn2VADT.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoAn2VADT.Database.Entities
{
    public class CategoryConfig : IEntityTypeConfiguration<Distributor>
    {
        public void Configure(EntityTypeBuilder<Distributor> builder)
        {
            builder.Property(x => x.Name).HasMaxLength(250).IsRequired(false);
            builder.Property(x => x.Address).HasMaxLength(250).IsRequired(false);
            builder.Property(x => x.Email).HasMaxLength(250).IsRequired(false);
            builder.Property(x => x.Discount).HasMaxLength(3).IsRequired(false);
        }
    }
}
