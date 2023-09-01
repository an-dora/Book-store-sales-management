using DoAn2VADT.Database.Entities;
using DoAn2VADT.Database;
using Microsoft.AspNetCore.Mvc;
using PagedList.Core;
using Microsoft.EntityFrameworkCore;
using DoAn2VADT.Helpper;
using Microsoft.AspNetCore.Mvc.Rendering;
using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;

namespace DoAn2VADT.Controllers
{
    public class ThongKeTonKhoController : Controller
    {
        private readonly AppDbContext _context;
        public INotyfService _notyfService { get; }

        public ThongKeTonKhoController(AppDbContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Orders
        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 8;
            var lsBook = _context.Books
                .AsNoTracking()
                .OrderByDescending(x => x.Id);
            PagedList<Book> models = new PagedList<Book>(lsBook, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }
        [HttpPost]
        public IActionResult Index(DateTime from_date, DateTime to_date)
        {
            using (_context)
            {
                ViewBag.GetBills = (from b in _context.Books where b.CreatedAt >= from_date && b.CreatedAt <= to_date == true select b).ToList();
                ViewBag.GetQuantityOrder = (from b in _context.Books where b.CreatedAt >= from_date && b.CreatedAt <= to_date == true select b.Id).Count();
                return View();
            }
        }
        public IActionResult Filtter(int CatID = 0)
        {
            var url = $"/Book?CatID={CatID}";
            if (CatID == 0)
            {
                url = $"/Book";
            }
            return Json(new { status = "success", redirectUrl = url });
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Include(b => b.Distributor)
                .Include(b => b.Publisher)
                .Include(b => b.Title)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Name", book.AuthorId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", book.CategoryId);
            ViewData["DistributorId"] = new SelectList(_context.Distributors, "Id", "Name", book.DistributorId);
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Name", book.PublisherId);
            ViewData["TitleId"] = new SelectList(_context.Titles, "Id", "Name", book.TitleId);
            return View(book);
        }

        // POST: Admin/Book/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Description,Image,Price,Discount,DistributorId,PublisherId,AuthorId,TitleId,CategoryId,Id,Name,CreatedAt,UpdatedAt,DeletedAt,UpdateUserId,CreateUserId,Amont")] Book book, Microsoft.AspNetCore.Http.IFormFile fThumb)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                book.Name = Utilities.ToTitleCase(book.Name);
                if (fThumb != null)
                {
                    string extension = Path.GetExtension(fThumb.FileName);
                    string image = Utilities.SEOUrl(book.Name) + extension;
                    book.Image = await Utilities.UploadFile(fThumb, @"book", image.ToLower());
                }
                if (string.IsNullOrEmpty(book.Name)) book.Image = "default.jpg";
                if (System.IO.File.Exists(book.Name))
                {
                    System.IO.File.Delete(book.Name);
                }
                book.UpdatedAt = DateTime.Now;
                _context.Update(book);
                await _context.SaveChangesAsync();
                _notyfService.Success("Update mới thành công");
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Name", book.AuthorId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", book.CategoryId);
            ViewData["DistributorId"] = new SelectList(_context.Distributors, "Id", "Name", book.DistributorId);
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Name", book.PublisherId);
            ViewData["TitleId"] = new SelectList(_context.Titles, "Id", "Name", book.TitleId);
            return View(book);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Include(b => b.Distributor)
                .Include(b => b.Publisher)
                .Include(b => b.Title)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, Microsoft.AspNetCore.Http.IFormFile fThumb)
        {
            if (_context.Books == null)
            {
                return Problem("Entity set 'AppDbContext.Books'  is null.");
            }
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                book.Name = Utilities.ToTitleCase(book.Name);
                if (fThumb != null)
                {
                    string extension = Path.GetExtension(fThumb.FileName);
                    string image = Utilities.SEOUrl(book.Name) + extension;
                    book.Image = await Utilities.UploadFile(fThumb, @"book", image.ToLower());
                }
                if (string.IsNullOrEmpty(book.Name)) book.Image = "default.jpg";
                if (System.IO.File.Exists(book.Name))
                {
                    System.IO.File.Delete(book.Name);
                }
                _context.Books.Remove(book);
            }

            await _context.SaveChangesAsync();
            _notyfService.Success("Xóa thành công");
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}
