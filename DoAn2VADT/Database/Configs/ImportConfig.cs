using DoAn2VADT.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoAn2VADT.Database.Entities
{
    public class ImportConfig : IEntityTypeConfiguration<Import>
    {
        public void Configure(EntityTypeBuilder<Import> builder)
        {
            builder.HasOne(x => x.Account)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x=>x.CreateUserId)
                 .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
