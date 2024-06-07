using MyProApiDiplom.CommonAppData.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyProApiDiplom.Services
{
    public interface IMessageService
    {
        Task<IEnumerable<MessageDTO>> GetMessagesAsync(int senderId, int receiverId);
        Task<IEnumerable<MessageDTO>> GetAllMessagesAsync();
        Task<MessageDTO> SendMessageAsync(MessageDTO messageDto);
    }
}

