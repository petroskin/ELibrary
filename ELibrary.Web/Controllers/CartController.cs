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
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Cart model = await _cartService.getCart(userId);

            int booksLeft = CalculateBooksLeft(await _userService.GetDto(userId));
            ViewData["BooksLeft"] = booksLeft;

            ViewData["BooksRented"] = (await _rentService.GetCurrentRentsByUser(userId)).Select(i => i.BookId);

            return View(model);
        }
        // GET: Cart/RemoveFromCart/5
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            await _cartService.RemoveFromCart(id);
            return RedirectToAction(nameof(Index));
        }
        // POST: Cart/RentNow
        [HttpPost]
        public async Task<IActionResult> RentNow()
        {
            await _rentService.RentNow(await _userService.GetDto(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return RedirectToAction(nameof(Index));
        }
        private int CalculateBooksLeft(ELibraryUserDto dto)
        {
            int ret = ELibraryUser.BooksAllowed[dto.Role] - dto.BooksRented;
            return ret < 0 ? -1 : ret;
        }
    }
}
