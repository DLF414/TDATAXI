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
    public class ClientsController : Controller
    {
        private readonly TDATAXIContext _context;

        public ClientsController(TDATAXIContext context)
        {
            _context = context;
        }

        // GET: Clients
        public async Task<IActionResult> Index()
        {
            var tDATAXIContext = _context.Clients.Include(c => c.Account);
            return View(await tDATAXIContext.ToListAsync());
        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clients = await _context.Clients
                .Include(c => c.Account)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clients == null)
            {
                return NotFound();
            }

            return View(clients);
        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Login");
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "client")]
        public async Task<IActionResult> Create(Clients clients)
        {
            NpgsqlConnection conn = new NpgsqlConnection("Host=localhost;Port=5432;Database=TDATAXI;Username=DLF414;Password=123456Qw");
            //  var conn = _context2.Database.GetDbConnection();
            await conn.OpenAsync();
            var command = conn.CreateCommand();
            const string query = "select nextval('clients_id_seq')";
            command.CommandText = query;
            var reader = await command.ExecuteReaderAsync();
            int currId = 0;
            while (await reader.ReadAsync())
            {
                currId = Convert.ToInt32(reader.GetInt64(0));
                // Do whatever you want with title 
            }
           
            clients.Id = currId;
            clients.AccountId = Convert.ToInt32(User.FindFirst(x => x.Type == ClaimTypes.Actor).Value);
            clients.CreatedAt = DateTime.Now;
            clients.UpdatedAt = DateTime.Now;
            if (ModelState.IsValid)
            {
                _context.Add(clients);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clients = await _context.Clients.FindAsync(id);
            if (clients == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Login", clients.AccountId);
            return View(clients);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Clients clients)
        {
            if (id != clients.Id)
            {
                return NotFound();
            }

            clients.UpdatedAt = DateTime.Now;
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clients);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientsExists(clients.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Login", clients.AccountId);
            return RedirectToAction("Index", "Home");
        }

        // GET: Clients/Edit/5
        public async Task<IActionResult> Update()
        {

              int AccountId = Convert.ToInt32(User.FindFirst(x => x.Type == ClaimTypes.Actor).Value);

            var clients = await _context.Clients.FirstOrDefaultAsync(m => m.AccountId == AccountId);
            if (clients == null)
            {
                return NotFound();
            }
            return View(clients);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Clients clients)
        {
            if (id != clients.Id)
            {
                return NotFound();
            }
           Clients clients2 = await _context.Clients.FirstOrDefaultAsync(r => r.Id == clients.Id);

            clients2.UpdatedAt = DateTime.Now;
            clients2.Address = clients.Address;
            clients2.FirstName = clients.FirstName;
            clients2.LastName = clients.LastName;
            clients2.Phone = clients.Phone;
            if (ModelState.IsValid)
            {
                _context.Update(clients2);
                await _context.SaveChangesAsync();
            }
            
            return View(clients2);
        }

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clients = await _context.Clients
                .Include(c => c.Account)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clients == null)
            {
                return NotFound();
            }

            return View(clients);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clients = await _context.Clients.FindAsync(id);
            _context.Clients.Remove(clients);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientsExists(int id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }
    }
}
