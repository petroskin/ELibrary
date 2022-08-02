using ELibrary.Domain.DTO;
using ELibrary.Domain.Identity;
using ELibrary.Domain.Models;
using ELibrary.Repository.Interface;
using ELibrary.Service.Interface;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELibrary.Service.Implementation
{
    public class RentService : IRentService
    {
        private readonly IRentRepository _rentRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IBookCartRepository _bookCartRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IBookAuthorRepository _bookAuthorRepository;
        private readonly IBookCategoriesRepository _bookCategoriesRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly UserManager<ELibraryUser> _userManager;
        public RentService(
            IRentRepository rentRepository, 
            ICartRepository cartRepository, 
            IBookCartRepository bookCartRepository, 
            IBookRepository bookRepository, 
            IBookAuthorRepository bookAuthorRepository, 
            IBookCategoriesRepository bookCategoriesRepository, 
            IAuthorRepository authorRepository,
            ICategoryRepository categoryRepository,
            UserManager<ELibraryUser> userManager)
        {
            _rentRepository = rentRepository;
            _cartRepository = cartRepository;
            _bookCartRepository = bookCartRepository;
            _bookRepository = bookRepository;
            _bookAuthorRepository = bookAuthorRepository;
            _bookCategoriesRepository = bookCategoriesRepository;
            _authorRepository = authorRepository;
            _categoryRepository = categoryRepository;
            _userManager = userManager;
        }

        public async Task<IEnumerable<Rent>> GetAll(string userId)
        {
            IEnumerable<Rent> rents = await _rentRepository.GetAll(userId);
            foreach (Rent rent in rents)
            {
                rent.Book = await _bookRepository.Get(rent.BookId);
            }
            return rents;
        }

        public async Task<IEnumerable<Rent>> GetCurrentRentsByUser(string userId)
        {
            IEnumerable<Rent> rents = await _rentRepository.GetAllCurrent(userId);
            foreach (Rent rent in rents)
            {
                rent.Book = await _bookRepository.Get(rent.BookId);
                rent.Book.Authors = await _bookAuthorRepository.GetByBookId(rent.Book.Id);
                foreach (BookAuthor bookAuthor in rent.Book.Authors)
                {
                    bookAuthor.Author = await _authorRepository.Get(bookAuthor.AuthorId);
                }
                rent.Book.Categories = await _bookCategoriesRepository.GetByBookId(rent.Book.Id);
                foreach (CategoriesInBook categoriesInBook in rent.Book.Categories)
                {
                    categoriesInBook.Category = await _categoryRepository.Get(categoriesInBook.CategoryId);
                }
            }
            return rents;
        }

        public async Task RentNow(ELibraryUserDto dto)
        {
            Cart cart = await _cartRepository.Get(dto.IdInt);
            cart.BooksInCart = await _bookCartRepository.GetByCart(dto.IdInt);
            IEnumerable<Rent> rents = await _rentRepository.GetAllCurrent(dto.Id);
            int booksAllowed = ELibraryUser.BooksAllowed[dto.Role];
            if (booksAllowed != -1 && booksAllowed < cart.BooksInCart.Where(i => !rents.Select(r => r.BookId).Contains(i.BookId)).Count())
                return;
            foreach(BooksInCart b in cart.BooksInCart)
            {
                if (rents.Select(r => r.BookId).Contains(b.BookId))
                    continue;
                await _rentRepository.Insert(new Rent(dto.IdInt, b.BookId));
            }
            return;
        }
    }
}
