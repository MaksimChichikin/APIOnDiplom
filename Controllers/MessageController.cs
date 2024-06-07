using Microsoft.AspNetCore.Mvc;
using MyProApiDiplom.CommonAppData.DTO;
using MyProApiDiplom.Services;

[ApiController]
[Route("api/messages")]
public class MessageController : ControllerBase
{
    private readonly IMessageService _messageService;

    public MessageController(IMessageService messageService)
    {
        _messageService = messageService;
    }

    [HttpGet("{senderId}/{receiverId}")]
    public async Task<ActionResult<IEnumerable<MessageDTO>>> GetMessages(int senderId, int receiverId)
    {
        var messages = await _messageService.GetMessagesAsync(senderId, receiverId);
        return Ok(messages);
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MessageDTO>>> GetAllMessages()
    {
        var messages = await _messageService.GetAllMessagesAsync();
        return Ok(messages);
    }


    [HttpPost]
    public async Task<ActionResult<MessageDTO>> SendMessage(MessageDTO messageDto)
    {
        var sentMessage = await _messageService.SendMessageAsync(messageDto);
        return CreatedAtAction(nameof(GetMessages), new { senderId = messageDto.SenderId, receiverId = messageDto.ReceiverId }, sentMessage);
    }
}
