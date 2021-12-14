using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Payment.Models;
using Payment.Models.Requests;
using Payment.Services.Auth;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Payment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        // POST api/<AuthController>
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRequest user)
        {
            if (ModelState.IsValid)
            {

                var existUser = await authService.UserManager().FindByEmailAsync(user.Email);

                if (existUser != null)
                {
                    return BadRequest(
                        new BasicResponse()
                        {
                            Message = "User already created",
                            Success = false
                        });
                }

                var newUser = new IdentityUser()
                {
                    Email = user.Email,
                    UserName = user.Username
                };

                var isCreated = await authService.UserManager().CreateAsync(newUser, user.Password);
                if (isCreated.Succeeded)
                {
                    //var jwToken = await authService.GenerateJwToken(newUser);
                    //return Ok(jwToken);
                    return Ok(new
                    {
                        Message = "Account created, now you can login",
                        Success = true
                    });
                }
                return BadRequest(
                    new
                    {
                        Message = "Failed to register account",
                        Success = false,
                        Errors = isCreated.Errors.Select(x => x.Description).ToList()
                    });
            }
            return BadRequest(
                new BasicResponse()
                {
                    Message = "Invalid payload",
                    Success = false
                });
        }

        // POST api/<AuthController>
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest user)
        {
            if (ModelState.IsValid)
            {

                var existingUser = await authService.UserManager().FindByEmailAsync(user.Email);

                if (existingUser == null)
                {
                    return NotFound(
                        new BasicResponse()
                        {
                            Message = "User not found",
                            Success = false
                        });
                }

                var isCorrect = await authService.UserManager()
                    .CheckPasswordAsync(existingUser, user.Password);

                if (!isCorrect)
                {
                    return BadRequest(
                        new BasicResponse()
                        {
                            Message = "Email and  password not match",
                            Success = false
                        });
                }

                var jwtToken = await authService.GenerateJwToken(existingUser);

                return Ok(jwtToken);
            }

            return BadRequest(
                new BasicResponse()
                {
                    Message = "Invalid payload",
                    Success = false
                });
        }

        [HttpPost]
        [Route("CheckToken")]
        public async Task<IActionResult> CheckToken([FromBody] TokenRequest tokenRequest)
        {
            if (ModelState.IsValid)
            {
                var result = await authService.CheckJwtToken(tokenRequest);

                return Ok(result);

            }
            return BadRequest(
           new BasicResponse()
           {
               Message = "invalid payload",
               Success = false
           });
        }

        [HttpPost]
        [Route("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequest tokenRequest)
        {
            if (ModelState.IsValid)
            {
                var result = await authService.RefreshJwtToken(tokenRequest);
                if (result == null)
                {
                    return BadRequest(
                        new BasicResponse()
                        {
                            Message = "invalid token",
                            Success = false
                        });
                }
                if (!result.Success)
                {
                    if (result.Message == "RefreshToken has been used")
                    {
                        return BadRequest(result);
                    }
                    else if (result.Message == "Token has been revoked")
                    {
                        return BadRequest(result);
                    }
                    else if (result.Message == "JWT Token is not expired")
                    {
                        return BadRequest(result);
                    }
                    //return Unauthorized(result);
                }
                return Ok(result);
            }

            return BadRequest(
               new BasicResponse()
               {
                   Message = "invalid payload",
                   Success = false
               });
        }


    }
}
