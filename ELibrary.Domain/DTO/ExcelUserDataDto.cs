using System;
using System.Collections.Generic;
using System.Text;

namespace ELibrary.Domain.DTO
{
    public class ExcelUserDataDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public ExcelUserDataDto(string email, string password, string role, string name = null, string surname = null)
        {
            Email = email;
            Password = password;
            Role = role;
            Name = name;
            Surname = surname;
        }
        public ExcelUserDataDto() { }
    }
}
