using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ELibrary.Domain.Models;
using ELibrary.Repository;
using ELibrary.Service.Interface;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace ELibrary.Web.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly IReviewService _reviewService;
        private readonly IBookService _bookService;
        private readonly IUserService _userService;

        public ReviewsController(IReviewService reviewService, IBookService bookService, IUserService userService)
        {
            _reviewService = reviewService;
            _bookService = bookService;
            _userService = userService;
        }

        // GET: Reviews/FromUser/5
        public async Task<IActionResult> FromUser(int? userid)
        {
            ViewData["MyReviews"] = false;
            if (!userid.HasValue)
            {
                userid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                ViewData["MyReviews"] = true;
            }
            IEnumerable<Review> reviews = await _reviewService.GetAllByUser(userid.Value);

            var user = await _userService.Get(userid.Value.ToString());

            ViewData["UserName"] = $"{user.Name} {user.Surname}";

            return View(reviews);
        }

        // GET: Reviews/Create/5
        public async Task<IActionResult> Create(int? bookid)
        {
            if (bookid == null)
            {
                return NotFound();
            }

            Book book = await _bookService.Get(bookid.Value);
            if (book == null)
            {
                return NotFound();
            }

            Review review = new Review(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)), book.Id, 1, "");
            review.Book = book;

            ViewData["BookName"] = book.Name;
            return View(review);
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,BookId,Rating,Comment")] Review review)
        {
            if (ModelState.IsValid)
            {
                await _reviewService.Insert(review);
                return RedirectToAction(nameof(FromUser));
            }
            return View(review);
        }

        // GET: Reviews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Review review = await _reviewService.Get(id.Value);
            if (review == null)
            {
                return NotFound();
            }
            ViewData["BookName"] = (await _bookService.Get(review.BookId)).Name;
            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,BookId,Rating,Comment,Id")] Review review)
        {
            if (id != review.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _reviewService.Update(review);
                return RedirectToAction(nameof(FromUser));
            }
            ViewData["BookName"] = (await _bookService.Get(review.BookId)).Name;
            return View(review);
        }

        // GET: Reviews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Review review = await _reviewService.Get(id.Value);
            if (review == null)
            {
                return NotFound();
            }
            ViewData["BookName"] = (await _bookService.Get(review.BookId)).Name;
            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _reviewService.Delete(id);
            return RedirectToAction(nameof(FromUser));
        }
    }
}
