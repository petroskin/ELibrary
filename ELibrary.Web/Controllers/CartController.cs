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
        public CartController(ICartService cartService, IUserService userService)
        {
            _cartService = cartService;
            _userService = userService;
        }
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Cart model = _cartService.getCart(userId);

            string booksLeft = "Number of books you can rent this month: ";
            booksLeft += CalculateBooksLeft(_userService.GetDto(userId));
            ViewData["BooksLeft"] = booksLeft;

            return View(model);
        }
        public IActionResult RemoveFromCart(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _cartService.RemoveFromCart(userId, id);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public IActionResult RentNow()
        {
            // NOT IMPLEMENTED
            return RedirectToAction(nameof(Index));
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
