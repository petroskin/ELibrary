using ELibrary.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELibrary.Domain.DTO
{
    public class ELibraryUserDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Role { get; set; }
        public int BooksRented { get; set; }
        public ELibraryUserDto() { }
        public ELibraryUserDto(ELibraryUser user)
        {
            Id = user.Id;
            Email = user.Email;
            Name = user.Name;
            Surname = user.Surname;
        }
    }
}
