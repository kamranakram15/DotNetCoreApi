using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore; 
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using System.Text;
using UMS.Domain.Interfaces;
using UMS.Infrastructure.Data;
using UMS.Infrastructure.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Get connection string
var connectionString = builder.Configuration.GetConnectionString("local");

// Configure ApplicationDbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString)); 

// Retrieve the secret key from appsettings.json
var secretKey = builder.Configuration["JwtSettings:SecretKey"];

// Check if the secret key is null or empty
if (string.IsNullOrEmpty(secretKey))
{
    throw new InvalidOperationException("The JWT secret key is not configured in appsettings.json.");
}
builder.Services.AddScoped<IunitOfWork, unitOfWork>();
// Register JwtTokenHelper with the retrieved secret key
builder.Services.AddSingleton(new JwtTokenHelper(secretKey));

// Add services to the container
builder.Services.AddControllers();

// Configure JWT Authentication
var key = Encoding.ASCII.GetBytes(secretKey);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            RequireExpirationTime = true,
            ValidateLifetime = true
        };
    });

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "JwtAuthApi", Version = "v1" });
});

var app = builder.Build();

// Enable Swagger only in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(policy =>
    {
        policy.WithOrigins("https://localhost:7109", "https://localhost:7109")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .WithHeaders(HeaderNames.ContentType);
    });
}

app.UseHttpsRedirection();
app.UseAuthentication(); // JWT Authentication middleware
app.UseAuthorization();
app.MapControllers();
app.Run();
