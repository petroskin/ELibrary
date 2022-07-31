using ELibrary.Domain.Models;
using ELibrary.Repository.Interface;
using ELibrary.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ELibrary.Service.Implementation
{
    public class PublisherService : IPublisherService
    {
        private readonly IPublisherRepository _publisherRepository;

        public PublisherService(IPublisherRepository publisherRepository)
        {
            _publisherRepository = publisherRepository;
        }
        public async Task Delete(Publisher entity)
        {
            await _publisherRepository.Delete(entity);
        }

        public async Task Delete(int id)
        {
            await _publisherRepository.Delete(id);
        }

        public async Task<Publisher> Get(int id)
        {
            return await _publisherRepository.Get(id);
        }

        public async Task<IEnumerable<Publisher>> GetAll()
        {
            return await _publisherRepository.GetAll();
        }

        public async Task<Publisher> GetWithBooks(int id)
        {
            return await _publisherRepository.GetWithBooks(id);
        }

        public async Task Insert(Publisher entity)
        {
            if (entity.Id != 0)
                throw new Exception("Cannot insert entity with an id!");
            await _publisherRepository.Insert(entity);
        }

        public async Task Update(Publisher entity)
        {
            await _publisherRepository.Update(entity);
        }
    }
}
