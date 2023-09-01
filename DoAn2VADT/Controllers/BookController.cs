using AspNetCoreHero.ToastNotification.Abstractions;
using ClosedXML.Excel;
using DoAn2VADT.Database;
using DoAn2VADT.Database.Entities;
using DoAn2VADT.Helpper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using System.Data;

namespace DoAn2VADT.Controllers
{
    public class BookController : Controller
    {
        private readonly AppDbContext _context;
        public INotyfService _notyfService { get; }

        public BookController(AppDbContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/Book
        public IActionResult Index(int page = 1, int CatID = 0)
        {
            var pageNumber = page;
            var pageSize = 8;

            List<Book> lsbooks = new List<Book>();
            if (CatID != 0)
            {
                lsbooks = _context.Books
                .AsNoTracking()
                .Where(x => x.CategoryId == CatID && x.PublisherId == CatID)
                .OrderBy(x => x.Name).ToList();
            }
            else
            {
                lsbooks = _context.Books
                .AsNoTracking()
                .OrderBy(x => x.Name).ToList();
            }
            PagedList<Book> models = new PagedList<Book>(lsbooks.AsQueryable(), pageNumber, pageSize);
            ViewBag.CurrentCateID = CatID;
            ViewBag.CurrentPage = pageNumber;
            ViewData["DanhMuc"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["XB"] = new SelectList(_context.Publishers, "Id", "Name");
            return View(models);
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
        // GET: Admin/Book/Details/5
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

        // GET: Admin/Book/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Name");
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["DistributorId"] = new SelectList(_context.Distributors, "Id", "Name");
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Name");
            ViewData["TitleId"] = new SelectList(_context.Titles, "Id", "Name");
            return View();
        }

        // POST: Admin/Book/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Description,Image,Price,Discount,DistributorId,PublisherId,AuthorId,TitleId,CategoryId,Id,Name,CreatedAt,UpdatedAt,DeletedAt,UpdateUserId,CreateUserId,Quantity")] Book book, Microsoft.AspNetCore.Http.IFormFile fThumb)
        {
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

                book.CreatedAt = DateTime.Now;
                book.UpdatedAt = DateTime.Now;
                _context.Add(book);
                await _context.SaveChangesAsync();
                _notyfService.Success("Thêm mới thành công");
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Name", book.AuthorId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", book.CategoryId);
            ViewData["DistributorId"] = new SelectList(_context.Distributors, "Id", "Name", book.DistributorId);
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Name", book.PublisherId);
            ViewData["TitleId"] = new SelectList(_context.Titles, "Id", "Name", book.TitleId);
            return View(book);
        }

        // GET: Admin/Book/Edit/5
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
        public async Task<IActionResult> Edit(int id, [Bind("Description,Image,Price,Discount,DistributorId,PublisherId,AuthorId,TitleId,CategoryId,Id,Name,CreatedAt,UpdatedAt,DeletedAt,UpdateUserId,CreateUserId,Quantity")] Book book, Microsoft.AspNetCore.Http.IFormFile fThumb)
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

        // GET: Admin/Book/Delete/5
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

        // POST: Admin/Book/Delete/5
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
        ///////////////////////////////////////////
        [HttpPost]
        public FileResult ExportToExcel()
        {
            DataTable dt = new DataTable("Book");
            dt.Columns.AddRange(new DataColumn[5] {  new DataColumn("Ten"),
                                                     new DataColumn("So luong"),
                                                     new DataColumn("Gia"),
                                                     new DataColumn("Gia Giam"),
                                                     new DataColumn("Ngay nhap")
            });

            var insuranceCertificate = from Book in _context.Books select Book;

            foreach (var insurance in insuranceCertificate)
            {
                dt.Rows.Add(insurance.Name, insurance.Quantity, insurance.Price, insurance.Discount,
                    insurance.CreatedAt);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ExcelFile.xlsx");
                }
            }
        }
        ///////////////////////////////////////////

    }
    /////////////////////////////////////////////
}

