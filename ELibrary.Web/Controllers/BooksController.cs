using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ELibrary.Domain.Models;
using ELibrary.Repository;
using Microsoft.AspNetCore.Authorization;
using ELibrary.Service.Interface;
using ELibrary.Domain.DTO;
using System.Security.Claims;
using ELibrary.Domain.Identity;
using ClosedXML.Excel;
using System.IO;
using ELibrary.Service.RDF.Interface;
using ELibrary.Service.RDF.Extension;

namespace ELibrary.Web.Controllers
{
    public class BooksController : Controller
    {
        private readonly IAuthorService _authorService;
        private readonly IBookService _bookService;
        private readonly IUserService _userService;
        private readonly ICartService _cartService;
        private readonly IRDFService _rdfService;
        public BooksController(IAuthorService authorService, IBookService bookService, IUserService userService, ICartService cartService, IRDFService rdfService)
        {
            _authorService = authorService;
            _bookService = bookService;
            _userService = userService;
            _cartService = cartService;
            _rdfService = rdfService;
        }

        // GET: Books
        public IActionResult Index(string category)
        {
            ViewData["BooksLeft"] = "";
            if (User.Identity.IsAuthenticated)
            {
                string booksLeft = "Number of books you can rent this month: ";
                booksLeft += CalculateBooksLeft(_userService.GetDto(User.FindFirstValue(ClaimTypes.NameIdentifier)));
                ViewData["BooksLeft"] = booksLeft;
            }

            IEnumerable<Book> books = _bookService.GetAll();
            if (category != null && category != "All")
            {
                books = books.Where(i => i.CategoriesInBook.Select(j => j.Category).Contains(category));
            }
            List<string> categories = new List<string> { "All" };
            categories.AddRange(Book.BookCategories.ToList());
            ViewData["categories"] = new SelectList(categories);

            return View(books);
        }

        // GET: Books/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = _bookService.Get(id);
            if (book == null)
            {
                return NotFound();
            }

            if (Request.Headers.ContainsKey("Accept"))
            {
                if (Request.Headers["Accept"] == "text/turtle")
                {
                    Response.Headers.Add("Content-Type", "text/turtle");
                    return File(book.GetRDFGraph(Service.RDF.Enum.Syntax.Turtle), "text/turtle");
                }
                if (Request.Headers["Accept"] == "application/rdf+xml")
                {
                    Response.Headers.Add("Content-Type", "application/rdf+xml");
                    return File(book.GetRDFGraph(Service.RDF.Enum.Syntax.RDFXML), "application/rdf+xml");
                }
            }

            ViewData["BooksLeft"] = "";
            if (User.Identity.IsAuthenticated)
            {
                string booksLeft = "Number of books you can rent this month: ";
                booksLeft += CalculateBooksLeft(_userService.GetDto(User.FindFirstValue(ClaimTypes.NameIdentifier)));
                ViewData["BooksLeft"] = booksLeft;
            }
            return View(book);
        }

        // GET: Books/MoreDetails/5
        public IActionResult MoreDetails(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = _bookService.Get(id);
            if (book == null)
            {
                return NotFound();
            }

            var info = _rdfService.GetBookInfo(book.Name);

            if (Request.Headers.ContainsKey("Accept"))
            {
                if (Request.Headers["Accept"] == "text/turtle")
                {
                    Response.Headers.Add("Content-Type", "text/turtle");
                    return File(info.AsGraph(book, Service.RDF.Enum.Syntax.Turtle), "text/turtle");
                }
                if (Request.Headers["Accept"] == "application/rdf+xml")
                {
                    Response.Headers.Add("Content-Type", "application/rdf+xml");
                    return File(info.AsGraph(book, Service.RDF.Enum.Syntax.RDFXML), "application/rdf+xml");
                }
            }

            string subject = info[0].Value("book").ToString();

            IDictionary<string, List<VDS.RDF.INode>> props = new Dictionary<string, List<VDS.RDF.INode>>();
            foreach (var i in info)
            {
                string key = i.Value("rel").ToString();
                if (!props.ContainsKey(key))
                    props.Add(new KeyValuePair<string, List<VDS.RDF.INode>>(key, new List<VDS.RDF.INode>()));
                props[key].Add(i.Value("obj"));
            }

            ViewData["subject"] = subject;
            ViewData["bookId"] = book.Id;
            return View(props);
        }

