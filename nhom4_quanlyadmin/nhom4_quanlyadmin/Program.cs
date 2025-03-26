using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using nhom4_quanlyadmin.Services;

var builder = WebApplication.CreateBuilder(args);

// Cấu hình CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyAllowOrigins", policy =>
    {
        policy.WithOrigins("http://127.0.0.1:5500", "http://localhost:5500") // Địa chỉ frontend
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ✅ Đăng ký ApiService
builder.Services.AddHttpClient<ApiService>(client =>
{
    var apiBaseUrl = builder.Configuration["APIBaseUrl"];
    if (string.IsNullOrEmpty(apiBaseUrl))
    {
        throw new ArgumentNullException("APIBaseUrl is not configured in appsettings.json");
    }
    client.BaseAddress = new Uri(apiBaseUrl);
});

// ✅ Đăng ký AuthService
builder.Services.AddHttpClient<AuthService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["APIBaseUrl"]);
});

// ✅ Đăng ký HttpContextAccessor và Session
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

// ✅ Cấu hình JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JWT");
var key = Encoding.UTF8.GetBytes(jwtSettings["Secret"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Bearer";
    options.DefaultChallengeScheme = "Bearer";
})
.AddJwtBearer("Bearer", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true, // Kiểm tra thời gian hết hạn của token
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero // Không có độ trễ khi hết hạn token
    };
});



// ✅ Cấu hình Authorization
builder.Services.AddAuthorization();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// ✅ Sử dụng HTTPS và Static Files
app.UseHttpsRedirection();
app.UseCors("MyAllowOrigins");
// ✅ Cấu hình Routing
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();
app.UseStaticFiles();


// ✅ Định tuyến đến trang Login mặc định
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.MapControllerRoute(
    name: "home",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "doctor",
    pattern: "{controller=Doctor}/{action=Index}/{id?}");


app.Run();
