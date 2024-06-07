using Microsoft.EntityFrameworkCore;
using MyProApiDiplom.CommonAppData.DTO;
using MyProApiDiplom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MyProApiDiplom.Services
{
    public class MessageService : IMessageService
    {
        private readonly IlecContext _context;

        public MessageService(IlecContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MessageDTO>> GetMessagesAsync(int senderId, int receiverId)
        {
            var messages = await _context.Messages
                .Where(m => (m.SenderId == senderId && m.ReceiverId == receiverId) ||
                            (m.SenderId == receiverId && m.ReceiverId == senderId))
                .OrderBy(m => m.Timestamp)
                .ToListAsync();

            return messages.Select(m => new MessageDTO
            {
                SenderId = m.SenderId,
                ReceiverId = m.ReceiverId,
                Contents = m.Contents,
                Timestamp = m.Timestamp
            }).ToList();
        }

        public async Task<IEnumerable<MessageDTO>> GetAllMessagesAsync()
        {
            var messages = await _context.Messages
                .OrderBy(m => m.Timestamp)
                .ToListAsync();

            return messages.Select(m => new MessageDTO
            {
                SenderId = m.SenderId,
                ReceiverId = m.ReceiverId,
                Contents = m.Contents,
                Timestamp = m.Timestamp
            }).ToList();
        }

        public async Task<MessageDTO> SendMessageAsync(MessageDTO messageDto)
        {
            var message = new Message
            {
                SenderId = messageDto.SenderId,
                ReceiverId = messageDto.ReceiverId,
                Contents = messageDto.Contents,
                Timestamp = DateTime.UtcNow
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

           
            var sender = await _context.Users.FirstOrDefaultAsync(u => u.Id == messageDto.SenderId);
            var receiver = await _context.Users.FirstOrDefaultAsync(u => u.Id == messageDto.ReceiverId);

            if (receiver != null && sender != null)
            {
                await SendNotificationAsync(receiver.Email, $"Вам пишет {sender.FullName}: {messageDto.Contents}");
            }

            messageDto.Timestamp = message.Timestamp;

            return messageDto;
        }

        public async Task<bool> SendNotificationAsync(string email, string messageBody)
        {
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

                    await client.SendMailAsync(message); 
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
