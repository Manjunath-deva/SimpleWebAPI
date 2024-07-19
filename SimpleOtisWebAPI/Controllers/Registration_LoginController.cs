using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using SimpleOtisAPI.Domain.DTOs;
using SimpleOtisAPI.Domain.Interfaces;
using SimpleOtisWebAPI.Models;
using SimpleOtisWebAPI.Services;

namespace SimpleOtisWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Registration_LoginController : ControllerBase
    {
        private readonly IRegister_Login _register_login;
        public Registration_LoginController(IRegister_Login register_login)
        {
            _register_login = register_login;
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
                Register_LoginDBDTO user = new Register_LoginDBDTO
                {
                    User_Name = login.User_Name
                };

                Register_LoginDBDTO result = await _register_login.Login(user);

                bool isLoggedIn = PasswordHasher.VerifyPasswordHasher(login.Password, result.PasswordHash, result.PasswordSalt);

                return isLoggedIn ? StatusCode(StatusCodes.Status200OK, $"Login is Successful") : StatusCode(StatusCodes.Status500InternalServerError, $"Login is not successfull, Please check the User_Name or Password");
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
