using DoAn2VADT.Database.Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace DoAn2VADT.Database.Entities
{
    public class Title : BaseEntity
    {
        public string? Description { get; set; }
    }
}
