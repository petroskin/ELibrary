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

            int booksLeft = CalculateBooksLeft(_userService.GetDto(userId));
            ViewData["BooksLeft"] = booksLeft;
            ViewData["Status"] = booksLeft == -1 ? "Premium" : "Standard";

            ViewData["BooksRented"] = _rentService.Get(userId, DateTime.Now.Year, DateTime.Now.Month).BooksInRent.Select(i => i.Book).ToList();

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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Cart cart = _cartService.getCart(userId);
            Rent rent = _rentService.Get(userId, DateTime.Now.Year, DateTime.Now.Month);
            List<BooksInRent> booksInRent = rent.BooksInRent.ToList();

            ELibraryUserDto userDto = _userService.GetDto(userId);
            if (userDto.Role == "Standard" &&
                userDto.BooksRented + cart.BooksInCart.Where(i => !booksInRent.Select(j => j.Book).Contains(i.Book)).Count() > ELibraryUser.BooksAllowedForStandard)
            {
                return RedirectToAction(nameof(Index));
            }

            foreach (Book book in cart.BooksInCart.Select(i => i.Book))
            {
                if (!booksInRent.Select(i => i.Book).Contains(book))
                {
                    booksInRent.Add(new BooksInRent
                    {
                        Book = book,
                        Rent = rent
                    });
                }
            }
            rent.BooksInRent = booksInRent;
            _rentService.Update(rent);
            _cartService.ClearCart(userId);
            return RedirectToAction(nameof(Index));
        }
        private int CalculateBooksLeft(ELibraryUserDto dto)
        {
            if (dto.Role == "Standard")
            {
                return ELibraryUser.BooksAllowedForStandard - dto.BooksRented;
            }
            else
            {
                return -1;
            }
        }
    }
}
