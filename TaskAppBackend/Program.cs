using Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Models;
using Services;
using System.Text;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IAuthService>(provider =>
{
    var context = provider.GetRequiredService<DBManagement>();
    var configuration = provider.GetRequiredService<IConfiguration>();
    var secretKey = configuration["JwtSettings:SecretKey"];
    return new AuthService(context, secretKey);
});

builder.Services.AddScoped<IUsersService>(provider =>
{
    var context = provider.GetRequiredService<DBManagement>();
    var configuration = provider.GetRequiredService<IConfiguration>();
    var secretKey = configuration["JwtSettings:SecretKey"];
    var httpContextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
    var authService = provider.GetRequiredService<IAuthService>(); 
    return new UsersService(context, secretKey, httpContextAccessor, authService);
});

builder.Services.AddScoped<ITaskService>(provider =>
{
    var context = provider.GetRequiredService<DBManagement>();
    var configuration = provider.GetRequiredService<IConfiguration>();
    var secretKey = configuration["JwtSettings:SecretKey"];
    var httpContextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
    var authService = provider.GetRequiredService<IAuthService>();
    return new TaskService(context, secretKey, httpContextAccessor, authService);
});

builder.Services.AddDbContext<DBManagement>(options =>
    options.UseInMemoryDatabase("TaskAppDB"));

// Configuración de JWT
var secretKey = builder.Configuration["JwtSettings:SecretKey"];
var key = Encoding.ASCII.GetBytes(secretKey);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Asegúrate de que UseAuthentication viene antes de UseAuthorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();