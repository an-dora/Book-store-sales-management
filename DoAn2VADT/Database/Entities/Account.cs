using DoAn2VADT.Database.Entities.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace DoAn2VADT.Database.Entities
{
    public class Account : BaseEntity
    {
        [Required(ErrorMessage = "{0} là bắt buộc")]
        [DisplayName("Mật khẩu")]
        public string? Password { get; set; }
        public int? Role { get; set; }
    }
}
