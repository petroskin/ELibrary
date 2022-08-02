﻿using ELibrary.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ELibrary.Repository.Interface
{
    public interface IRentRepository : IRepository<Rent>
    {
        Task<IEnumerable<Rent>> GetAll(string userId);
        Task<IEnumerable<Rent>> GetAllCurrent(string userId);
    }
}
