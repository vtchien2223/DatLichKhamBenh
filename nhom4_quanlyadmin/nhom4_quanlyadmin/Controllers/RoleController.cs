using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nhom4_QLBA_API.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Nhom4_QLBA_API.Controllers
{
	public class RoleController : Controller
	{
		private readonly HttpClient _httpClient;
		private readonly IConfiguration _configuration;
		private readonly ILogger<RoleController> _logger;

		public RoleController(IConfiguration configuration, ILogger<RoleController> logger)
		{
			_configuration = configuration;
			_logger = logger;
			_httpClient = new HttpClient();
			var apiUrl = _configuration["APIBaseUrl"];
			_logger.LogInformation($"API Base URL: {apiUrl}");
			_httpClient.BaseAddress = new Uri(apiUrl ?? throw new ArgumentNullException("APIBaseUrl not configured"));
		}

		private void AddJwtTokenToHeader()
		{
			var token = HttpContext.Session.GetString("JWTToken");
			_logger.LogInformation($"Current token from session: {token}");

			if (string.IsNullOrEmpty(token))
			{
				_logger.LogWarning("Token is null or empty");
				throw new UnauthorizedAccessException("Bạn chưa đăng nhập hoặc token đã hết hạn.");
			}

			// Clear existing headers first
			_httpClient.DefaultRequestHeaders.Clear();
			_httpClient.DefaultRequestHeaders.Accept.Clear();
			_httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			// Add new authorization header
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
			_logger.LogInformation("Added token to request headers");
		}

		public async Task<IActionResult> Index()
		{
			try
			{
				// Debug information
				var token = HttpContext.Session.GetString("JWTToken");
				var role = HttpContext.Session.GetString("UserRole");
				_logger.LogInformation($"Attempting to access Role Index");
				_logger.LogInformation($"Token exists: {!string.IsNullOrEmpty(token)}");
				_logger.LogInformation($"User Role: {role}");

				if (role != "Admin")
				{
					_logger.LogWarning($"User with role {role} attempted to access admin page");
					TempData["ErrorMessage"] = "Bạn không có quyền truy cập trang này.";
					return RedirectToAction("Index", "Home");
				}

				AddJwtTokenToHeader();

				// Log the full request URL
				var requestUrl = $"{_httpClient.BaseAddress}Role";
				_logger.LogInformation($"Making request to: {requestUrl}");

				var response = await _httpClient.GetAsync("Role");
				_logger.LogInformation($"API Response Status: {response.StatusCode}");

				if (response.IsSuccessStatusCode)

				{
					var roles = await response.Content.ReadFromJsonAsync<List<ApplicationRole>>();
					return View(roles);
				}
				else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
				{
					var content = await response.Content.ReadAsStringAsync();
					_logger.LogWarning($"Unauthorized response from API: {content}");
					TempData["ErrorMessage"] = "Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại.";
					return RedirectToAction("Login", "Auth");
				}

				var errorContent = await response.Content.ReadAsStringAsync();
				_logger.LogError($"API Error: {errorContent}");
				TempData["ErrorMessage"] = $"Lỗi từ API: {errorContent}";
				return View(new List<ApplicationRole>());
			}
			catch (UnauthorizedAccessException ex)
			{
				_logger.LogWarning($"Unauthorized access: {ex.Message}");
				TempData["ErrorMessage"] = ex.Message;
				return RedirectToAction("Login", "Auth");
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error in Role Index: {ex.Message}");
				TempData["ErrorMessage"] = $"Lỗi: {ex.Message}";
				return View(new List<ApplicationRole>());
			}
		}

		public async Task<IActionResult> Edit(string id)
		{
			try
			{
				AddJwtTokenToHeader();
				var response = await _httpClient.GetAsync($"Role/{id}");
				if (response.IsSuccessStatusCode)
				{
					var rolePermissions = await response.Content.ReadFromJsonAsync<RolePermissionDto>();
					return View(rolePermissions);
				}
				else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
				{
					TempData["ErrorMessage"] = "Bạn chưa đăng nhập hoặc token đã hết hạn.";
					return RedirectToAction("Login", "Auth");
				}
				return NotFound();
			}
			catch (UnauthorizedAccessException ex)
			{
				TempData["ErrorMessage"] = ex.Message;
				return RedirectToAction("Login", "Auth");
			}
			catch (Exception ex)
			{
				TempData["ErrorMessage"] = "Lỗi: " + ex.Message;
				return RedirectToAction(nameof(Index));
			}
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(string id, RolePermissionDto rolePermissions)
		{
			try
			{
				AddJwtTokenToHeader();
				var response = await _httpClient.PutAsJsonAsync($"Role/{id}", rolePermissions);
				if (response.IsSuccessStatusCode)
				{
					TempData["SuccessMessage"] = "Cập nhật quyền thành công";
					return RedirectToAction(nameof(Index));
				}
				else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
				{
					TempData["ErrorMessage"] = "Bạn chưa đăng nhập hoặc token đã hết hạn.";
					return RedirectToAction("Login", "Auth");
				}
				else
				{
					var errorContent = await response.Content.ReadAsStringAsync();
					TempData["ErrorMessage"] = $"Lỗi khi cập nhật quyền: {errorContent}";
					return View(rolePermissions);
				}
			}
			catch (UnauthorizedAccessException ex)
			{
				TempData["ErrorMessage"] = ex.Message;
				return RedirectToAction("Login", "Auth");
			}
			catch (Exception ex)
			{
				TempData["ErrorMessage"] = "Lỗi: " + ex.Message;
				return View(rolePermissions);
			}
		}
	}
}
