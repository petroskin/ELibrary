using ELibrary.Domain;
using ELibrary.Domain.DTO;
using ELibrary.Domain.Identity;
using ELibrary.Domain.Models;
using ELibrary.Repository.Interface;
using ELibrary.Service.Interface;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ELibrary.Service.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IRentRepository _rentRepository;
        private readonly ICartRepository _cartRepository;
        private readonly UserManager<ELibraryUser> _userManager;
        private readonly RoleManager<ELibraryRole> _roleManager;
        private readonly IRepository<EmailMessage> _emailMessageRepository;
        private readonly EmailSettings _settings;
        public UserService(IUserRepository userRepository, IRoleRepository roleRepository, IUserRoleRepository userRoleRepository, IRentRepository rentRepository, ICartRepository cartRepository, UserManager<ELibraryUser> userManager, RoleManager<ELibraryRole> roleManager, IRepository<EmailMessage> emailMessageRepository, EmailSettings settings)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _rentRepository = rentRepository;
            _cartRepository = cartRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailMessageRepository = emailMessageRepository;
            _settings = settings;
        }

        public async Task ChangeRole(ELibraryUser user, string currentRole, string futureRole)
        {
            ELibraryRole futureRoleEntity = await _roleManager.FindByNameAsync(futureRole);
            if (futureRoleEntity == null)
                throw new Exception("Role does not exist.");
            await _userRoleRepository.ChangeUserRole(user.Id, futureRoleEntity.Id);
            await _userManager.RemoveFromRoleAsync(user, currentRole);
            await _userManager.AddToRoleAsync(user, futureRole);
        }

        public IEnumerable<ELibraryUser> GetUsers()
        {
            return _userManager.Users;
        }

        public IEnumerable<ELibraryRole> GetRoles()
        {
            return _roleManager.Roles;
        }

        public async Task<ELibraryUserDto> GetDto(string id)
        {
            ELibraryUser user = await _userManager.FindByIdAsync(id);
            ELibraryUserDto dto = new ELibraryUserDto(user);
            dto.Role = (await _userManager.GetRolesAsync(user))[0];
            dto.BooksRented = (await _rentRepository.GetAllCurrent(id)).Count();
            return dto;
        }

        public async Task InsertFromDtoAsync(IEnumerable<ExcelUserDataDto> entities)
        {
            foreach (ExcelUserDataDto entity in entities)
            {
                if (_userManager.Users.Where(i => i.Email == entity.Email).Any())
                {
                    continue;
                }
                ELibraryUser user = new ELibraryUser
                {
                    UserName = entity.Email,
                    Email = entity.Email,
                    Name = entity.Name,
                    Surname = entity.Surname,
                    EmailConfirmed = true
                };
                ELibraryRole role = await _roleManager.FindByNameAsync(entity.Role);
                if (role == null) 
                    continue;
                await _userManager.CreateAsync(user, entity.Password);
                int newUserId = (await _userRepository.GetLatest()).Id;
                await _cartRepository.Create(newUserId);
                await _userRoleRepository.ChangeUserRole(newUserId, (await _roleManager.FindByNameAsync(entity.Role)).Id);
                await _userManager.AddToRoleAsync(user, entity.Role);
            }
        }

        public async Task<ELibraryUser> Get(string id)
        {
            ELibraryUser user = await _userManager.FindByIdAsync(id);
            user.Roles = await _userRoleRepository.GetForUser(user.Id);
            foreach(UserRole role in user.Roles)
            {
                role.Role = await _roleRepository.Get(role.RoleId);
            }
            return user;
        }
        /*
* If everything works, delete this
* 
public async Task UpgradeStatus(string userId)
{
   ELibraryUserDto dto = this.GetDto(userId);
   if (dto.Role == "Admin")
       return;
   _userRepository.RemoveRoles(dto);
   _userManager.AddToRoleAsync(_userRepository.Get(dto.Id), "Premium").Wait();

   EmailMessage emailMessage = new EmailMessage();
   emailMessage.MailTo = dto.Email;
   emailMessage.Subject = "Successfully upgraded status!";
   emailMessage.Status = false;
   emailMessage.Content = "Congratulations, you have successfully upgraded your user status to premium for the user " + dto.Name + " " + dto.Surname;

   _emailMessageRepository.Insert(emailMessage);

   var mimeMessage = new MimeMessage
   {
       Sender = new MailboxAddress(_settings.SenderName, _settings.SmtpUsername),
       Subject = emailMessage.Subject
   };
   mimeMessage.From.Add(new MailboxAddress(_settings.EmailDisplayName, _settings.SmtpUsername));
   mimeMessage.Body = new TextPart(TextFormat.Plain) { Text = emailMessage.Content };
   mimeMessage.To.Add(new MailboxAddress(emailMessage.MailTo));

   try
   {
       using (var smtp = new MailKit.Net.Smtp.SmtpClient())
       {
           var socketOption = _settings.EmableSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto;
           smtp.Connect(_settings.SmtpServer, _settings.SmtpServerPort, socketOption);

           if (!string.IsNullOrEmpty(_settings.SmtpUsername))
           {
               smtp.Authenticate(_settings.SmtpUsername, _settings.SmtpPassword);
           }

           smtp.Send(mimeMessage);

           smtp.Disconnect(true);
       }
   }
   catch (SmtpException e)
   {
       throw e;
   }
}*/
    }
}
