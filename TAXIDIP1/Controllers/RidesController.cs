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
    public class RidesController : Controller
    {
        private readonly TDATAXIContext _context;

        public RidesController(TDATAXIContext context)
        {
            _context = context;
        }

        // GET: Rides
        public async Task<IActionResult> Index()
        {
            var tDATAXIContext = _context.Rides.Include(r => r.Client).Include(r => r.Driver);
            return View(await tDATAXIContext.ToListAsync());
        }

        // GET: Rides/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rides = await _context.Rides
                .Include(r => r.Client)
                .Include(r => r.Driver)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rides == null)
            {
                return NotFound();
            }

            return View(rides);
        }

        // GET: Rides/Create
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Id");
            ViewData["DriverId"] = new SelectList(_context.Drivers, "Id", "Id");
            return View();
        }

        // POST: Rides/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DriverId,ClientId,Distance,Price,AcceptedAt,StartedAt,IsAccepted,AddressStart,AddressEnd,AddressCurrent,IsCanceled,Rate,IsComplained,CreatedAt,UpdatedAt,Path")] Rides rides)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rides);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Id", rides.ClientId);
            ViewData["DriverId"] = new SelectList(_context.Drivers, "Id", "Id", rides.DriverId);
            return View(rides);
        }

        // GET: Rides/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rides = await _context.Rides.FindAsync(id);
            if (rides == null)
            {
                return NotFound();
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Id", rides.ClientId);
            ViewData["DriverId"] = new SelectList(_context.Drivers, "Id", "Id", rides.DriverId);
            return View(rides);
        }

        // POST: Rides/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DriverId,ClientId,Distance,Price,AcceptedAt,StartedAt,IsAccepted,AddressStart,AddressEnd,AddressCurrent,IsCanceled,Rate,IsComplained,CreatedAt,UpdatedAt,Path")] Rides rides)
        {
            if (id != rides.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rides);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RidesExists(rides.Id))
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
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Id", rides.ClientId);
            ViewData["DriverId"] = new SelectList(_context.Drivers, "Id", "Id", rides.DriverId);
            return View(rides);
        }

        // GET: Rides/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rides = await _context.Rides
                .Include(r => r.Client)
                .Include(r => r.Driver)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rides == null)
            {
                return NotFound();
            }

            return View(rides);
        }

        // POST: Rides/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rides = await _context.Rides.FindAsync(id);
            _context.Rides.Remove(rides);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RidesExists(int id)
        {
            return _context.Rides.Any(e => e.Id == id);
        }
    }
}
