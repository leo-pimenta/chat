using Domain;
using Infra.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Name
{
    [ApiController]
    [Route("/api/messages")]
    public class MessageController : ControllerBase
    {
        private readonly IMessageRepository<Message> MessageRepository;

        public MessageController(IMessageRepository<Message> messageRepository)
        {
            this.MessageRepository = messageRepository;
        }

        [HttpPost]
        public IActionResult NewMessage(MessageRequestDto dto)
        {
            var message = new Message(dto.SenderId, dto.TargetId, dto.Body);
            this.MessageRepository.Add(message, message.TargetId);
            return Ok();
        }
    }
}