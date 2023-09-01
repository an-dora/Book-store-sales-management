using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace DoAn2VADT.Database.Entities.Base
{
	public class BaseEntity
    {
		public int Id { get; set; }
        [Required(ErrorMessage = "{0} là bắt buộc")]
        [DisplayName("Tên")]
        public string? Name { get; set; }
		public DateTime? CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public DateTime? DeletedAt { get; set; }
		public int? UpdateUserId { get; set; }
		public int? CreateUserId { get; set; }
	}
}
