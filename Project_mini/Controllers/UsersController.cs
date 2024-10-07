using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Project_mini.DTO;
using Project_mini.Models; // Model User của bạn
using Project_mini.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Project_mini.Helpers;

namespace Project_mini.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository; // Dịch vụ kết nối với cơ sở dữ liệu
        private readonly IJwtHelper _jwtHelper;

        public UsersController(IConfiguration configuration, IUserRepository userRepository, IJwtHelper jwtHelper)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _jwtHelper = jwtHelper;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            User user = await _userRepository.GetUserByEmailAndPasswordAsync(loginDTO);

            if (user != null)
            {
                var token = _jwtHelper.GenerateToken(user);
                return Ok(new { token = token, message = "Login successful" });
            }
            return Unauthorized(new { message = "Invalid credentials" });
        }

    }
}