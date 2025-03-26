using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace nhom4_quanlyadmin.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // ✅ Login API
        public async Task<string?> LoginAsync(string username, string password)
        {
            var response = await _httpClient.PostAsJsonAsync("/login", new { Username = username, Password = password });

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<JsonElement>();
                if (result.TryGetProperty("token", out var token))
                {
                    return token.GetString();
                }
            }

            // Trả về null nếu không thành công hoặc thêm thông báo lỗi
            var error = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Login failed: " + error);
            return null;
        }


        // ✅ Lấy Role từ JWT Token
        public string GetUserRoleFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Kiểm tra cả hai loại claim 'role' và 'ClaimTypes.Role'
            var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "role" || c.Type == ClaimTypes.Role);
            return roleClaim?.Value ?? string.Empty;
        }

    }
}
