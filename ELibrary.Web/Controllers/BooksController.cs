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
        private readonly IPublisherService _publisherService;
        private readonly ICategoryService _categoryService;
        private readonly IUserService _userService;
        private readonly ICartService _cartService;
        private readonly IReviewService _reviewService;
        private readonly IRentService _rentService;
        public BooksController(
            IAuthorService authorService, 
            IBookService bookService, 
            IPublisherService publisherService, 
            ICategoryService categoryService, 
            IUserService userService, 
            ICartService cartService, 
            IReviewService reviewService,
            IRentService rentService)
        {
            _authorService = authorService;
            _bookService = bookService;
            _publisherService = publisherService;
            _categoryService = categoryService;
            _userService = userService;
            _cartService = cartService;
            _reviewService = reviewService;
            _rentService = rentService;
        }

        // GET: Books
        public async Task<IActionResult> Index(int categoryId)
        {
            ViewData["BooksLeft"] = "";
            if (User.Identity.IsAuthenticated)
            {
                string booksLeft = "Number of books you can rent this month: ";
                booksLeft += CalculateBooksLeft(await _userService.GetDto(User.FindFirstValue(ClaimTypes.NameIdentifier)));
                ViewData["BooksLeft"] = booksLeft;
            }

            IEnumerable<Category> categories = await _categoryService.GetAll();
            Category chosenCategory = null;
            IEnumerable<Book> books;
            if (categoryId == 0)
            {
                books = await _bookService.GetAll();
            }
            else
            {
                chosenCategory = await _categoryService.GetWithBooks(categoryId);
                books = chosenCategory.Books.Select(bc => bc.Book);
            }

            ViewData["categories"] = categories;
            ViewData["chosenCategoryId"] = categoryId;
            return View(books);
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var book = await _bookService.GetWithAuthorsCategoriesPublisher(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["BooksLeft"] = "";
            ViewData["UserId"] = 0;
            ViewData["Reviewable"] = false;
            if (User.Identity.IsAuthenticated)
            {
                string booksLeft = "Number of books you can rent this month: ";
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                booksLeft += CalculateBooksLeft(await _userService.GetDto(userId));
                ViewData["BooksLeft"] = booksLeft;
                ViewData["UserId"] = int.Parse(userId);
                if ((await _rentService.GetAll(userId)).Any(rent => rent.BookId == id))
                {
                    ViewData["Reviewable"] = true;
                }
            }

            book.Reviews = await _reviewService.GetAllByBook(book.Id);

            return View(book);
        }

        // GET: Books/Create
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Name,Description,ImageLink")] Book book)
        {
            if (ModelState.IsValid)
            {
                await _bookService.Insert(book);
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var book = await _bookService.GetWithAuthorsCategoriesPublisher(id);
            if (book == null)
            {
                return NotFound();
            }

            IEnumerable<Author> authors = await _authorService.GetAll();
            IEnumerable<Publisher> publishers = await _publisherService.GetAll();
            IEnumerable<Category> categories = await _categoryService.GetAll();
            ViewData["authors"] = authors;
            ViewData["publishers"] = publishers;
            ViewData["categories"] = categories;
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Description,ImageLink,PublisherId,Id")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                List<BookAuthor> bookAuthors = new List<BookAuthor>();
                List<CategoriesInBook> bookCategories = new List<CategoriesInBook>();
                foreach (string authorId in Request.Form["Authors"])
                {
                    bookAuthors.Add(new BookAuthor(int.Parse(authorId), book.Id));
                }
                foreach (string categoryId in Request.Form["Categories"])
                {
                    bookCategories.Add(new CategoriesInBook(book.Id, int.Parse(categoryId)));
                }
                book.Authors = bookAuthors;
                book.Categories = bookCategories;
                await _bookService.Update(book);
                return RedirectToAction(nameof(Index));
            }

            IEnumerable<Author> authors = await _authorService.GetAll();
            IEnumerable<Publisher> publishers = await _publisherService.GetAll();
            IEnumerable<Category> categories = await _categoryService.GetAll();
            ViewData["authors"] = authors;
            ViewData["publishers"] = publishers;
            ViewData["categories"] = categories;
            return View(book);
        }

        // GET: Books/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var book = await _bookService.GetWithAuthorsCategoriesPublisher(id);
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _bookService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
        // POST: Books/AddToCart
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddToCart(int id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _cartService.AddToCart(userId, id);
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin")]
        // GET: Books/Export?category=All
        public async Task<IActionResult> Export(int categoryId)
        {
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var workbook = new XLWorkbook();

            IXLWorksheet worksheet = workbook.Worksheets.Add("Books");
            worksheet.Cell(1, 1).Value = "Id";
            worksheet.Cell(1, 2).Value = "Name";
            worksheet.Cell(1, 3).Value = "Description";
            worksheet.Cell(1, 4).Value = "Authors";
            worksheet.Cell(1, 5).Value = "Categories";
            worksheet.Cell(1, 6).Value = "Publisher";

            IEnumerable<Book> books = await _bookService.GetAllWithAuthorsCategoriesPublisher();
            if (categoryId != 0)
            {
                books = books.Where(b => b.Categories.Select(bc => bc.CategoryId).Contains(categoryId));
            }
            int index = 1;
            foreach (Book book in books)
            {
                index++;
                worksheet.Cell(index, 1).Value = book.Id;
                worksheet.Cell(index, 2).Value = book.Name;
                worksheet.Cell(index, 3).Value = book.Description;
                worksheet.Cell(index, 4).Value = string.Join(", ", book.Authors.Select(ba => ba.Author.FullName()));
                worksheet.Cell(index, 5).Value = string.Join(", ", book.Categories.Select(ba => ba.Category.Name));
                worksheet.Cell(index, 6).Value = book.Publisher.Name;
            }

            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                var content = stream.ToArray();
                return File(content, contentType, "Books.xlsx");
            }
        }
        private string CalculateBooksLeft(ELibraryUserDto dto)
        {
            if (ELibraryUser.BooksAllowed[dto.Role] == -1)
                return "unlimited";
            int amount = ELibraryUser.BooksAllowed[dto.Role] - dto.BooksRented;
            return amount.ToString();
        }
    }
}
