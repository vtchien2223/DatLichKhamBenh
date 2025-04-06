using Microsoft.AspNetCore.Mvc;
using nhom4_quanlyadmin.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace nhom4_quanlyadmin.Controllers
{
	public class AuthController : Controller
	{
		private readonly AuthService _authService;
		private readonly ILogger<AuthController> _logger;

		public AuthController(AuthService authService, ILogger<AuthController> logger)
		{
			_authService = authService;
			_logger = logger;
		}

		// GET: /Auth/Login
		[HttpGet]
		public IActionResult Login()
		{
			// Clear existing session
			HttpContext.Session.Clear();
			return View();
		}

		// POST: /Auth/Login
		[HttpPost]
		public async Task<IActionResult> Login(string username, string password)
		{
			try
			{
				if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
				{
					TempData["ErrorMessage"] = "Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu.";
					return View();
				}

				_logger.LogInformation($"Attempting login for user: {username}");
				var token = await _authService.LoginAsync(username, password);

				if (!string.IsNullOrEmpty(token))
				{
					// Lưu token vào Session
					HttpContext.Session.SetString("JWTToken", token);
					_logger.LogInformation("Token saved to session");

					// Lấy và lưu role
					var role = _authService.GetUserRoleFromToken(token);
					HttpContext.Session.SetString("UserRole", role);
					_logger.LogInformation($"User role from token: {role}");

					// Debug log
					_logger.LogInformation($"Login successful. Token: {token}");
					_logger.LogInformation($"Role: {role}");

					if (!IsAuthorizedUser())
					{
						TempData["ErrorMessage"] = "Bạn không có quyền truy cập hệ thống quản trị.";
						HttpContext.Session.Clear();
						return View();
					}

					return RedirectToAction("Index", "Home");
				}

				TempData["ErrorMessage"] = "Tên đăng nhập hoặc mật khẩu không đúng.";
				return View();
			}
			catch (Exception ex)
			{
				_logger.LogError($"Login error: {ex.Message}");
				TempData["ErrorMessage"] = "Có lỗi xảy ra trong quá trình đăng nhập.";
				return View();
			}
		}
		private bool IsAuthorizedUser()
		{
			var role = HttpContext.Session.GetString("UserRole");
			return role == "Admin" || role == "Mod";
		}

		// ✅ Hàm tách Role từ JWT Token
		private string GetUserRoleFromToken(string token)
		{
			var handler = new JwtSecurityTokenHandler();
			var jwtToken = handler.ReadJwtToken(token);

			var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "role" || c.Type == ClaimTypes.Role);
			var role = roleClaim?.Value ?? string.Empty;
			_logger.LogInformation($"Extracted role from token: {role}");
			return role;
		}
	}
}
