using Dal.Models.DtoModels;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace ABPTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]  
    public class ExperementController : Controller
    {
        private readonly ITokenService _tokenService;
        private readonly IExperementService _experementService;

        public ExperementController(ITokenService tokenService, IExperementService experementService)
        {
            _tokenService = tokenService;
            _experementService = experementService;
        }

        [HttpGet("button_color")]
        public async Task<ActionResult<ViewExperement>> GetButtonColor(string token)
        {
            await _tokenService.TryToCreateNewToken(token);

            return Ok(await _experementService.GetExperementByTokenAndName("button_color", token));
        }

        [HttpGet("price")]
        public async Task<ActionResult<ViewExperement>> GetPrice(string token)
        {
            await _tokenService.TryToCreateNewToken(token);

            return Ok(await _experementService.GetExperementByTokenAndName("price", token));
        }
    }
}
