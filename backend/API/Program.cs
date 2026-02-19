using backend.Repository;
using backend.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
        builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});

builder.Services.AddDbContext<RepositoryContext>(opts =>
    opts.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(o =>
{
    o.Password.RequireDigit = true;
    o.Password.RequireLowercase = false;
    o.Password.RequireUppercase = false;
    o.Password.RequireNonAlphanumeric = false;
    o.Password.RequiredLength = 10;
    o.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<RepositoryContext>()
.AddDefaultTokenProviders();

builder.Services.Configure<backend.Entities.JwtConfiguration>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddAutoMapper(typeof(backend.Application.MappingProfile));
builder.Services.AddScoped<backend.Service.Contracts.ILoggerManager, backend.Service.LoggerManager>();
builder.Services.AddScoped<backend.Repository.Contracts.IRepositoryManager, backend.Repository.RepositoryManager>();
builder.Services.AddScoped<backend.Service.Contracts.IServiceManager, backend.Service.ServiceManager>();
builder.Services.AddScoped<backend.API.ActionFilters.ValidationFilterAttribute>();

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:ValidIssuer"],
        ValidAudience = builder.Configuration["JwtSettings:ValidAudience"],
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(System.Environment.GetEnvironmentVariable("SECRET") ?? "This is a failing default secret just in case"))
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

// app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
