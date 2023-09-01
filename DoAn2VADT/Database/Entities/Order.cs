using DoAn2VADT.Database.Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace DoAn2VADT.Database.Entities
{
    public class Order : BaseEntity
    {
        public decimal? Total { get; set; }
        public Account? Account { get; set; }
    }
}
