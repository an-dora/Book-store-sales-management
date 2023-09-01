using DoAn2VADT.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoAn2VADT.Database.Entities
{
    public class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasOne(x => x.Account)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x=>x.CreateUserId)
                 .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
