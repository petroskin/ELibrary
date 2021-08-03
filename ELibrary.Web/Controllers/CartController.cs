using ELibrary.Domain.DTO;
using ELibrary.Domain.Identity;
using ELibrary.Domain.Models;
using ELibrary.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ELibrary.Web.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IUserService _userService;
        private readonly IRentService _rentService;
        public CartController(ICartService cartService, IUserService userService, IRentService rentService)
        {
            _cartService = cartService;
            _userService = userService;
            _rentService = rentService;
        }
        // GET: Cart
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Cart model = _cartService.getCart(userId);

            string booksLeft = "Number of books you can rent this month: ";
            booksLeft += CalculateBooksLeft(_userService.GetDto(userId));
            ViewData["BooksLeft"] = booksLeft;

            return View(model);
        }
        // GET: Cart/RemoveFromCart/5
        public IActionResult RemoveFromCart(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _cartService.RemoveFromCart(userId, id);
            return RedirectToAction(nameof(Index));
        }
        // POST: Cart/RentNow
        [HttpPost]
        public IActionResult RentNow()
        {
            // NOT IMPLEMENTED

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Cart cart = _cartService.getCart(userId);
            ELibraryUserDto userDto = _userService.GetDto(userId);
            if (userDto.Role == "Standard" && userDto.BooksRented + cart.BooksInCart.Count() > ELibraryUser.BooksAllowedForStandard)
            {
                return RedirectToAction(nameof(RentDenied));
            }

            Rent rent = _rentService.Get(userId, DateTime.Now.Year, DateTime.Now.Month);
            List<BooksInRent> booksInRent = rent.BooksInRent.ToList();
            foreach (Book book in cart.BooksInCart.Select(i => i.Book))
            {
                booksInRent.Add(new BooksInRent
                {
                    Book = book,
                    Rent = rent
                });
            }

            _rentService.Update(rent);
            return RedirectToAction(nameof(Index));
        }
        // GET: Cart/RentDenied
        public IActionResult RentDenied()
        {
            return View();
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
