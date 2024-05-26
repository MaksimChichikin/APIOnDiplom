using Microsoft.EntityFrameworkCore;
using MyProApiDiplom.CommonAppData.DTO;
using MyProApiDiplom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

            
            messageDto.Timestamp = message.Timestamp;

            return messageDto;
        }
    }
}

