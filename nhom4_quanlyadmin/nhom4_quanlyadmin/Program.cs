using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using nhom4_quanlyadmin.Services;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
	options.AddPolicy("MyAllowOrigins", policy =>
	{
		policy.WithOrigins("http://127.0.0.1:5500", "http://localhost:5500") // Địa chỉ frontend
			  .AllowAnyHeader()
			  .AllowAnyMethod();
	});
});

builder.Services.AddHttpClient<ApiService>(client =>
{
	var apiBaseUrl = builder.Configuration["APIBaseUrl"];
	if (string.IsNullOrEmpty(apiBaseUrl))
	{
		throw new ArgumentNullException("APIBaseUrl is not configured in appsettings.json");
	}
	client.BaseAddress = new Uri(apiBaseUrl);
});
builder.Services.AddScoped<AuthService>();
builder.Services.AddHttpClient<AuthService>(client =>
{
	client.BaseAddress = new Uri(builder.Configuration["APIBaseUrl"]);
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
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
builder.Services.AddAuthorization();
builder.Services.AddControllersWithViews();
var app = builder.Build();
app.UseHttpsRedirection();

// Middleware xử lý lỗi 403
app.UseExceptionHandler(errorApp =>
{
	errorApp.Run(async context =>
	{
		var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
		var exception = exceptionHandlerPathFeature?.Error;

		if (exception is HttpRequestException httpException && httpException.Message.Contains("403"))
		{
			context.Response.Redirect("/Home/Error?statusCode=403&message=Bạn không có quyền truy cập chức năng này");
		}
		else
		{
			context.Response.Redirect("/Home/Error?statusCode=500&message=Đã xảy ra lỗi trong quá trình xử lý yêu cầu");
		}
	});
});

app.UseRouting();
app.UseCors("MyAllowOrigins");
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
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
