using DoAn2VADT.Database.Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
namespace DoAn2VADT.Database.Entities
{
    public class OrderDetail : BaseEntity
    {
        public decimal? Price { get; set; }
        public decimal? Discount { get; set; }
        public int? Quanlity { get; set; }
        public int? OrderId { get; set; }
        public int? BookId { get; set; }
        public Order? Order { get; set; }
        public Book? Book { get; set; }

    }
}
