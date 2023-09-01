using DoAn2VADT.Database;
using DoAn2VADT.Database.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAn2VADT.Controllers
{
    public class SearchController : Controller
    {
        private readonly AppDbContext _context;

        public SearchController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult FindProduct(string keyword)
        {
            List<Book> ls = new List<Book>();
            if (string.IsNullOrEmpty(keyword) || keyword.Length < 1)
            {
                return PartialView("ListProductsSearchPartial", null);
            }
            ls = _context.Books.AsNoTracking()
                                  .Include(a => a.Category)
                                  .Where(x => x.Name.Contains(keyword))
                                  .OrderByDescending(x => x.Name)
                                  .Take(10)
                                  .ToList();
            if (ls == null)
            {
                return PartialView("ListProductsSearchPartial", null);
            }
            else
            {
                return PartialView("ListProductsSearchPartial", ls);
            }
        }
        public IActionResult FindAccount(string keyword)
        {
            List<Account> ls = new List<Account>();
            if (string.IsNullOrEmpty(keyword) || keyword.Length < 1)
            {
                return PartialView("ListAccountSearchPartial", null);
            }
            ls = _context.Accounts.Where(x => x.Name.Contains(keyword))
                                  .OrderByDescending(x => x.Name)
                                  .Take(10)
                                  .ToList();
            if (ls == null)
            {
                return PartialView("ListAccountSearchPartial", null);
            }
            else
            {
                return PartialView("ListAccountSearchPartial", ls);
            }
        }
        public IActionResult FindOrder(string keyword)
        {
            List<Order> ls = new List<Order>();
            if (string.IsNullOrEmpty(keyword) || keyword.Length < 1)
            {
                return PartialView("ListOrder", null);
            }
            ls = _context.Orders.Where(x => x.Name.Contains(keyword))
                                  .OrderByDescending(x => x.Name)
                                  .Take(10)
                                  .ToList();
            if (ls == null)
            {
                return PartialView("ListOrder", null);
            }
            else
            {
                return PartialView("ListOrder", ls);
            }
        }
        public IActionResult FindAuthor(string keyword)
        {
            List<Author> ls = new List<Author>();
            if (string.IsNullOrEmpty(keyword) || keyword.Length < 1)
            {
                return PartialView("ListAuthorSeach", null);
            }
            ls = _context.Authors.Where(x => x.Name.Contains(keyword))
                                  .OrderByDescending(x => x.Name)
                                  .Take(10)
                                  .ToList();
            if (ls == null)
            {
                return PartialView("ListAuthorSeach", null);
            }
            else
            {
                return PartialView("ListAuthorSeach", ls);
            }
        }
        public IActionResult FindCate(string keyword)
        {
            List<Category> ls = new List<Category>();
            if (string.IsNullOrEmpty(keyword) || keyword.Length < 1)
            {
                return PartialView("ListCategorySaerch", null);
            }
            ls = _context.Categories.Where(x => x.Name.Contains(keyword))
                                  .OrderByDescending(x => x.Name)
                                  .Take(10)
                                  .ToList();
            if (ls == null)
            {
                return PartialView("ListCategorySaerch", null);
            }
            else
            {
                return PartialView("ListCategorySaerch", ls);
            }
        }
        public IActionResult FindKho(string keyword)
        {
            List<Book> ls = new List<Book>();
            if (string.IsNullOrEmpty(keyword) || keyword.Length < 1)
            {
                return PartialView("ListTonKho", null);
            }
            ls = _context.Books.AsNoTracking()
                                  .Include(a => a.Category)
                                  .Where(x => x.Name.Contains(keyword))
                                  .OrderByDescending(x => x.Name)
                                  .Take(10)
                                  .ToList();
            if (ls == null)
            {
                return PartialView("ListTonKho", null);
            }
            else
            {
                return PartialView("ListTonKho", ls);
            }
        }
        public IActionResult FindTitle(string keyword)
        {
            List<Title> ls = new List<Title>();
            if (string.IsNullOrEmpty(keyword) || keyword.Length < 1)
            {
                return PartialView("ListTitle", null);
            }
            ls = _context.Titles.AsNoTracking()
                                  .Where(x => x.Name.Contains(keyword))
                                  .OrderByDescending(x => x.Name)
                                  .Take(10)
                                  .ToList();
            if (ls == null)
            {
                return PartialView("ListTitle", null);
            }
            else
            {
                return PartialView("ListTitle", ls);
            }
        }
        public IActionResult FindBanHang(string keyword)
        {
            List<Book> ls = new List<Book>();
            if (string.IsNullOrEmpty(keyword) || keyword.Length < 1)
            {
                return PartialView("ListBanHang", null);
            }
            ls = _context.Books.AsNoTracking()
                                  .Include(a => a.Category)
                                  .Where(x => x.Name.Contains(keyword))
                                  .OrderByDescending(x => x.Name)
                                  .Take(10)
                                  .ToList();
            if (ls == null)
            {
                return PartialView("ListBanHang", null);
            }
            else
            {
                return PartialView("ListBanHang", ls);
            }
        }
        public IActionResult FindPubli(string keyword)
        {
            List<Publisher> ls = new List<Publisher>();
            if (string.IsNullOrEmpty(keyword) || keyword.Length < 1)
            {
                return PartialView("ListTitle", null);
            }
            ls = _context.Publishers.AsNoTracking()
                                  .Where(x => x.Name.Contains(keyword))
                                  .OrderByDescending(x => x.Name)
                                  .Take(10)
                                  .ToList();
            if (ls == null)
            {
                return PartialView("ListPublisher", null);
            }
            else
            {
                return PartialView("ListPublisher", ls);
            }
        }
        public IActionResult FindDistri(string keyword)
        {
            List<Distributor> ls = new List<Distributor>();
            if (string.IsNullOrEmpty(keyword) || keyword.Length < 1)
            {
                return PartialView("ListTitle", null);
            }
            ls = _context.Distributors.AsNoTracking()
                                  .Where(x => x.Name.Contains(keyword))
                                  .OrderByDescending(x => x.Name)
                                  .Take(10)
                                  .ToList();
            if (ls == null)
            {
                return PartialView("ListDistributor", null);
            }
            else
            {
                return PartialView("ListDistributor", ls);
            }
        }
    }
}
