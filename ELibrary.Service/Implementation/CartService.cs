using ELibrary.Domain.Identity;
using ELibrary.Domain.Models;
using ELibrary.Repository.Interface;
using ELibrary.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ELibrary.Service.Implementation
{
    public class CartService : ICartService
    {
        private readonly IUserRepository _userRepository;
        private readonly IBookRepository _bookRepository;
        public CartService(IUserRepository userRepository, IBookRepository bookRepository)
        {
            _userRepository = userRepository;
            _bookRepository = bookRepository;
        }
        public void AddToCart(string userId, Guid bookId)
        {
            ELibraryUser user = _userRepository.GetWithCart(userId);
            Book book = _bookRepository.Get(bookId);
            List<BooksInCart> l = user.UserCart.BooksInCart.ToList();
            if (!l.Select(i => i.Book).Contains(book))
            l.Add(new BooksInCart
            {
                Book = book,
                Cart = user.UserCart
            });
            user.UserCart.BooksInCart = l;
            _userRepository.Update(user);
        }

        public void ClearCart(string userId)
        {
            ELibraryUser user = _userRepository.GetWithCart(userId);
            List<BooksInCart> l = user.UserCart.BooksInCart.ToList();
            l.Clear();
            user.UserCart.BooksInCart = l;
            _userRepository.Update(user);
        }

        public Cart getCart(string userId)
        {
            ELibraryUser user = _userRepository.GetWithCart(userId);
            return user.UserCart;
        }

        public void RemoveFromCart(string userId, Guid Id)
        {
            ELibraryUser user = _userRepository.GetWithCart(userId);
            List<BooksInCart> l = user.UserCart.BooksInCart.ToList();
            l.RemoveAll(i => i.Id == Id);
            user.UserCart.BooksInCart = l;
            _userRepository.Update(user);
        }
    }
}
