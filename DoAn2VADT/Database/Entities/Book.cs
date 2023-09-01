using DoAn2VADT.Database.Entities.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace DoAn2VADT.Database.Entities
{
    public class Book : BaseEntity
    {
       
        public string? Description { get; set; }
        public string? Image { get; set; }
        [DisplayName("Giá bán")]
        public int? Price { get; set; }
        public int? Discount { get; set; }
        [DisplayName("Số lượng")]
        public int? Quantity { get; set; }
        [DisplayName("Nhà phát hành")]
        public int? DistributorId { get; set; }

        [DisplayName("Nhà xuất bản")]
        public int? PublisherId { get; set; }
        [DisplayName("Tác giả")]
        public int? AuthorId { get; set; }
        [DisplayName("Đầu sách")]
        public int? TitleId { get; set; }
       
        public Title? Title { get; set; }
        public Distributor? Distributor { get; set; }
        public Publisher? Publisher { get; set; }
        public Author? Author { get; set; }
        public Category? Category { get; set; }
        [DisplayName("Đầu sách")]
        public int? CategoryId { get; set; }
    }
}
