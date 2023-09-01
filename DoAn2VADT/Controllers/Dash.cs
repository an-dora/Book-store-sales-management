using Microsoft.AspNetCore.Mvc;

namespace DoAn2VADT.Controllers
{
    public class Dash : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