        // GET: Books/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            IEnumerable<Author> authors = _authorService.GetAll();
            IEnumerable<SelectListItem> selectList =
                from i in authors
                select new SelectListItem
                {
                    Selected = false,
                    Value = i.Id.ToString(),
                    Text = i.FullName()
                };
            ViewData["AuthorId"] = selectList;
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Create([Bind("Name,Description,Image,AuthorId,CategoriesInBook,Id")] Book book)
        {
            if (ModelState.IsValid)
            {
                book.Id = Guid.NewGuid();
                List<CategoriesInBook> list = book.CategoriesInBook.ToList();
                foreach (string i in Request.Form["CategoriesInBook"])
                {
                    if (!list.Select(i => i.Category).Contains(i))
                        list.Add(new CategoriesInBook(book, i));
                }
                book.CategoriesInBook = list;
                _bookService.Insert(book);
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_authorService.GetAll(), "Id", "FullName()", book.AuthorId);
            return View(book);
        }

        // GET: Books/Edit/5
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = _bookService.Get(id);
            if (book == null)
            {
                return NotFound();
            }
            IEnumerable<Author> authors = _authorService.GetAll();
            IEnumerable<SelectListItem> selectList =
                from i in authors
                select new SelectListItem
                {
                    Selected = (book.AuthorId == i.Id),
                    Value = i.Id.ToString(),
                    Text = i.FullName()
                };
            ViewData["AuthorId"] = selectList;
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Guid id, [Bind("Name,Description,Image,AuthorId,Id")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    List<CategoriesInBook> list = book.CategoriesInBook.ToList();
                    foreach (string i in Request.Form["CategoriesInBook"])
                    {
                        if (!list.Select(i => i.Category).Contains(i))
                            list.Add(new CategoriesInBook(book, i));
                    }
                    foreach (CategoriesInBook i in list)
                    {
                        if (!Request.Form["CategoriesInBook"].Contains(i.Category))
                            list.Remove(i);
                    }
                    book.CategoriesInBook = list;
                    _bookService.Update(book);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
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
            ViewData["AuthorId"] = new SelectList(_authorService.GetAll(), "Id", "FullName()", book.AuthorId);
            return View(book);
        }

        // GET: Books/Delete/5
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = _bookService.Get(id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var book = _bookService.Get(id);
            _bookService.Delete(book);
            return RedirectToAction(nameof(Index));
        }
        // POST: Books/AddToCart
        [HttpPost]
        [Authorize]
        public IActionResult AddToCart(Guid id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _cartService.AddToCart(userId, id);
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin")]
        // GET: Books/Export?category=All
        public IActionResult Export(string category)
        {
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var workbook = new XLWorkbook();

            IXLWorksheet worksheet = workbook.Worksheets.Add("Books");
            worksheet.Cell(1, 1).Value = "Id";
            worksheet.Cell(1, 2).Value = "Name";
            worksheet.Cell(1, 3).Value = "Description";
            worksheet.Cell(1, 4).Value = "Author";
            worksheet.Cell(1, 5).Value = "Categories";

            IEnumerable<Book> books;
            if (category == "All" || category == "")
            {
                books = _bookService.GetAll();
            }
            else
            {
                books = _bookService.GetAll().Where(i => i.CategoriesInBook.Select(j => j.Category).Contains(category));
            }
            int index = 1;
            foreach (Book book in books)
            {
                index++;
                worksheet.Cell(index, 1).Value = book.Id;
                worksheet.Cell(index, 2).Value = book.Name;
                worksheet.Cell(index, 3).Value = book.Description;
                worksheet.Cell(index, 4).Value = book.Author.FullName();
                worksheet.Cell(index, 5).Value = book.CategoriesInBook.Select(i => i.Category).Aggregate((i, j) => i + ", " + j);
            }

            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                var content = stream.ToArray();
                return File(content, contentType, "Books.xlsx");
            }
        }
        private bool BookExists(Guid id)
        {
            return _bookService.Get(id) != null;
        }

        private string CalculateBooksLeft(ELibraryUserDto dto)
        {
            if (dto.Role == "Standard")
            {
                return string.Format("{0}", ELibraryUser.BooksAllowedForStandard - dto.BooksRented);
            }
            else
            {
                return "unlimited";
            }
        }
    }
}
