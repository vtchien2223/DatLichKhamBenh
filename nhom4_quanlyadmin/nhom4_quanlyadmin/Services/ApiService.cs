/*namespace nhom4_quanlyadmin.Services
{
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;

    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Phương thức GET để lấy dữ liệu từ API
        public async Task<T> GetAsync<T>(string endpoint)
        {
            return await _httpClient.GetFromJsonAsync<T>(endpoint);
        }

        // Phương thức POST để gửi dữ liệu đến API
        public async Task<HttpResponseMessage> PostAsync<T>(string endpoint, T data)
        {
            var response = await _httpClient.PostAsJsonAsync(endpoint, data);

            // Đảm bảo rằng nếu phản hồi không thành công, sẽ báo lỗi
            response.EnsureSuccessStatusCode();
            return response;
        }


        // Phương thức PUT để cập nhật dữ liệu qua API
        public async Task<HttpResponseMessage> PutAsync<T>(string endpoint, T data)
        {
            return await _httpClient.PutAsJsonAsync(endpoint, data);
        }

        // Phương thức DELETE để xóa dữ liệu qua API
        public async Task<HttpResponseMessage> DeleteAsync(string endpoint)
        {
            return await _httpClient.DeleteAsync(endpoint);
        }
    }

}
*/

/*using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace nhom4_quanlyadmin.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        // ✅ Phương thức GET để lấy dữ liệu từ API với token
        public async Task<T> GetAsync<T>(string endpoint)
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("JWTToken");
            if (token != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            return await _httpClient.GetFromJsonAsync<T>(endpoint);
        }

        // ✅ Phương thức POST để gửi dữ liệu đến API với token
        public async Task<HttpResponseMessage> PostAsync<T>(string endpoint, T data)
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("JWTToken");
            if (token != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            return await _httpClient.PostAsJsonAsync(endpoint, data);
        }

        // Phương thức PUT để cập nhật dữ liệu qua API
        public async Task<HttpResponseMessage> PutAsync<T>(string endpoint, T data)
        {
            return await _httpClient.PutAsJsonAsync(endpoint, data);
        }

        // Phương thức DELETE để xóa dữ liệu qua API
        public async Task<HttpResponseMessage> DeleteAsync(string endpoint)
        {
            return await _httpClient.DeleteAsync(endpoint);
        }
    }
}
*/

using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;

namespace nhom4_quanlyadmin.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        // ✅ Phương thức GET với token
        public async Task<T> GetAsync<T>(string endpoint)
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("JWTToken");
            if (token != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await _httpClient.GetFromJsonAsync<T>(endpoint);
        }


        // ✅ Phương thức POST với token
        public async Task<HttpResponseMessage> PostAsync<T>(string endpoint, T data)
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("JWTToken");
            if (token != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await _httpClient.PostAsJsonAsync(endpoint, data);
        }



        // Phương thức PUT để cập nhật dữ liệu qua API
        public async Task<HttpResponseMessage> PutAsync<T>(string endpoint, T data)
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("JWTToken");
            if (token != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return await _httpClient.PutAsJsonAsync(endpoint, data);
        }

        // Phương thức DELETE để xóa dữ liệu qua API
        public async Task<HttpResponseMessage> DeleteAsync(string endpoint)
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("JWTToken");
            if (token != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return await _httpClient.DeleteAsync(endpoint);
        }
    }
}
