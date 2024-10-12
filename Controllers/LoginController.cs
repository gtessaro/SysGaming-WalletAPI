using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using SysGaming_WalletAPI.Controllers.DTO;
using SysGaming_WalletAPI.Models;
using SysGaming_WalletAPI.Services;
using System.Security.Authentication;

namespace SysGaming_WalletAPI.Controllers
{
    [ApiController]
    [Route("api/login")]
    public class LoginController(AppDbContext context, LoginService service) : ControllerBase
    {

        private readonly AppDbContext _context = context;
        private readonly LoginService _service = service;

        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginDTO loginDTO)
        {

            try
            {
                var player = await _service.DoLogin(loginDTO);

                return Ok(player);
            }
            catch (InvalidCredentialException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, new { Message = "Internal Error.", Details = ex.Message });
            }
        }
    }
}