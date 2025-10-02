using API.Dtos;
using API.Errors;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ChatController : BaseApiController
    {
        private readonly IAgentService _agentService;
        private readonly IConfiguration _config;
        private bool ChatEnabled
        {
            get
            {
                var value = _config["ChatSettings:Enabled"];
                if (string.IsNullOrWhiteSpace(value)) return false;

                return bool.TryParse(value, out var result) && result;
            }
        }

        public ChatController(IConfiguration config, IAgentService agentService)
        {
            _config = config;
            _agentService = agentService;
        }

        [HttpGet("enabled")]
        public IActionResult GetChatEnabled()
        {
            return Ok(new { chatEnabled = ChatEnabled });
        }

        [HttpGet("token")]
        public IActionResult GetChatToken()
        {
            if (!ChatEnabled)
            {
                return BadRequest(new ApiResponse(503, "Chat feature disabled"));
            }

            string chatToken = Guid.NewGuid().ToString("N");
            return Ok(new { chatToken = chatToken });
        }

        [HttpPost]
        public async Task<ActionResult<QuestionToReturnDto>> Ask(QuestionDto questionDto)
        {
            if (!ChatEnabled)
            {
                return BadRequest(new ApiResponse(503, "Chat feature disabled"));
            }

            string? token = HttpContext.Request.Headers["ChatToken"];

            if (string.IsNullOrEmpty(token) || !Guid.TryParseExact(token, "N", out _))
                return Unauthorized();

            var answer = await _agentService.HandleUserInput(questionDto.Question, token);

            if (answer == null) return BadRequest(new ApiResponse(400, "Problem asking question"));

            return Ok(new QuestionToReturnDto { Answer = answer });
        }
    }
}