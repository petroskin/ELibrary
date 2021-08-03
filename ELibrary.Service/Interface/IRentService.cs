using ELibrary.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELibrary.Service.Interface
{
    public interface IRentService
    {
        IEnumerable<Rent> GetAll();
        Rent Get(Guid? id);
        void Insert(Rent entity);
        void Update(Rent entity);
        void Delete(Rent entity);
        void Delete(Guid? id);
        Rent Get(string userId, int year, int month);
        IEnumerable<Rent> GetAll(string userId);
    }
}
