using DoAn2VADT.Database.Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace DoAn2VADT.ViewModel
{
    public class Title:BaseEntity
    {
        [Display(Name = "Tiêu đề")]
        [Required(ErrorMessage = "Vui lòng nhập title")]
        [MinLength(5, ErrorMessage = "Bạn cần nhập title tối thiểu 5 ký tự")]
        public string? Description { get; set; }
    }
}
