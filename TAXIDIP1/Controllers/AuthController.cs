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

namespace TAXIDIP1.Controllers
{
    public class AuthController : Controller
    {
        private readonly TDATAXIContext _context;
        private readonly TDATAXIContext _context2;
        public AuthController(TDATAXIContext context)
        {
            _context = context;
            _context2 = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                Accounts account = await _context.Accounts.FirstOrDefaultAsync(u => u.Login == model.Login && u.Password == model.Password);
                if (account != null)
                {
                    if (account.IsBlocked != true)
                    {
                        await Authenticate(account); // аутентификация

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("valid", "Вам бан");
                    }
                }
                ModelState.AddModelError("valid", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Register model)
        {
            if (ModelState.IsValid)
            {
                Accounts account = await _context.Accounts.FirstOrDefaultAsync(u => u.Login == model.Login && u.Password == model.Password);
                if (account == null)
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
                    _context.Accounts.Add(new Accounts {Id=currId, Login = model.Login, Password = model.Password,Role = model.Role,CreatedAt=DateTime.Now,UpdatedAt=DateTime.Now });
                    await _context.SaveChangesAsync();
                    await Authenticate(new Accounts { Id = currId, Login = model.Login, Password = model.Password, Role = model.Role, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now });


                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("valid", "Логин занят, либо некорректен");
            }
            return View(model);
        }

        private async Task Authenticate(Accounts account)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, account.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, account.Role),
                new Claim(ClaimTypes.Actor, account.Id.ToString())
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Auth");
        }
    }
}