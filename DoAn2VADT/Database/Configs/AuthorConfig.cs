using DoAn2VADT.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoAn2VADT.Database.Entities
{
	public class AuthorConfig : IEntityTypeConfiguration<Author>
	{
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            
            builder.Property(x => x.Name).HasMaxLength(250).IsRequired(false);
            builder.Property(x => x.HomeTown).HasMaxLength(250).IsRequired(false);
            builder.Property(x => x.Description).HasMaxLength(250).IsRequired(false);
        }
    }
}
