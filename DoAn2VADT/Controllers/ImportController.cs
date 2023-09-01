using AspNetCoreHero.ToastNotification.Abstractions;
using DoAn2VADT.Database;
using DoAn2VADT.Database.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using PagedList.Core;

namespace DoAn2VADT.Controllers
{
    public class ImportController : Controller
    {
        private readonly AppDbContext _context;
        public INotyfService _notyfService { get; }

        public ImportController(AppDbContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }
        public IActionResult Index(int CatID = 0)
        {
            var pageNumber = 1;
            var pageSize = 8;

            List<Import> lsProducts = new List<Import>();
            if (CatID != 0)
            {
                lsProducts = _context.Imports
                .AsNoTracking()
                .OrderBy(x => x.Name).ToList();
            }
            else
            {
                lsProducts = _context.Imports
                .AsNoTracking()
                .OrderBy(x => x.Name).ToList();
            }
            PagedList<Import> models = new PagedList<Import>(lsProducts.AsQueryable(), pageNumber, pageSize);
            ViewBag.CurrentCateID = CatID;
            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }
        ///////////////////////////////////////////////

        public async Task<IActionResult> ImportfromExcel([FromForm] IFormFile file)
        {
            if (file == null)
            {
                _notyfService.Error("Vui lòng chọn file trước khi nhập");
                return View();
            }


            /*            try
                        {*/
            /*            var list = new List<ImportBook>();*/
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet ws = package.Workbook.Worksheets[0];
                    var rowcount = ws.Dimension.Rows;
                    for (int row = 2; row <= rowcount; row++)
                    {
                        /*                        list.Add(new ImportBook
                                                {
                                                    Name = ws.Cells[row, 1].Value.ToString().Trim(),
                                                    Quantity = ws.Cells[row, 2].Value.ToString().Trim(),
                                                    Price = ws.Cells[row, 3].Value.ToString().Trim(),
                                                    Discount = ws.Cells[row, 4].Value.ToString().Trim()
                                                });*/
                        if (_context.Books.Where(n => n.Name == ws.Cells[row, 1].Value.ToString().Trim()).FirstOrDefault() == null)
                        {
                            var b = new Book()
                            {
                                Name = ws.Cells[row, 1].Value.ToString().Trim(),
                                Quantity = int.Parse(ws.Cells[row, 2].Value.ToString().Trim()),
                                Price = int.Parse(ws.Cells[row, 3].Value.ToString().Trim()),
                                Discount = int.Parse(ws.Cells[row, 4].Value.ToString().Trim())
                            };
                            _context.Add(b);
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            var b = _context.Books.Where(n => n.Name == ws.Cells[row, 1].Value.ToString().Trim()).FirstOrDefault();
                            b.Quantity += int.Parse(ws.Cells[row, 2].Value.ToString().Trim());
                            b.Price = int.Parse(ws.Cells[row, 3].Value.ToString().Trim());
                            b.Discount = int.Parse(ws.Cells[row, 4].Value.ToString().Trim());
                            _context.Update(b);
                            await _context.SaveChangesAsync();
                        }
                    }
                }
                _notyfService.Success("Nhập dữ liệu thành công, vui lòng kiểm tra lại trong mục -Quản lý sách-");
                return RedirectToAction("Index", "Book");
            }
            /*             }
                       catch(Exception ex)
                        {
                            _notyfService.Error("Nhập không thành công, vui lòng xem lại: "+ ex.Message);
                            return RedirectToAction(nameof(Index));
                        }*/
        }


        ///////////////////////////////////////////

    }
}
