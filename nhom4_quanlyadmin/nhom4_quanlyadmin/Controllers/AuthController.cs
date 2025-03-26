using Microsoft.AspNetCore.Mvc;
using nhom4_quanlyadmin.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace nhom4_quanlyadmin.Controllers
{
    public class AuthController : Controller
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        // ✅ Trang Login (GET)
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        /*// ✅ Xử lý Login (POST)
        public async Task<IActionResult> Login(string username, string password)
        {
            var token = await _authService.LoginAsync(username, password);
            if (token != null)
            {
                HttpContext.Session.SetString("JWTToken", token);

                var role = _authService.GetUserRoleFromToken(token);
                HttpContext.Session.SetString("UserRole", role);

                // ✅ In ra Token và Role để kiểm tra
                Console.WriteLine("JWTToken: " + token);
                Console.WriteLine("Role: " + role);


                return RedirectToAction("Index", "Home");
            }

            ViewBag.ErrorMessage = "Invalid username or password.";
            return View();
        }*/
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            // Gọi phương thức từ AuthService để nhận token JWT
            var token = await _authService.LoginAsync(username, password);
            if (token != null)
            {
                // Lưu token vào session
                HttpContext.Session.SetString("JWTToken", token);

                // Lấy role từ token và lưu vào session
                var role = GetUserRoleFromToken(token);
                HttpContext.Session.SetString("UserRole", role);

                // Log thông tin để kiểm tra
                Console.WriteLine("JWTToken: " + token);
                Console.WriteLine("Role: " + role);

                // Điều hướng đến trang Home nếu đăng nhập thành công
                return RedirectToAction("Index", "Home");
            }

            // Nếu thông tin đăng nhập sai, hiển thị thông báo lỗi
            TempData["ErrorMessage"] = "Invalid username or password.";
            return View();
        }


        // ✅ Phương thức Logout
        public string GetUserRoleFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // ✅ Lấy Role từ Claims
            var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "role" || c.Type == ClaimTypes.Role);
            return roleClaim?.Value ?? string.Empty;
        }

    }
}
