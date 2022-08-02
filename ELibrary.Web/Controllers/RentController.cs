using ELibrary.Domain.Models;
using ELibrary.Service.Interface;
using GemBox.Document;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ELibrary.Web.Controllers
{
    [Authorize]
    public class RentController : Controller
    {
        private readonly IRentService _rentService;
        private readonly IUserService _userService;
        public RentController(IRentService rentService, IUserService userService)
        {
            _rentService = rentService;
            _userService = userService;
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
        }
        // GET: Rent
        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IEnumerable<Rent> model;
            model = await _rentService.GetCurrentRentsByUser(userId);
            return View(model);
        }
        // GET: Rent/AllRents
        public async Task<IActionResult> AllRents()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IEnumerable<Rent> model = await _rentService.GetAll(userId);
            return View(model);
        }
        // GET: Rent/Export/5
        public async Task<FileContentResult> Export()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IEnumerable<Rent> rents = await _rentService.GetCurrentRentsByUser(userId);

            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.docx");
            var document = DocumentModel.Load(templatePath);

            StringBuilder sb = new StringBuilder();

            foreach (var item in rents.Select(i => i.Book))
            {
                sb.AppendLine(string.Join(", ", item.Authors.Select(a => a.Author.FullName())) + " - \"" + item.Name + "\"");
            }

            document.Content.Replace("{{UserName}}", (await _userService.GetDto(userId)).Email);
            document.Content.Replace("{{NumberBooks}}", rents.Count().ToString());
            document.Content.Replace("{{Books}}", sb.ToString());
            document.Content.Replace("{{Date}}", DateTime.Now.ToString("dd MMMM yyyy"));

            var stream = new MemoryStream();

            document.Save(stream, new PdfSaveOptions());

            return File(stream.ToArray(), new PdfSaveOptions().ContentType, "ExportInvoice.pdf");
        }
    }
}
