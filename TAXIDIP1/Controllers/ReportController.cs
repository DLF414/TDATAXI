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

namespace TAXIDIP1.Controllers
{
    public class ReportController : Controller
    {
        private readonly TDATAXIContext _context;

        public ReportController(TDATAXIContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "admin")]
        public IActionResult AdminIndex()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AdminIndex(string login, string role, DateTime start, DateTime end)
        {
            if(role=="client")
            {

                Accounts account = await _context.Accounts.FirstOrDefaultAsync(m => m.Login == login);
                Clients client = await _context.Clients.FirstOrDefaultAsync(m => m.AccountId == account.Id);
                var rides = _context.Rides.Where(m => m.ClientId == client.Id && m.CreatedAt > start && m.CreatedAt < end);
                int RidesTotal = 0;
                int PriceTotal = 0;
                foreach (var ride in rides)
                {
                    RidesTotal++;
                    PriceTotal = PriceTotal + Convert.ToInt32(ride.Price);
                }
                string ReportData = $"Login: {login}   Поездок: {RidesTotal}   Сумма: {PriceTotal}  ";
                return Content(ReportData);
            }
            if (role == "driver")
            {

                Accounts account = await _context.Accounts.FirstOrDefaultAsync(m => m.Login == login);
                Drivers driver = await _context.Drivers.FirstOrDefaultAsync(m => m.AccountId == account.Id);
                var rides = _context.Rides.Where(m => m.DriverId == driver.Id && m.CreatedAt> start && m.CreatedAt<end);
                
                int RidesTotal = 0;
                int PriceTotal = 0;
                foreach (var ride in rides)
                {
                    RidesTotal++;
                    PriceTotal = PriceTotal + Convert.ToInt32(ride.Price);
                }
                string ReportData = $"Login: {login}   Поездок: {RidesTotal}   Сумма: {PriceTotal}  ";
                return Content(ReportData);
            }
            return Content("error");
        }
    }
}