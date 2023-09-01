using DoAn2VADT.Database.Entities.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace DoAn2VADT.Database.Entities
{
    public class ImportDetail : BaseEntity
    {
        public decimal? Price { get; set; }
        public decimal? Discount { get; set; }
        public int? Quanlity { get; set; }
        public int? ImportId { get; set; }
        public Import? Import { get; set; }
    }
}
