using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TAXIDIP1.Models;

using Microsoft.AspNetCore.Authorization;

namespace TAXIDIP1.Controllers
{
    public class ManagersController : Controller
    {
        private readonly TDATAXIContext _context;

        public ManagersController(TDATAXIContext context)
        {
            _context = context;
        }

        // GET: Managers
        public async Task<IActionResult> Index()
        {
            var tDATAXIContext = _context.Managers.Include(m => m.Account).Include(m => m.Company);
            return View(await tDATAXIContext.ToListAsync());
        }

        // GET: Managers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var managers = await _context.Managers
                .Include(m => m.Account)
                .Include(m => m.Company)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (managers == null)
            {
                return NotFound();
            }

            return View(managers);
        }

        // GET: Managers/Create
        public IActionResult Create()
        {
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Login");
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name");
            return View();
        }

        // POST: Managers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AccountId,CompanyId,FirstName,LastName,Phone,CreatedAt,UpdatedAt")] Managers managers)
        {
            if (ModelState.IsValid)
            {
                _context.Add(managers);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Login", managers.AccountId);
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", managers.CompanyId);
            return View(managers);
        }

        // GET: Managers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var managers = await _context.Managers.FindAsync(id);
            if (managers == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Login", managers.AccountId);
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", managers.CompanyId);
            return View(managers);
        }

        // POST: Managers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AccountId,CompanyId,FirstName,LastName,Phone,CreatedAt,UpdatedAt")] Managers managers)
        {
            if (id != managers.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(managers);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ManagersExists(managers.Id))
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
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Login", managers.AccountId);
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", managers.CompanyId);
            return View(managers);
        }

        // GET: Managers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var managers = await _context.Managers
                .Include(m => m.Account)
                .Include(m => m.Company)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (managers == null)
            {
                return NotFound();
            }

            return View(managers);
        }

        // POST: Managers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var managers = await _context.Managers.FindAsync(id);
            _context.Managers.Remove(managers);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ManagersExists(int id)
        {
            return _context.Managers.Any(e => e.Id == id);
        }
    }
}
