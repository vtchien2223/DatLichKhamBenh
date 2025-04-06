
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Nhom4_QLBA_API;
using Nhom4_QLBA_API.Models;
using Nhom4_QLBA_API.Repositories;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Cấu hình SQL Server làm hệ quản trị cơ sở dữ liệu
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Đăng ký Identity
builder.Services.AddIdentity<User, ApplicationRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Đăng ký các Repository
builder.Services.AddControllers();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ISpecialtyRepository, SpecialtyRepository>();
builder.Services.AddScoped<IMedicalRecordRepository, MedicalRecordRepository>();
builder.Services.AddScoped<IAppointmentDetailsRepository, AppointmentDetailsRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<RoleManager<ApplicationRole>>();
builder.Services.AddScoped<UserManager<User>>();
builder.Services.AddScoped<IUserClaimsPrincipalFactory<User>, CustomUserClaimsPrincipalFactory>();
// Cấu hình Swagger để tài liệu hóa API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Cấu hình JWT trong Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Nhập 'Bearer {token}' để xác thực API."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanManageDoctors", policy =>
        policy.RequireClaim("Permission", "CanManageDoctors"));

    options.AddPolicy("CanManagePatients", policy =>
        policy.RequireClaim("Permission", "CanManagePatients"));

    options.AddPolicy("CanManageAppointments", policy =>
        policy.RequireClaim("Permission", "CanManageAppointments"));

    options.AddPolicy("CanManageServices", policy =>
        policy.RequireClaim("Permission", "CanManageServices"));

    options.AddPolicy("CanManageSpecialty", policy =>
        policy.RequireClaim("Permission", "CanManageSpecialty"));

    options.AddPolicy("CanManageAppointmentDetails", policy =>
        policy.RequireClaim("Permission", "CanManageAppointmentDetails"));

    options.AddPolicy("CanManagePosts", policy =>
        policy.RequireClaim("Permission", "CanManagePosts"));
});
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

// Cấu hình JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JWTKey");
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
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["ValidIssuer"],
        ValidAudience = jwtSettings["ValidAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Tạo các Role mặc định nếu chưa tồn tại
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
    var roles = new[] { "Admin", "User", "Mod" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            var newRole = new ApplicationRole { Name = role };
            if (role == "Admin")
            {
                // Gán quyền mặc định cho Admin
                newRole.CanManageDoctors = true;
                newRole.CanManagePatients = true;
                newRole.CanManageAppointments = true;
                newRole.CanManageServices = true;
                newRole.CanManageSpecialty = true;
                newRole.CanManageAppointmentDetails = true;
                newRole.CanManagePosts = true;
            }
            await roleManager.CreateAsync(newRole);
        }
    }
}

// Middleware xử lý lỗi
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

// Cấu hình Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("MyAllowOrigins");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

