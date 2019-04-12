using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TAXIDIP1.Models;

namespace TAXIDIP1.Controllers
{
    public class DriversController : Controller
    {
        private readonly TDATAXIContext _context;

        public DriversController(TDATAXIContext context)
        {
            _context = context;
        }

        // GET: Drivers
        public async Task<IActionResult> Index()
        {
            var tDATAXIContext = _context.Drivers.Include(d => d.Account).Include(d => d.Company);
            return View(await tDATAXIContext.ToListAsync());
        }

        // GET: Drivers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var drivers = await _context.Drivers
                .Include(d => d.Account)
                .Include(d => d.Company)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (drivers == null)
            {
                return NotFound();
            }

            return View(drivers);
        }

        // GET: Drivers/Create
        public IActionResult Create()
        {
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Login");
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name");
            return View();
        }

        // POST: Drivers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AccountId,FirstName,LastName,Phone,Address,CompanyId,CreatedAt,UpdatedAt")] Drivers drivers)
        {
            if (ModelState.IsValid)
            {
                _context.Add(drivers);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Login", drivers.AccountId);
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", drivers.CompanyId);
            return View(drivers);
        }

        // GET: Drivers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var drivers = await _context.Drivers.FindAsync(id);
            if (drivers == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Login", drivers.AccountId);
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", drivers.CompanyId);
            return View(drivers);
        }

        // POST: Drivers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AccountId,FirstName,LastName,Phone,Address,CompanyId,CreatedAt,UpdatedAt")] Drivers drivers)
        {
            if (id != drivers.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(drivers);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DriversExists(drivers.Id))
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
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Login", drivers.AccountId);
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", drivers.CompanyId);
            return View(drivers);
        }

        // GET: Drivers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var drivers = await _context.Drivers
                .Include(d => d.Account)
                .Include(d => d.Company)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (drivers == null)
            {
                return NotFound();
            }

            return View(drivers);
        }

        // POST: Drivers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var drivers = await _context.Drivers.FindAsync(id);
            _context.Drivers.Remove(drivers);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DriversExists(int id)
        {
            return _context.Drivers.Any(e => e.Id == id);
        }
    }
}
