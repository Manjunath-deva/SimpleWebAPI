using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using SimpleOtisAPI.Domain.DTOs;
using SimpleOtisAPI.Domain.Interfaces;
using SimpleOtisWebAPI.Models;
using SimpleOtisWebAPI.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SimpleOtisWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Registration_LoginController : ControllerBase
    {
        private readonly IRegister_Login _register_login;
        private readonly IConfiguration _configuration;
        public Registration_LoginController(IRegister_Login register_login, IConfiguration configuration)
        {
            _register_login = register_login;
            _configuration = configuration;
        }
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody]Register_LoginDTO register)
        {
            try
            {
                byte[] PasswordHash, PasswordSalt;
                PasswordHasher.CreatePasswordHasher(register.Password, out PasswordHash, out PasswordSalt);

                Register_LoginDBDTO user = new()
                {
                    User_Name = register.User_Name,
                    Azure_ID = Guid.NewGuid().ToString(),
                    PasswordHash = PasswordHash,
                    PasswordSalt = PasswordSalt,
                };

                var result = await _register_login.Register(user);
                return result ? Ok(user) : StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                var response = new
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Message = $"Internal Server Error",
                    ErrorMessage = ex.Message
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login(Register_LoginDTO login)
        {
            try
            {
                Register_LoginDBDTO loginUser = new Register_LoginDBDTO
                {
                    User_Name = login.User_Name
                };

                Register_LoginDBDTO user = await _register_login.Login(loginUser);

                bool isLoggedIn = PasswordHasher.VerifyPasswordHasher(login.Password, user.PasswordHash, user.PasswordSalt);

                var returnResult = new Object();

                if(isLoggedIn && user!=null)
                {
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Iss, _configuration["Jwt:Issuer"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim("UserName", user.User_Name),
                        new Claim("AzureID", user.Azure_ID),
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
                    var token = new JwtSecurityToken(
                            _configuration["Jwt:Issuer"],
                            _configuration["Jwt:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddMinutes(60),
                            signingCredentials: signIn
                      );

                    var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

                    returnResult = Ok(new {Token = tokenValue, user});
                }

                return (IActionResult)returnResult;

                //return isLoggedIn ? StatusCode(StatusCodes.Status200OK, $"Login is Successful") : StatusCode(StatusCodes.Status500InternalServerError, $"Login is not successfull, Please check the User_Name or Password");
            }
            catch (Exception ex)
            {
                var response = new
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Message = "Internal Server Error",
                    ErrorMessage = ex.Message,
                    InnerExpection = ex.InnerException
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
    }
}
