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
using AspNetCoreHero.ToastNotification.Abstractions;

namespace DoAn2VADT.Controllers
{
    public class DistributorController : Controller
    {
        private readonly AppDbContext _context;
        public INotyfService _notyfService { get; }

        public DistributorController(AppDbContext context, INotyfService notyfService)
        {
            _notyfService = notyfService;
            _context = context;
        }

        // GET: Admin/Distributor
        public async Task<IActionResult> Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 8;
            var lsPt = _context.Distributors
                .AsNoTracking()
                .OrderByDescending(x => x.Id);
            PagedList<Distributor> models = new PagedList<Distributor>(lsPt, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }

        // GET: Admin/Distributor/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Distributors == null)
            {
                return NotFound();
            }

            var distributor = await _context.Distributors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (distributor == null)
            {
                return NotFound();
            }

            return View(distributor);
        }

        // GET: Admin/Distributor/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Distributor/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Address,PhoneNumber,Email,Discount,Id,Name,CreatedAt,UpdatedAt,DeletedAt,UpdateUserId,CreateUserId")] Distributor distributor)
        {
            if (ModelState.IsValid)
            {
                distributor.CreatedAt = DateTime.Now;
                distributor.UpdatedAt = DateTime.Now;
                _context.Add(distributor);
                await _context.SaveChangesAsync();
                _notyfService.Success("Thêm mới thành công");
                return RedirectToAction(nameof(Index));
            }
            return View(distributor);
        }

        // GET: Admin/Distributor/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Distributors == null)
            {
                return NotFound();
            }

            var distributor = await _context.Distributors.FindAsync(id);
            if (distributor == null)
            {
                return NotFound();
            }
            return View(distributor);
        }

        // POST: Admin/Distributor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Address,PhoneNumber,Email,Discount,Id,Name,CreatedAt,UpdatedAt,DeletedAt,UpdateUserId,CreateUserId")] Distributor distributor)
        {
            if (id != distributor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                distributor.UpdatedAt = DateTime.Now;
                try
                {
                    _context.Update(distributor);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Update thành công");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DistributorExists(distributor.Id))
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
            return View(distributor);
        }

        // GET: Admin/Distributor/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Distributors == null)
            {
                return NotFound();
            }

            var distributor = await _context.Distributors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (distributor == null)
            {
                return NotFound();
            }

            return View(distributor);
        }

        // POST: Admin/Distributor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Distributors == null)
            {
                return Problem("Entity set 'AppDbContext.Distributors'  is null.");
            }
            var distributor = await _context.Distributors.FindAsync(id);
            if (distributor != null)
            {
                _context.Distributors.Remove(distributor);
            }
            
            await _context.SaveChangesAsync();
            _notyfService.Success("Xóa thành công");
            return RedirectToAction(nameof(Index));
        }

        private bool DistributorExists(int id)
        {
          return _context.Distributors.Any(e => e.Id == id);
        }
    }
}
