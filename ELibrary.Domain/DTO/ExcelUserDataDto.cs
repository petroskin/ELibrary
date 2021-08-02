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
        public ExcelUserDataDto(string email, string password, string role)
        {
            Email = email;
            Password = password;
            Role = role;
        }
        public ExcelUserDataDto() { }
    }
}
