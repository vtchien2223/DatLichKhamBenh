using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Nhom4_QLBA_API.Models;
using Microsoft.AspNetCore.Authorization;

namespace Nhom4_QLBA_API.Models
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthenticateController(
            UserManager<User> userManager,
            RoleManager<ApplicationRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status400BadRequest, new
                {
                    Status = false,
                    Message = "User already exists"
                });

            var user = new User
            {
                UserName = model.Username,
                Email = model.Email,
                Initials = model.Initials
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Status = false,
                    Message = "User creation failed"
                });

            // Assign Role if Provided 
            if (!string.IsNullOrEmpty(model.Role))
            {
                if (!await _roleManager.RoleExistsAsync(model.Role))
                {
                    await _roleManager.CreateAsync(new ApplicationRole { Name = model.Role }); // Sử dụng ApplicationRole
                }
                await _userManager.AddToRoleAsync(user, model.Role);
            }

            return Ok(new { Status = true, Message = "User created successfully" });
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return Unauthorized(new
                {
                    Status = false,
                    Message = "Invalid username or password"
                });
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));

                var role = await _roleManager.FindByNameAsync(userRole);
                if (role != null)
                {

                    if (role.CanManageDoctors)
                        authClaims.Add(new Claim("Permission", "CanManageDoctors"));
                    if (role.CanManagePatients)
                        authClaims.Add(new Claim("Permission", "CanManagePatients"));
                    if (role.CanManageAppointments)
                        authClaims.Add(new Claim("Permission", "CanManageAppointments"));
                    if (role.CanManageServices)
                        authClaims.Add(new Claim("Permission", "CanManageServices"));
                    if (role.CanManageSpecialty)
                        authClaims.Add(new Claim("Permission", "CanManageSpecialty"));
                    if (role.CanManageAppointmentDetails)
                        authClaims.Add(new Claim("Permission", "CanManageAppointmentDetails"));
                    if (role.CanManagePosts)
                        authClaims.Add(new Claim("Permission", "CanManagePosts"));
                }
            }

            var token = GenerateToken(authClaims);

            return Ok(new
            {
                Status = true,
                Message = "Logged in successfully",
                Token = token,
                Name = user.UserName,
                Email = user.Email,
                Role = userRoles.FirstOrDefault()
            });
        }

        private string GenerateToken(IEnumerable<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("JWTKey");
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(Convert.ToDouble(jwtSettings["TokenExpiryTimeInHour"])),
                Issuer = jwtSettings["ValidIssuer"],
                Audience = jwtSettings["ValidAudience"],
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}