using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoAn2VADT.Controllers
{
    [Authorize]

    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
