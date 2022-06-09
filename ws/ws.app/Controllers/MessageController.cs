using Microsoft.AspNetCore.Mvc;

namespace Name
{
    [Controller]
    [Route("/api/messages")]
    public class MessageController : ControllerBase
    {
        [HttpPost]
        public IActionResult NewMessage()
        {
            return Ok(new { message = "Successfully requested to BE." });
        }
    }
}