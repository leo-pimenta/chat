using Domain;
using Infra.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Name
{
    [Controller]
    [Route("/api/ws")]
    public class WSManagerController : ControllerBase
    {
        private readonly IWSConnectionRepository ConnectionRepository;

        public WSManagerController(IWSConnectionRepository connectionRepository)
        {
            this.ConnectionRepository = connectionRepository;
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Connect(string id)
        {
            if (!this.Request.Headers.ContainsKey("user_id") || string.IsNullOrWhiteSpace(this.Request.Headers["user_id"]))
            {
                return Unauthorized();
            }

            var userId = this.Request.Headers["user_id"];
            await this.ConnectionRepository.AddAsync(new WSConnection(id, userId));
            return Ok(new { message = $"Connected to BE with Id {id} and user id: {userId}."});
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Disconnect(string id)
        {
            await this.ConnectionRepository.DeleteAsync(id);
            return Ok(new { message = $"Disconnected from BE with Id {id}" });
        }
    }
}