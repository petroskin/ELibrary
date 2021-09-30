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

namespace ELibrary.Service.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ELibraryUser> _userManager;
        private readonly IRepository<EmailMessage> _emailMessageRepository;
        private readonly EmailSettings _settings;
        public UserService(IUserRepository userRepository, UserManager<ELibraryUser> userManager, IRepository<EmailMessage> emailMessageRepository, EmailSettings settings)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _emailMessageRepository = emailMessageRepository;
            _settings = settings;
        }

        public void ChangeRoles(ELibraryUserDto dto)
        {
            _userRepository.RemoveRoles(dto);
            _userManager.AddToRoleAsync(_userRepository.Get(dto.Id), dto.Role).Wait();
        }

        public void Delete(ELibraryUser entity)
        {
            _userRepository.Delete(entity);
        }

        public ELibraryUser Get(string id)
        {
            return _userRepository.Get(id);
        }

        public IEnumerable<ELibraryUser> GetAll()
        {
            return _userRepository.GetAll();
        }

        public ELibraryUserDto GetDto(string id)
        {
            return _userRepository.GetDto(id);
        }

        public IEnumerable<IdentityRole> GetRoles()
        {
            return _userRepository.GetRoles();
        }

        public ELibraryUser GetWithCart(string id)
        {
            return _userRepository.GetWithCart(id);
        }

        public void Insert(ELibraryUser entity)
        {
            _userRepository.Insert(entity);
        }

        public void InsertFromDtoAsync(IEnumerable<ExcelUserDataDto> entities)
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
                    UserCart = new Cart(),
                    EmailConfirmed = true
                };
                user.UserCart.UserId = user.Id;
                _userManager.CreateAsync(user, entity.Password).Wait();
                if (entity.Role != "Admin" && entity.Role != "Premium")
                {
                    entity.Role = "Standard";
                }
                _userManager.AddToRoleAsync(user, entity.Role).Wait();
            }
        }

        public void UpgradeStatus(string userId)
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
        }

        public void Update(ELibraryUser entity)
        {
            _userRepository.Update(entity);
        }
    }
}
