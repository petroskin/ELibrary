using ELibrary.Domain.Identity;
using ELibrary.Domain.Models;
using ELibrary.Repository.Interface;
using ELibrary.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELibrary.Service.Implementation
{
    public class CartService : ICartService
    {
        private readonly IBookCartRepository _bookCartRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IBookAuthorRepository _bookAuthorRepository;
        private readonly IAuthorRepository _authorRepository;
        public CartService(IBookCartRepository bookCartRepository, ICartRepository cartRepository, IBookRepository bookRepository, IBookAuthorRepository bookAuthorRepository, IAuthorRepository authorRepository)
        {
            _bookCartRepository = bookCartRepository;
            _cartRepository = cartRepository;
            _bookRepository = bookRepository;
            _bookAuthorRepository = bookAuthorRepository;
            _authorRepository = authorRepository;
        }
        public async Task AddToCart(string userId, int bookId)
        {
            int id = int.Parse(userId);
            bool isPresent = await _bookCartRepository.IsPresent(id, bookId);
            if (!isPresent)
                await _bookCartRepository.Add(id, bookId);
        }

        public async Task<Cart> getCart(string userId)
        {
            Cart cart = await _cartRepository.Get(int.Parse(userId));
            cart.BooksInCart = await _bookCartRepository.GetByCart(int.Parse(userId));
            foreach(BooksInCart booksInCart in cart.BooksInCart)
            {
                booksInCart.Book = await _bookRepository.Get(booksInCart.BookId);
                booksInCart.Book.Authors = await _bookAuthorRepository.GetByBookId(booksInCart.Book.Id);
                foreach (BookAuthor bookAuthor in booksInCart.Book.Authors)
                {
                    bookAuthor.Author = await _authorRepository.Get(bookAuthor.AuthorId);
                }
            }
            return cart;
        }

        public async Task RemoveFromCart(int id)
        {
            await _bookCartRepository.Remove(id);
        }
    }
}
