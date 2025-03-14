using Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Models;
using Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

// Obtener configuración de claves JWT
var secretKey = builder.Configuration["JwtSettings:SecretKey"];
if (string.IsNullOrEmpty(secretKey))
{
    throw new Exception("JWT SecretKey no está configurada en appsettings.json.");
}
var key = Encoding.ASCII.GetBytes(secretKey);

// Configurar autenticación JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

// Configuración de servicios
builder.Services.AddScoped<IAuthService>(provider =>
{
    var context = provider.GetRequiredService<DBManagement>();
    return new AuthService(context, secretKey);
});

builder.Services.AddScoped<IUsersService>(provider =>
{
    var context = provider.GetRequiredService<DBManagement>();
    var httpContextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
    var authService = provider.GetRequiredService<IAuthService>();
    return new UsersService(context, secretKey, httpContextAccessor, authService);
});

builder.Services.AddScoped<ITaskService>(provider =>
{
    var context = provider.GetRequiredService<DBManagement>();
    var httpContextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
    var authService = provider.GetRequiredService<IAuthService>();
    return new TaskService(context, secretKey, httpContextAccessor, authService);
});

// Base de datos en memoria (cambiar si usas una real)
builder.Services.AddDbContext<DBManagement>(options =>
    options.UseInMemoryDatabase("TaskAppDB"));

var app = builder.Build();

// Habilitar Swagger en todos los entornos
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Configurar puerto dinámico para Heroku
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
app.Run($"http://*:{port}");
