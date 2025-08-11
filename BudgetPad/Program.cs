using BudgetPad.Data;
using BudgetPad.Helpers;
using BudgetPad.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
  // Add JWT Authentication
  c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
  {
    Name = "Authorization",
    Type = SecuritySchemeType.Http,
    Scheme = "Bearer",
    BearerFormat = "JWT",
    In = ParameterLocation.Header,
    Description = "Enter JWT token. Example: Bearer {your_token}"
  });

  c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
      {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
        },
        Array.Empty<string>()
      }
    });
});

//Controller
builder.Services.AddControllers();

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
  options.UseNpgsql(
    builder.Configuration.GetConnectionString("DefaultConnection")
 ));

builder.Services.AddScoped<IBudgetRepository, BudgetRepository>();

// Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddJwtBearer(option =>
  {
    option.TokenValidationParameters = new TokenValidationParameters
    {
      ValidateIssuerSigningKey = true,
      IssuerSigningKey = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
      ValidateIssuer = true,
      ValidateAudience = true,
      ValidAudience = builder.Configuration["Jwt:Audience"],
      ValidIssuer = builder.Configuration["Jwt:Issuer"],
    };
  });
builder.Services.AddAuthorization();

// Helper class
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserContextService, UserContextService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();