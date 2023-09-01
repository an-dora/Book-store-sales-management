using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DoAn2VADT.Database;
using DoAn2VADT.Database.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagedList.Core;
using Newtonsoft.Json;
using DoAn2VADT.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Session;
using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using System.Drawing.Printing;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace DoAn2VADT.Controllers
{
    public class OrdersController : Controller
    {

        private readonly AppDbContext _context;
        public INotyfService _notyfService { get; }

        public OrdersController(AppDbContext context, INotyfService notyfService)
        {
            _notyfService = notyfService;
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index(int page = 1, int CatID = 0)
        {
            var pageNumber = page;
            var pageSize = 8;

            List<Book> lsProducts = new List<Book>();
            if (CatID != 0)
            {
                lsProducts = _context.Books
                .AsNoTracking()
                .Where(x => x.CategoryId == CatID && x.PublisherId == CatID)
                .Include(x => x.Category)
                .OrderBy(x => x.Id).ToList();
            }
            else
            {
                lsProducts = _context.Books
                .AsNoTracking()
                .Include(x => x.Category)
                .OrderBy(x => x.Id).ToList();
            }
            PagedList<Book> models = new PagedList<Book>(lsProducts.AsQueryable(), pageNumber, pageSize);
            ViewBag.CurrentCateID = CatID;
            ViewBag.CurrentPage = pageNumber;
            ViewData["DanhMuc"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["XB"] = new SelectList(_context.Publishers, "Id", "Name");
            return View(models);
        }
        public IActionResult HistoryOrder(int page = 1)
        {
            var pageNumber = page;
            var pageSize = 8;

                 var lsOrder = _context.Orders
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedAt);

            PagedList<Order> models = new PagedList<Order>(lsOrder.AsQueryable(), pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }
        public IActionResult OrderDetail(int id)
        {
            var pageNumber = 1;
            var pageSize = 8;
            var orderDetail = _context.OrderDetails
                .Where(x => x.OrderId == id)
                .Include(x=>x.Book);
            PagedList<OrderDetail> models = new PagedList<OrderDetail>(orderDetail.AsQueryable(), pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            return View(models);
            
        }
        [HttpPost]
        public IActionResult Index(DateTime from_date, DateTime to_date)
        {
            using (_context)
            {
                ViewBag.GetBills = (from b in _context.Orders where b.CreatedAt >= from_date && b.CreatedAt <= to_date== true select b).ToList();
                ViewBag.GetQuantityOrder = (from b in _context.Orders where b.CreatedAt >= from_date && b.CreatedAt <= to_date == true select b.Id).Count();
                ViewBag.SumToTal = (from b in _context.Orders where b.CreatedAt >= from_date && b.CreatedAt <= to_date == true select b.Total).Sum();
                return View();
            }
        }
        public IActionResult Filtter(int CatID = 0)
        {
            var url = $"/Order?CatID={CatID}";
            if (CatID == 0)
            {
                url = $"/Order";
            }
            return Json(new { status = "success", redirectUrl = url });
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public const string OKEY = "order";

        // Lấy Order từ Session (danh sách OrderItem)
        List<OrderItem> GetOrderItems()
        {

            var session = HttpContext.Session;
            string jsonOrder = session.GetString(OKEY);
            if (jsonOrder != null)
            {
                return JsonConvert.DeserializeObject<List<OrderItem>>(jsonOrder);
            }
            return new List<OrderItem>();
        }

        // Xóa Order khỏi session
        void ClearOrder()
        {
            var session = HttpContext.Session;
            session.Remove(OKEY);
        }

        // Lưu Order (Danh sách OrderItem) vào session
        void SaveOrderSession(List<OrderItem> ls)
        {
            var session = HttpContext.Session;
            string jsonorder = JsonConvert.SerializeObject(ls);
            session.SetString(OKEY, jsonorder);
        }

        [Route("addorder/{id:int}", Name = "addorder")]
        public IActionResult AddToOrder([FromRoute] int id)
        {

            var book = _context.Books
                .Where(p => p.Id == id)
                .FirstOrDefault();
            if (book == null)
                return NotFound("Không có sản phẩm");

            // Xử lý đưa vào Order ...
            var order = GetOrderItems();
            var oitem = order.Find(p => p.book.Id == id);
            if (oitem != null)
            {
                // Đã tồn tại, tăng thêm 1
                oitem.quantity++;
            }
            else
            {
                //  Thêm mới
                order.Add(new OrderItem() { quantity = 1, book = book });
            }
            // Lưu Order vào Session
            SaveOrderSession(order);
            // Chuyển đến trang hiện thị Order
            return RedirectToAction(nameof(Pay));
        }
        [Route("/removeorder/{id:int}", Name = "removeorder")]
        public IActionResult Removeorder([FromRoute] int id)
        {
            var order = GetOrderItems();
            var orderitem = order.Find(p => p.book.Id == id);
            if (orderitem != null)
            { 
                order.Remove(orderitem);
            }

            SaveOrderSession(order);
            return RedirectToAction(nameof(Pay));
        }
        [Route("/updateorder", Name = "updateorder")]
        [HttpPost]
        public IActionResult UpdateOrder([FromForm] int id, [FromForm] int quantity)
        {
            // Cập nhật Order thay đổi số lượng quantity ...
            var bookC = _context.Books.Where(b => b.Id == id);
            var order = GetOrderItems();
            var orderitem = order.Find(p => p.book.Id == id);
            if (orderitem != null)
            {
                orderitem.quantity = quantity;
            }
            SaveOrderSession(order);
            // Trả về mã thành công (không có nội dung gì - chỉ để Ajax gọi)
            return Ok();
        }

        [Route("/order", Name = "order")]
        public IActionResult Pay()
        {
            return View(GetOrderItems());
        }
        [Route("/checkout/{total:decimal?}", Name = "checkout")]
        public async Task<IActionResult> CheckOut(decimal? total)
        {
            List<OrderItem> Orderlts = GetOrderItems().ToList();
           var order = new Order();
            order.Name = "";
            order.CreatedAt = DateTime.Now;
            order.Total= total;
/*            order.CreateUserId = Convert.ToInt32(HttpContext.User.FindFirstValue("Id"));*/
            _context.Add(order);
            await _context.SaveChangesAsync();
            _notyfService.Success("Xong cmnr");
            foreach (OrderItem item in Orderlts)
            {
                OrderDetail o = new OrderDetail()
                {
                    Name = "",
                    OrderId = order.Id,
                    Price = item.book.Price,
                    Quanlity = item.quantity,
                    BookId = item.book.Id,
                };
                var bookO = _context.Books.Find(o.BookId);
                bookO.Quantity -= o.Quanlity;
                _context.Update(bookO);
                await _context.SaveChangesAsync();
                _context.Add(o);
                await _context.SaveChangesAsync();
            }
            ClearOrder();
            return RedirectToAction(nameof(Index));
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public IActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Total,Id,Name,CreatedAt,UpdatedAt,DeletedAt,UpdateUserId,CreateUserId")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Total,Id,Name,CreatedAt,UpdatedAt,DeletedAt,UpdateUserId,CreateUserId")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'AppDbContext.Orders'  is null.");
            }
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
          return _context.Orders.Any(e => e.Id == id);
        }
    }
}
