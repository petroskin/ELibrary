using ELibrary.Domain.DTO;
using ELibrary.Domain.Identity;
using ELibrary.Service.Interface;
using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ELibrary.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        // GET: User
        public IActionResult Index()
        {
            IEnumerable<ELibraryUser> users = _userService.GetAll();
            ViewData["user"] = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(users);
        }
        // GET: User/Details/5
        public IActionResult Details(string id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (id == userId)
            {
                return RedirectToAction(nameof(Index));
            }
            ELibraryUserDto model = _userService.GetDto(id);
            if (model == null)
            {
                return RedirectToAction(nameof(Index));
            }

            ViewData["roles"] = _userService.GetRoles();
            return View(model);
        }
        // POST: /User/Details/5
        [HttpPost]
        public IActionResult Details([Bind("Id,Role")] ELibraryUserDto model)
        {
            _userService.ChangeRoles(model);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public IActionResult Import(IFormFile file)
        {
            //////////////////////////////////////////////////////////////////////////////
            //                                                                          //
            //      TO IMPORT USERS:                                                    //
            //                                                                          //
            //      Import each user in a separate row.                                 //
            //      The first field is email                                            //
            //      The second field is password                                        //
            //      The third field is role (Admin / Premium / Standard)                //
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

            _userService.InsertFromDtoAsync(users);

            return RedirectToAction(nameof(Index));
        }
    }
}
