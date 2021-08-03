using ELibrary.Domain.Models;
using ELibrary.Repository.Interface;
using ELibrary.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELibrary.Service.Implementation
{
    public class RentService : IRentService
    {
        private readonly IRentRepository _rentRepository;
        public RentService(IRentRepository rentRepository)
        {
            _rentRepository = rentRepository;
        }
        public void Delete(Rent entity)
        {
            _rentRepository.Delete(entity);
        }

        public void Delete(Guid? id)
        {
            _rentRepository.Delete(id);
        }

        public Rent Get(Guid? id)
        {
            return _rentRepository.Get(id);
        }

        public Rent Get(string userId, int year, int month)
        {
            return _rentRepository.Get(userId, year, month);
        }

        public IEnumerable<Rent> GetAll()
        {
            return _rentRepository.GetAll();
        }

        public IEnumerable<Rent> GetAll(string userId)
        {
            return _rentRepository.GetAll(userId);
        }

        public void Insert(Rent entity)
        {
            _rentRepository.Insert(entity);
        }

        public void Update(Rent entity)
        {
            _rentRepository.Update(entity);
        }
    }
}
