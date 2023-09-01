using AspNetCoreHero.ToastNotification.Abstractions;
using DoAn2VADT.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAn2VADT.Controllers
{
    public class Profit : Controller
    {
        private readonly AppDbContext _context;
        public INotyfService _notyfService { get; }

        public Profit(AppDbContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }
        public IActionResult Index(DateTime from_date, DateTime to_date)
        {
            using(_context)
            {
                ViewBag.GetBills = (from b in _context.Orders where b.CreatedAt >= from_date && b.CreatedAt <= to_date == true select b.Id).ToList();
                ViewBag.GetQuantityOrder = (from b in _context.Orders where b.CreatedAt >= from_date && b.CreatedAt <= to_date == true select b.Id).Count();
                ViewBag.SumToTal = (from b in _context.Orders where b.CreatedAt >= from_date && b.CreatedAt <= to_date == true select b.Total).Sum();
                return View();
            } 
            
        }
    }
}
