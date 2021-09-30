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
        public RentController(IRentService rentService)
        {
            _rentService = rentService;
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
        }
        // GET: Rent
        public IActionResult Index(Guid? id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Rent model;
            if (id.HasValue)
            {
                model = _rentService.Get(id.Value);
            }
            else
            {
                model = _rentService.Get(userId, DateTime.Now.Year, DateTime.Now.Month);
            }
            return View(model);
        }
        // GET: Rent/AllRents
        public IActionResult AllRents()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IEnumerable<Rent> model = _rentService.GetAll(userId).Where(i => i.BooksInRent.Count() > 0);
            return View(model);
        }
        // GET: Rent/Export/5
        public FileContentResult Export(Guid? id)
        {
            if (!id.HasValue)
            {
                return null;
            }
            Rent rent = _rentService.Get(id.Value);

            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.docx");
            var document = DocumentModel.Load(templatePath);

            StringBuilder sb = new StringBuilder();

            // done this far

            foreach (var item in rent.BooksInRent.Select(i => i.Book))
            {
                sb.AppendLine(item.Author.FullName() + " - \"" + item.Name + "\"");
            }

            document.Content.Replace("{{RentNumber}}", rent.Id.ToString());
            document.Content.Replace("{{UserName}}", rent.User.UserName);
            document.Content.Replace("{{NumberBooks}}", rent.BooksInRent.Count().ToString());
            document.Content.Replace("{{Books}}", sb.ToString());
            document.Content.Replace("{{Month}}", rent.GetDateFormat());

            var stream = new MemoryStream();

            document.Save(stream, new PdfSaveOptions());

            return File(stream.ToArray(), new PdfSaveOptions().ContentType, "ExportInvoice.pdf");
        }
    }
}
