using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace LibraryManagement.Controllers
{
    /// <summary>
    /// Authentication API for user registration and login.
    /// Provides JWT tokens for API access.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        /// <summary>
        /// Register a new user account.
        /// </summary>
        /// <param name="model">User registration details with email and password</param>
        /// <returns>Success or error message</returns>
        /// <response code="200">User registered successfully</response>
        /// <response code="400">If registration fails (invalid email, weak password, etc.)</response>
        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return Ok(new { message = "User registered successfully" });
            }

            return BadRequest(result.Errors);
        }

        /// <summary>
        /// Login user and get JWT token for API access.
        /// </summary>
        /// <param name="model">User login credentials (email and password)</param>
        /// <returns>JWT token for authenticated requests</returns>
        /// <response code="200">Returns JWT token</response>
        /// <response code="401">If credentials are invalid</response>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var token = GenerateJwtToken(user);
                return Ok(new { token });
            }

            return Unauthorized(new { message = "Invalid email or password" });
        }

        private string GenerateJwtToken(IdentityUser user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"] ?? "ThisIsAVeryLongSecretKeyForJWTAuthenticationPurposeWithMinimumLength32Characters";
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(jwtSettings["ExpiryMinutes"] ?? "60")),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    /// <summary>
    /// Model for user registration
    /// </summary>
    public class RegisterModel
    {
        /// <summary>
        /// User email address (used as username)
        /// </summary>
        public string Email { get; set; } = string.Empty;
        
        /// <summary>
        /// User password (must be at least 6 characters with uppercase, lowercase, number, and special character)
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }

    /// <summary>
    /// Model for user login
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// User email address
        /// </summary>
        public string Email { get; set; } = string.Empty;
        
        /// <summary>
        /// User password
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }
}
