using DoAn2VADT.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoAn2VADT.Database.Entities
{
    public class OrderDetailConfig : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.HasOne(x => x.Order)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.OrderId)
                 .OnDelete(DeleteBehavior.NoAction); ;
            builder.HasOne(x => x.Book)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x=>x.BookId)
                 .OnDelete(DeleteBehavior.NoAction); ;
        }
    }
}
