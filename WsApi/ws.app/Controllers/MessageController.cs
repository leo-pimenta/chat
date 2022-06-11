using System.Text.Json;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace Name
{
    [ApiController]
    [Route("/api/messages")]
    public class MessageController : ControllerBase
    {
        [HttpPost]
        public IActionResult NewMessage(MessageRequestDto dto)
        {
            var message = new Message(dto.SenderId, dto.TargetId, dto.Body);
            Console.WriteLine(JsonSerializer.Serialize(message));
            return Ok(new { message = "Successfully requested to BE." });
        }
    }
}