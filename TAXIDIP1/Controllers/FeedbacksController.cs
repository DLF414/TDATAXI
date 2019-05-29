using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TAXIDIP1.Models;
using Npgsql;
using Microsoft.AspNetCore.Authorization;

using System.Security.Claims;
namespace TAXIDIP1.Controllers
{

    [Authorize(Roles = "driver, admin, client")]
    public class FeedbacksController : Controller
    {
        private readonly TDATAXIContext _context;

        public FeedbacksController(TDATAXIContext context)
        {
            _context = context;
        }

        // GET: Feedbacks
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Index()
        {
            var tDATAXIContext = _context.Feedback.Include(f => f.Ride);
            return View("AdminFeedbackList",await tDATAXIContext.ToListAsync());
        }

        // GET: Feedbacks/Details/5
        [Authorize(Roles = "driver, admin, client")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feedback = await _context.Feedback
                .Include(f => f.Ride)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (feedback == null)
            {
                return NotFound();
            }

            return View(feedback);
        }
        
        // GET: Feedbacks/Create
        [Authorize(Roles = "driver, admin, client")]
        public IActionResult Create(int? id)
        {
            ViewData["RideId"] = id.ToString();
            return View();
        }

        [Authorize(Roles = "driver, admin, client")]
        public async Task<IActionResult> GetFeedback(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feedback = await _context.Feedback.FirstOrDefaultAsync(m => m.RideId == id);
            if (feedback == null)
            {
                return NotFound();
            }

            return View("UserFeedbackDisplay",feedback);
        }

        // POST: Feedbacks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "driver, admin, client")]
        public async Task<IActionResult> Create(Feedback feedback)
        {
            int currId = 0;
                    NpgsqlConnection conn = new NpgsqlConnection("Host=localhost;Port=5432;Database=TDATAXI;Username=DLF414;Password=123456Qw");
                    //  var conn = _context2.Database.GetDbConnection();
                    await conn.OpenAsync();
                    var command = conn.CreateCommand();
                    const string query = "select nextval('feedback_id_seq')";
                    command.CommandText = query;
                    var reader = await command.ExecuteReaderAsync();
                     currId = 0;
                    while (await reader.ReadAsync())
                    {
                        currId = Convert.ToInt32(reader.GetInt64(0));
                        // Do whatever you want with title 
                    }


            Rides ride = await _context.Rides.FirstOrDefaultAsync(r => r.Id == feedback.RideId);
            ride.IsComplained = true;
            ride.UpdatedAt = DateTime.Now;
            _context.Update(ride);
            string role = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value;
                _context.Feedback.Add(new Feedback { Id = currId, RideId=feedback.RideId,Note=feedback.Note,createdBy=role,Type=feedback.Type, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now });
                    await _context.SaveChangesAsync();
                Feedback feedbackres = await _context.Feedback.FirstOrDefaultAsync(r => r.Id == currId);
                return View("UserFeedbackDisplay", feedbackres);
        }

        // GET: Feedbacks/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feedback = await _context.Feedback.FindAsync(id);
            if (feedback == null)
            {
                return NotFound();
            }
            return View(feedback);
        }



        [Authorize(Roles = "driver, client")]
        public async Task<IActionResult> ClientEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feedback = await _context.Feedback.FindAsync(id);
            if (feedback == null)
            {
                return NotFound();
            }
            return View(feedback);
        }

        // POST: Feedbacks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RideId,Note,CreatedAt,UpdatedAt")] Feedback feedback)
        {
            if (id != feedback.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(feedback);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FeedbackExists(feedback.Id))
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
            ViewData["RideId"] = new SelectList(_context.Rides, "Id", "Id", feedback.RideId);
            return View(feedback);
        }
        // POST: Feedbacks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "client, driver")]
        public async Task<IActionResult> ClientEdit(int id, Feedback feedback)
        {
            if (id != feedback.Id)
            {
                return NotFound();
            }
            Feedback feedback2 = await _context.Feedback.FirstOrDefaultAsync(r => r.Id == id);

            feedback2.UpdatedAt = DateTime.Now;
            feedback2.Note = feedback.Note;
            if (ModelState.IsValid)
            {
                _context.Update(feedback2);
                await _context.SaveChangesAsync();
           
                Feedback feedbackres = await _context.Feedback.FirstOrDefaultAsync(r => r.Id == id);
                return View("UserFeedbackDisplay", feedbackres);
            }
            return View(feedback);
        }


        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateResponse(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feedback = await _context.Feedback.FindAsync(id);
            if (feedback == null)
            {
                return NotFound();
            }
            return View(feedback);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateResponse(int id, Feedback feedback)
        {
            if (id != feedback.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    Feedback feedbackres = await _context.Feedback.FirstOrDefaultAsync(r => r.Id == id);
                    feedbackres.UpdatedAt = DateTime.Now;
                    feedbackres.Responce = feedback.Responce;
                    _context.Update(feedbackres);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FeedbackExists(feedback.Id))
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
            return View(feedback);
        }


        // GET: Feedbacks/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feedback = await _context.Feedback
                .Include(f => f.Ride)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (feedback == null)
            {
                return NotFound();
            }

            return View(feedback);
        }

        // POST: Feedbacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var feedback = await _context.Feedback.FindAsync(id);
            _context.Feedback.Remove(feedback);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FeedbackExists(int id)
        {
            return _context.Feedback.Any(e => e.Id == id);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ManageDriver(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feedback = await _context.Feedback.FirstOrDefaultAsync(m => m.Id == id);
            if (feedback == null)
            {
                return NotFound();
            }
            Rides ride = await _context.Rides.FirstOrDefaultAsync(m => m.Id == feedback.RideId);
            Drivers driver = await _context.Drivers.FirstOrDefaultAsync(m => m.Id == ride.DriverId);
            Accounts account = await _context.Accounts.FirstOrDefaultAsync(m => m.Id == driver.AccountId);
            return RedirectToAction("AdminDetails", "Accounts",account);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ManageClient(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feedback = await _context.Feedback.FirstOrDefaultAsync(m => m.Id == id);
            if (feedback == null)
            {
                return NotFound();
            }
            Rides ride = await _context.Rides.FirstOrDefaultAsync(m => m.Id == feedback.RideId);
            Clients client = await _context.Clients.FirstOrDefaultAsync(m => m.Id == ride.ClientId);
            Accounts account = await _context.Accounts.FirstOrDefaultAsync(m => m.Id == client.AccountId);
            return RedirectToAction("AdminDetails", "Accounts", account);
        }
    }
}
