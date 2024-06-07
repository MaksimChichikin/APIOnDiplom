using MyProApiDiplom.Interface;
using MyProApiDiplom.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyProApiDiplom.CommonAppData.DTO;
using System.Net.Mail;
using System.Net;

namespace MyProApiDiplom.CommonAppData.User
{
    public class UsermyClass
    {
        private readonly IlecContext _context;

        public UsermyClass(IlecContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserDTO>> GetUsersAsync()
        {
            var users = await _context.Users
                                      .Include(u => u.IdRoleNavigation)  
                                      .ToListAsync();
            return users.Select(u => new UserDTO
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                Password = u.Password,
                RoleId = u.IdRole
            }).ToList();
        }

        public async Task<UserDTO> GetUserByIdAsync(int id)
        {
            var user = await _context.Users
                                     .Include(u => u.IdRoleNavigation)  
                                     .FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                return null;

            return new UserDTO
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Password = user.Password,
                RoleId = user.IdRole  
            };
        }

        public async Task<UserDTO> CreateUserAsync(UserCreateDTO userCreateDto)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == userCreateDto.RoleId);
            if (role == null)
            {
                throw new Exception("Role not found");
            }

            var user = new MyProApiDiplom.Models.User
            {
                FullName = userCreateDto.FullName,
                Email = userCreateDto.Email,
                Password = userCreateDto.Password,
                IdRole = role.Id
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDTO
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Password = user.Password,
                RoleId = role.Id  
            };
        }

        public async Task<UserDTO> UpdateUserAsync(int id, UserDTO userDto)
        {
            var user = await _context.Users.Include(u => u.IdRoleNavigation).FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                return null;

            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == userDto.RoleId);
            if (role == null)
            {
                throw new Exception("Role not found");
            }

            user.FullName = userDto.FullName;
            user.Email = userDto.Email;
            user.Password = userDto.Password;
            user.IdRole = role.Id;

            await _context.SaveChangesAsync();

            return new UserDTO
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Password = user.Password,
                RoleId = role.Id  
            };
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> SendNotificationAsync(string email, string messageBody)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return false;
            }

            try
            {
                using (SmtpClient client = new SmtpClient("smtp.mail.ru", 587))
                {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential("sierdzhi@mail.ru", "xvb5CCmzZescDHp4ZNqm");
                    client.EnableSsl = true;

                    MailMessage message = new MailMessage();
                    message.From = new MailAddress("sierdzhi@mail.ru", "Сиэрджи Констракшн");
                    message.To.Add(email);
                    message.Subject = "Уведомление";
                    message.Body = messageBody;

                    client.Send(message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error sending email: {ex.Message}");
            }

            return true;
        }
    }
}
