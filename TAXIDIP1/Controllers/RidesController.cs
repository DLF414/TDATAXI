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
    [Authorize]
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
            string role = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value;
            if(role =="driver")
            {
                Drivers curDri = await _context.Drivers.FirstOrDefaultAsync(u => u.AccountId == Convert.ToInt32(User.FindFirst(x => x.Type == ClaimTypes.Actor).Value));
                var tDATAXIContext = _context.Rides.Where(r => r.DriverId == curDri.Id);
                return View("DriverHistory",await tDATAXIContext.ToListAsync());
            }
            if (role == "client")
            {
                Clients curCli = await _context.Clients.FirstOrDefaultAsync(u => u.AccountId == Convert.ToInt32(User.FindFirst(x => x.Type == ClaimTypes.Actor).Value));
                var tDATAXIContext = _context.Rides.Where(r => r.ClientId == curCli.Id);
                return View("ClientHistory",await tDATAXIContext.ToListAsync());
            }
            if (role == "admin")
            {
                var tDATAXIContext = _context.Rides.Include(r => r.Client).Include(r => r.Driver);
                return View(await tDATAXIContext.ToListAsync());
            }
            return View();
            
        }

        [Authorize(Roles = "driver, admin")]
        public async Task<IActionResult> NotAccepted()
        {
            var tDATAXIContext = _context.Rides.Where(r => r.IsAccepted == false);
            return View(await tDATAXIContext.ToListAsync());
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AdminRideDisplay(int id)
        {
            Rides rides = await _context.Rides.FirstOrDefaultAsync(r => r.Id == id);

            return View("Edit",rides);
        }
        [Authorize(Roles = "driver")]
        public async Task<IActionResult> Accept(int id)
        {
            Rides rides = await _context.Rides.FirstOrDefaultAsync(r => r.Id == id);
            var cur = await _context.Drivers.FirstOrDefaultAsync(u => u.AccountId == Convert.ToInt32(User.FindFirst(x => x.Type == ClaimTypes.Actor).Value));
            rides.Driver = cur;
            rides.IsAccepted = true;
            rides.AcceptedAt = DateTime.Now;
            if (ModelState.IsValid)
            {
                _context.Update(rides);
                await _context.SaveChangesAsync();
            }
            return View(rides);
        }
        [Authorize(Roles = "driver")]
        public async Task<IActionResult> Finish(int id)
        {
            Rides rides = await _context.Rides.FirstOrDefaultAsync(r => r.Id == id);
            
            return View(rides);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "driver")]
        public async Task<IActionResult> Finish(Rides rides)
        {
            Rides rides2 = await _context.Rides.FirstOrDefaultAsync(r => r.Id == rides.Id);

            rides2.UpdatedAt = DateTime.Now;
            rides2.Distance = rides.Distance;
            rides2.Price = rides.Price;
            rides2.AddressEnd = rides.AddressEnd;
            if (ModelState.IsValid)
            {
                _context.Update(rides2);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Menu");

        }
        [Authorize(Roles = "client")]
         public async Task<IActionResult> Rate(int id)
         {
             Rides rides = await _context.Rides.FirstOrDefaultAsync(r => r.Id == id);

             return View(rides);
         }
         [HttpPost]
         [ValidateAntiForgeryToken]
         [Authorize(Roles = "client")]
         public async Task<IActionResult> Rate(Rides rides)
         {
             Rides rides2 = await _context.Rides.FirstOrDefaultAsync(r => r.Id == rides.Id);

             rides2.UpdatedAt = DateTime.Now;
             rides2.Rate = rides.Rate;
             if (ModelState.IsValid)
             {
                 _context.Update(rides2);
                 await _context.SaveChangesAsync();
             }
            Clients cli = await _context.Clients.FirstOrDefaultAsync(u => u.AccountId == Convert.ToInt32(User.FindFirst(x => x.Type == ClaimTypes.Actor).Value));

            return View("ClientHistory", await _context.Rides.Where(m => m.ClientId == cli.Id).ToListAsync());
        }

        [Authorize(Roles = "client")]
        public async Task<IActionResult> CurrentClientRide(int? id)
        {
            var rides = await _context.Rides.FirstOrDefaultAsync(m => m.Id == id);
            if (rides == null)
            {
                return NotFound();
            }

            return View(rides);
        }
        [Authorize(Roles = "client")]
        public async Task<IActionResult> Cancel(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rides = await _context.Rides.FirstOrDefaultAsync(m => m.Id == id);
            if (rides == null)
            {
                return NotFound();
            }
            rides.IsCanceled = true;
            _context.Rides.Update(rides);
            await _context.SaveChangesAsync();
            Clients cli = await _context.Clients.FirstOrDefaultAsync(u => u.AccountId == Convert.ToInt32(User.FindFirst(x => x.Type == ClaimTypes.Actor).Value));
            
            return View("ClientHistory", await _context.Rides.Where(m => m.ClientId == cli.Id).ToListAsync());
        }
        [Authorize(Roles = "client")]
        public async Task<IActionResult> ClientHistory()
        {
            Clients cli = await _context.Clients.FirstOrDefaultAsync(u => u.AccountId == Convert.ToInt32(User.FindFirst(x => x.Type == ClaimTypes.Actor).Value));

            return View(await _context.Rides.Where(m => m.ClientId == cli.Id).ToListAsync());
        }
        [Authorize(Roles = "driver")]
        public async Task<IActionResult> DriverHistory()
        {
            Drivers dri = await _context.Drivers.FirstOrDefaultAsync(u => u.AccountId == Convert.ToInt32(User.FindFirst(x => x.Type == ClaimTypes.Actor).Value));

            return View(await _context.Rides.Where(m => m.ClientId == dri.Id).ToListAsync());
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
        public async Task<IActionResult> Create()
        {
            Clients cli = await _context.Clients.FirstOrDefaultAsync(u => u.AccountId == Convert.ToInt32(User.FindFirst(x => x.Type == ClaimTypes.Actor).Value));

            var rideCheck = _context.Rides.Where(u => u.ClientId == cli.Id && u.Price == null || u.IsCanceled == true);

            if (rideCheck.Count() == 0)
            {
                return View();
            }
            else
            {
                return View("NotComplited");
            }
        }

        // POST: Rides/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Rides rides)
        {
            Clients cli = await _context.Clients.FirstOrDefaultAsync(u => u.AccountId == Convert.ToInt32(User.FindFirst(x => x.Type == ClaimTypes.Actor).Value));
            
                NpgsqlConnection conn = new NpgsqlConnection("Host=localhost;Port=5432;Database=TDATAXI;Username=DLF414;Password=123456Qw");
                //  var conn = _context2.Database.GetDbConnection();
                await conn.OpenAsync();
                var command = conn.CreateCommand();
                const string query = "select nextval('rides_id_seq')";
                command.CommandText = query;
                var reader = await command.ExecuteReaderAsync();
                int currId = 0;
                while (await reader.ReadAsync())
                {
                    currId = Convert.ToInt32(reader.GetInt64(0));
                }
                
                rides.Id = currId;
                rides.IsAccepted = false;
                rides.IsCanceled = false;
                rides.ClientId = cli.Id;
                rides.CreatedAt = DateTime.Now;
                rides.IsComplained = false;
                rides.UpdatedAt = DateTime.Now;
                if (ModelState.IsValid)
                {
                    _context.Add(rides);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
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
