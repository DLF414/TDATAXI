using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TAXIDIP1.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

using Npgsql;

namespace TAXIDIP1.Controllers
{
    [Authorize(Roles = "driver, client, admin")]
    public class AccountsController : Controller
    {
        private readonly TDATAXIContext _context;

        public AccountsController(TDATAXIContext context)
        {
            _context = context;
        }

        // GET: Accounts
        public async Task<IActionResult> Index()
        {
            return View(await _context.Accounts.ToListAsync());
        }

        // GET: Accounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accounts = await _context.Accounts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (accounts == null)
            {
                return NotFound();
            }

            return View(accounts);
        }

        public async Task<IActionResult> AdminDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accounts = await _context.Accounts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (accounts == null)
            {
                return NotFound();
            }

            return View(accounts);
        }

        // GET: Accounts/Create
        public IActionResult Create()
        {
            return View();
        }

        // GET: AdminIndex
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AdminIndex()
        {
            return View(await _context.Accounts.ToListAsync());
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Ban(int? id)
        {
           
            if (id == null )
            {
                return NotFound();
            }

            var accounts = await _context.Accounts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (accounts == null)
            {
                return NotFound();
            }
            accounts.IsBlocked = true;
            accounts.UpdatedAt = DateTime.Now;
            _context.Update(accounts);
            await _context.SaveChangesAsync();
            return View("AdminIndex", await _context.Accounts.ToListAsync());
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Unban(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accounts = await _context.Accounts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (accounts == null)
            {
                return NotFound();
            }
            accounts.IsBlocked = false;
            accounts.UpdatedAt = DateTime.Now;
            _context.Update(accounts);
            await _context.SaveChangesAsync();
            return View("AdminIndex", await _context.Accounts.ToListAsync());
        }
        // POST: Accounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Login,Password,Role,CreatedAt,UpdatedAt")] Accounts accounts)
        {
            if (ModelState.IsValid)
            {
                NpgsqlConnection conn = new NpgsqlConnection("Host=localhost;Port=5432;Database=TDATAXI;Username=DLF414;Password=123456Qw");
                //  var conn = _context2.Database.GetDbConnection();
                await conn.OpenAsync();
                var command = conn.CreateCommand();
                const string query = "select nextval('accounts_id_seq')";
                command.CommandText = query;
                var reader = await command.ExecuteReaderAsync();
                int currId = 0;
                while (await reader.ReadAsync())
                {
                    currId = Convert.ToInt32(reader.GetInt64(0));
                    // Do whatever you want with title 
                }

                // добавляем пользователя в бд
                _context.Accounts.Add(new Accounts { Id = currId, Login = accounts.Login, Password = accounts.Password, Role = accounts.Role, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(accounts);
        }

        // GET: Accounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accounts = await _context.Accounts.FindAsync(id);
            if (accounts == null)
            {
                return NotFound();
            }
            return View(accounts);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Login,Password,Role,CreatedAt,UpdatedAt")] Accounts accounts)
        {
            if (id != accounts.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(accounts);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountsExists(accounts.Id))
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
            return View(accounts);
        }

        // GET: Accounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accounts = await _context.Accounts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (accounts == null)
            {
                return NotFound();
            }

            return View(accounts);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var accounts = await _context.Accounts.FindAsync(id);
            _context.Accounts.Remove(accounts);
            await _context.SaveChangesAsync();
            return View("AdminIndex", await _context.Accounts.ToListAsync());
        }

        private bool AccountsExists(int id)
        {
            return _context.Accounts.Any(e => e.Id == id);
        }
    }
}
