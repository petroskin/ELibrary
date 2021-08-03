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

namespace ELibrary.Web.Controllers
{
    public class BooksController : Controller
    {
        private readonly IAuthorService _authorService;
        private readonly IBookService _bookService;
        private readonly IUserService _userService;
        private readonly ICartService _cartService;
        public BooksController(IAuthorService authorService, IBookService bookService, IUserService userService, ICartService cartService)
        {
            _authorService = authorService;
            _bookService = bookService;
            _userService = userService;
            _cartService = cartService;
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
            if (category != null || category != "")
            {
                books = books.Where(i => i.CategoriesInBook.Select(j => j.Category).Contains(category));
            }
            List<string> categories = Book.BookCategories.ToList();
            categories.Add("");
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
            ViewData["BooksLeft"] = "";
            if (User.Identity.IsAuthenticated)
            {
                string booksLeft = "Number of books you can rent this month: ";
                booksLeft += CalculateBooksLeft(_userService.GetDto(User.FindFirstValue(ClaimTypes.NameIdentifier)));
                ViewData["BooksLeft"] = booksLeft;
            }
            return View(book);
        }

        // GET: Books/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_authorService.GetAll(), "Id", "FullName()");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Create([Bind("Name,Description,Image,AuthorId,Id")] Book book)
        {
            if (ModelState.IsValid)
            {
                book.Id = Guid.NewGuid();
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
            ViewData["AuthorId"] = new SelectList(_authorService.GetAll(), "Id", "FullName()", book.AuthorId);
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
                return string.Format("%d", ELibraryUser.BooksAllowedForStandard - dto.BooksRented);
            }
            else
            {
                return "unlimited";
            }
        }
    }
}
