using ELibrary.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELibrary.Repository.Interface
{
    public interface IRentRepository : IRepository<Rent>
    {
        Rent Get(string userId, int year, int month);
        IEnumerable<Rent> GetAll(string userId);
    }
}
