using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using TAXIDIP1.Models;
using Npgsql;

using Microsoft.AspNetCore.Authorization;

namespace TAXIDIP1.Controllers
{
    [Authorize(Roles = "driver, admin")]
    public class DriversController : Controller
    {
        private readonly TDATAXIContext _context;

        public DriversController(TDATAXIContext context)
        {
            _context = context;
        }

        // GET: Drivers
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Index()
        {
            var tDATAXIContext = _context.Drivers.Include(d => d.Account).Include(d => d.Company);
            return View(await tDATAXIContext.ToListAsync());
        }

        // GET: Drivers/Details/5
        [Authorize(Roles = "driver, admin")]
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
        [Authorize(Roles = "driver, admin")]
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
        [Authorize(Roles = "driver, admin")]
        public async Task<IActionResult> Create(Drivers drivers)
        {
            NpgsqlConnection conn = new NpgsqlConnection("Host=localhost;Port=5432;Database=TDATAXI;Username=DLF414;Password=123456Qw");
            //  var conn = _context2.Database.GetDbConnection();
            await conn.OpenAsync();
            var command = conn.CreateCommand();
            const string query = "select nextval('drivers_id_seq')";
            command.CommandText = query;
            var reader = await command.ExecuteReaderAsync();
            int currId = 0;
            while (await reader.ReadAsync())
            {
                currId = Convert.ToInt32(reader.GetInt64(0));
                // Do whatever you want with title 
            }

            drivers.Id = currId;
            drivers.AccountId = Convert.ToInt32(User.FindFirst(x => x.Type == ClaimTypes.Actor).Value);
            drivers.CreatedAt = DateTime.Now;
            drivers.UpdatedAt = DateTime.Now;
            if (ModelState.IsValid)
            {
                _context.Add(drivers);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Menu");
            }
            return RedirectToAction("Index", "Menu");
        }

        [Authorize(Roles = "driver, admin")]
        public async Task<IActionResult> Update()
        {

            int AccountId = Convert.ToInt32(User.FindFirst(x => x.Type == ClaimTypes.Actor).Value);

            var drivers = await _context.Drivers.FirstOrDefaultAsync(m => m.AccountId == AccountId);
            if (drivers == null)
            {
                return NotFound();
            }
            return View(drivers);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "driver, admin")]
        public async Task<IActionResult> Update(int id, Drivers drivers)
        {
            if (id != drivers.Id)
            {
                return NotFound();
            }
            Drivers drivers2 = await _context.Drivers.FirstOrDefaultAsync(r => r.Id == drivers.Id);

            drivers2.UpdatedAt = DateTime.Now;
            drivers2.Address = drivers.Address;
            drivers2.FirstName = drivers.FirstName;
            drivers2.LastName = drivers.LastName;
            drivers2.Phone = drivers.Phone;
            if (ModelState.IsValid)
            {
                _context.Update(drivers2);
                await _context.SaveChangesAsync();
            }

            return View(drivers2);
        }
        // GET: Drivers/Edit/5
        [Authorize(Roles = "driver, admin")]
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
        [Authorize(Roles = "driver, admin")]
        public async Task<IActionResult> Edit(int id, Drivers drivers)
        {
            if (id != drivers.Id)
            {
                return NotFound();
            }
            drivers.UpdatedAt = DateTime.Now;
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
                return RedirectToAction("Index", "Menu");
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Login", drivers.AccountId);
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", drivers.CompanyId);
            return RedirectToAction("Index", "Menu");
        }

        // GET: Drivers/Delete/5
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
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
