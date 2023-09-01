using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace DoAn2VADT.ViewModel
{
    public class LoginViewModel
    {
        [Key]
        [MaxLength(100)]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        [Display(Name = "UserName")]
        [EmailAddress(ErrorMessage = "Sai định dạng {0}")]
        public string UserName { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [MinLength(5, ErrorMessage = "Bạn cần đặt mật khẩu tối thiểu 5 ký tự")]
        public string Password { get; set; }
    }
}
