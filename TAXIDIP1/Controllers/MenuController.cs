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
    public class MenuController : Controller
    {
        private readonly TDATAXIContext _context;

        public MenuController(TDATAXIContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "driver, client, admin")]
        public async Task<IActionResult> Index()
        {
            string role = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value;
            ViewData["Role"] = role;
            Clients cli = null;
            if (role == "client")
            {
               cli  = await _context.Clients.FirstOrDefaultAsync(u => u.AccountId == Convert.ToInt32(User.FindFirst(x => x.Type == ClaimTypes.Actor).Value));
            }
            Drivers dri = null;
            if (role == "driver")
            {
                dri = await _context.Drivers.FirstOrDefaultAsync(u => u.AccountId == Convert.ToInt32(User.FindFirst(x => x.Type == ClaimTypes.Actor).Value));
            }
            if (cli != null|| dri !=null)
            {
                ViewData["UserInfo"] = "true";
            }
            else
            {
                ViewData["UserInfo"] = "false";
            }
            return View();
        }
        [Authorize(Roles = "admin")]
        public IActionResult AdminData()
        {
            return View();
        }
    }
}