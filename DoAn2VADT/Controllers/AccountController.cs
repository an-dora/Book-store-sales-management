using AspNetCoreHero.ToastNotification.Abstractions;
using DoAn2VADT.Database;
using DoAn2VADT.Database.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DoAn2VADT.ViewModel;
using DoAn2VADT.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using LoginViewModel = DoAn2VADT.Models.LoginViewModel;
using DoAn2VADT.Extension;

namespace DoAn2VADT.Controllers
{
    [Authorize(Roles = "1")]
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        public INotyfService _notyfService { get; }
        public AccountController(AppDbContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }
        // GET: /<controller>/
        [AllowAnonymous]
        public IActionResult Login( string returnUrl,Account usr)
        {
            var u = _context.Accounts.SingleOrDefault(m => m.Name == usr.Name && m.Password == usr.Password);
            if (u != null)
            {
                List<Account> ls = new List<Account>();
                ls = _context.Accounts
                .AsNoTracking()
                .OrderBy(x => x.Id).ToList();
                HttpContext.Session.SetString("Id", u.Id.ToString());
                var taikhoanID = HttpContext.Session.GetString("Id");

                //Identity
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, u.Name),
                        new Claim("Id", u.Id.ToString())
                    };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                HttpContext.SignInAsync(claimsPrincipal);
                return RedirectToAction("Index", "Dash");
            }
            else
            {
                return View() ;
            }

        }
        public IActionResult Dashboard()
        {
            var taikhoanID = HttpContext.Session.GetString("Id");
            if (taikhoanID != null)
            {
                var khachhang = _context.Accounts.AsNoTracking().SingleOrDefault(x => x.Id == Convert.ToInt32(taikhoanID));
                if (khachhang != null)
                {
                    var lsDonHang = _context.Orders
                        .AsNoTracking()
                        .Where(x => x.Id == khachhang.Id)
                        .OrderByDescending(x => x.CreatedAt)
                        .ToList();
                    ViewBag.DonHang = lsDonHang;
                    return View(khachhang);
                }

            }
            return RedirectToAction("Login");
        }
        public async Task<IActionResult> EditAc(int? id)
        {
            var taikhoanID = HttpContext.Session.GetString("Id");
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAc(int id, [Bind("Name,Password,Role,Id,CreatedAt,UpdatedAt,DeletedAt,UpdateUserId,CreateUserId")] Account account)
        {
            var taikhoanID = HttpContext.Session.GetString("Id");
            if (id != account.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    account.UpdatedAt = DateTime.Now;
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Dash");
            }
            return View(account);
        }
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            HttpContext.Session.Remove("Id");
            return RedirectToAction("Login", "Account");
        }
        private bool AccountExists(int id)
        {
            return _context.Accounts.Any(e => e.Id == id);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
