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
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IBookRepository _bookRepository;
        private readonly UserManager<ELibraryUser> _userManager;
        public ReviewService(IReviewRepository reviewRepository, IBookRepository bookRepository, UserManager<ELibraryUser> userManager)
        {
            _reviewRepository = reviewRepository;
            _bookRepository = bookRepository;
            _userManager = userManager;
        }
        public async Task Delete(Review entity)
        {
            await _reviewRepository.Delete(entity);
        }

        public async Task Delete(int id)
        {
            await _reviewRepository.Delete(id);
        }

        public async Task<Review> Get(int id)
        {
            return await _reviewRepository.Get(id);
        }

        public async Task<IEnumerable<Review>> GetAll()
        {
            return await _reviewRepository.GetAll();
        }

        public async Task<IEnumerable<Review>> GetAllByBook(int id)
        {
            IEnumerable<Review> reviews = await _reviewRepository.GetAllByBook(id);
            foreach(Review review in reviews)
            {
                review.User = await _userManager.FindByIdAsync(review.UserId.ToString());
            }

            return reviews;
        }

        public async Task<IEnumerable<Review>> GetAllByUser(int id)
        {
            IEnumerable<Review> reviews = await _reviewRepository.GetAllByUser(id);
            foreach (Review review in reviews)
            {
                review.Book = await _bookRepository.Get(review.BookId);
            }

            return reviews;
        }

        public async Task Insert(Review entity)
        {
            await _reviewRepository.Insert(entity);
        }

        public async Task Update(Review entity)
        {
            await _reviewRepository.Update(entity);
        }
    }
}
