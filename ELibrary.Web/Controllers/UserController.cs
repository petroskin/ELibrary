using ELibrary.Domain.DTO;
using ELibrary.Domain.Identity;
using ELibrary.Service.Interface;
using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ELibrary.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        // GET: User
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            IEnumerable<ELibraryUser> users = _userService.GetUsers();
            ViewData["user"] = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(users);
        }
        // GET: User/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(string id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (id == userId)
            {
                return RedirectToAction(nameof(Index));
            }
            ELibraryUserDto model = await _userService.GetDto(id);
            if (model == null)
            {
                return RedirectToAction(nameof(Index));
            }

            ViewData["roles"] = _userService.GetRoles();
            return View(model);
        }
        // POST: /User/Details/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Details([Bind("Id,Role")] ELibraryUserDto model)
        {
            ELibraryUser user = await _userService.Get(model.Id);
            await _userService.ChangeRole(user, user.Roles.Where(r => r.DateEnd == null).FirstOrDefault().Role.Name, model.Role);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Import(IFormFile file)
        {
            //////////////////////////////////////////////////////////////////////////////
            //                                                                          //
            //      TO IMPORT USERS:                                                    //
            //                                                                          //
            //      Import each user in a separate row.                                 //
            //      The first field is email                                            //
            //      The second field is password                                        //
            //      The third field is role (Free / Regular / Premium / Gold / Admin)   //
            //      The fourth and fifth are name and surname, and are optional         //
            //                                                                          //
            //////////////////////////////////////////////////////////////////////////////

            string filePath = $"{Directory.GetCurrentDirectory()}\\Files\\{file.FileName}";
            using (FileStream fileStream = System.IO.File.Create(filePath))
            {
                file.CopyTo(fileStream);
                fileStream.Flush();
            }

            List<ExcelUserDataDto> users = new List<ExcelUserDataDto>();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            ExcelUserDataDto tmp;
            using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        tmp = new ExcelUserDataDto
                        {
                            Email = reader.GetValue(0).ToString(),
                            Password = reader.GetValue(1).ToString(),
                            Role = reader.GetValue(2).ToString()
                        };
                        if (reader.GetValue(3) == null)
                        {
                            tmp.Name = "Unknown";
                            tmp.Surname = "Unknown";
                        }
                        else
                        {
                            tmp.Name = reader.GetValue(3).ToString();
                            tmp.Surname = reader.GetValue(4).ToString();
                        }
                        users.Add(tmp);
                    }
                }
            }

            await _userService.InsertFromDtoAsync(users);

            return RedirectToAction(nameof(Index));
        }
        //GET: /User/Status
        [Authorize]
        public async Task<IActionResult> Status()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userDto = await _userService.GetDto(userId);
            ViewData["UserRole"] = userDto.Role;
            return View();
        }

        public async Task<IActionResult> UpgradeStatus(string stripeEmail, string stripeToken)
        {
            throw new NotImplementedException();
            //var customerService = new CustomerService();
            //var chargeService = new ChargeService();
            //string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            //var customer = customerService.Create(new CustomerCreateOptions
            //{
            //    Email = stripeEmail,
            //    Source = stripeToken
            //});

            //var charge = chargeService.Create(new ChargeCreateOptions
            //{
            //    Amount = (1000),
            //    Description = "ELibrary Status Upgrade",
            //    Currency = "usd",
            //    Customer = customer.Id
            //});

            //if (charge.Status == "succeeded")
            //{
            //    _userService.UpgradeStatus(userId);
            //}

            //return RedirectToAction("Status");
        }
    }
}
