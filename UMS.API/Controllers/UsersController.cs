using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UMS.Application.DTOs;
using UMS.Application.Services;
using UMS.Domain.Entities;
using UMS.Domain.Interfaces;
using UMS.Infrastructure.Data.Repositories;

namespace UMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IunitOfWork _unitOfWork;
        private readonly JwtTokenHelper _jwtTokenHelper;
        public UsersController(IunitOfWork unitOfWork, JwtTokenHelper jwtTokenHelper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _jwtTokenHelper = jwtTokenHelper; ;
        }
       

      
        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] RegisterUserDto registerUserDto)
        {
            // Validate the incoming request model
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Error = "Invalid data.", Details = ModelState });
            }

            try
            {
                // Check if the user already exists
                var existingUser = await _unitOfWork.User.GetUserByUsernameAsync(registerUserDto.Username);
                if (existingUser != null)
                {
                    return BadRequest(new { Error = "Username already exists." });
                }

                // Hash the password securely
                var hashedPassword = HashPassword(registerUserDto.Password);

                // Create the new user entity
                var newUser = new User
                {
                    Username = registerUserDto.Username,
                    PasswordHash = hashedPassword,
                    FirstName = registerUserDto.FirstName,
                    LastName = registerUserDto.LastName,
                    Device = registerUserDto.Device,
                    IPAddress = registerUserDto.IPAddress,
                    Balance = 5.0m // Add 5 GBP as a welcome bonus
                };

                // Save the user to the database
                _unitOfWork.User.Add(newUser);
                await _unitOfWork.CompleteAsync();

                return Ok(new { Message = "User registered successfully." });
            }
            catch (Exception ex)
            {
                // Log the exception (consider using a logging library)
                // Log.Error("Signup error: " + ex.Message);
                return StatusCode(500, new { Error = "An unexpected error occurred." });
            }
        }
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateUserDto authenticateUserDto)
        {
            try
            {
                // Validate the input
                if (string.IsNullOrEmpty(authenticateUserDto.Username))
                {
                    return BadRequest(new { Error = "Username is required." });
                }
                if (string.IsNullOrEmpty(authenticateUserDto.Password))
                {
                    return BadRequest(new { Error = "Password is required." });
                }

                // Authenticate the user and get the response DTO
                var authResponse = await AuthenticateUserAsync(authenticateUserDto.Username, authenticateUserDto.Password);

                // Return the response with the user's details and token
                return Ok(new
                {
                    firstName = authResponse.FirstName, 
                    lastName = authResponse.LastName,
                    token = authResponse.Token
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        public async Task<AuthenticateResponseDto> AuthenticateUserAsync(string username, string password)
        {
            var user = await _unitOfWork.User.GetUserByUsernameAsync(username);

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            // Verify the password
            if (!BCrypt.Net.BCrypt.Verify(password + "^XX4-44p", user.PasswordHash))
            {
                throw new Exception("Invalid password.");
            }

            // Generate the JWT token
            var token = _jwtTokenHelper.GenerateToken(user.Username, user.UserId, user.FirstName, user.LastName);

            // Return the response DTO with user's details and token
            return new AuthenticateResponseDto(user.FirstName, user.LastName, token);
        }
        [Authorize]
        [HttpGet("balance")]
        public async Task<IActionResult> GetBalance()
        {
            try
            {
                // Extract username from the JWT token claims
                var username = User.Identity.Name;

                if (string.IsNullOrEmpty(username))
                {
                    return BadRequest(new { Error = "Invalid token." });
                }

                // Retrieve the user by username
                var user = await _unitOfWork.User.GetUserByUsernameAsync(username);
                if (user == null)
                {
                    return NotFound(new { Error = "User not found." });
                }

                // Return the user's balance
                return Ok(new { balance = user.Balance });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        private string HashPassword(string password)
        {
            // Hash the password with a generated salt
            string salt = BCrypt.Net.BCrypt.GenerateSalt(6);
            return BCrypt.Net.BCrypt.HashPassword(password + "^XX4-44p", salt);
        }

    }
}
