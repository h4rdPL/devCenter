using DevCenter.Application.Users;
using DevCenter.Domain.Users;
using DevCenter.Infrastructure.Data;
using DevCenter.Infrastructure.Repositories.Users;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure servicesrgh
var services = builder.Services;
var configuration = builder.Configuration;

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddControllers();

// Dependency Injection for custom services and repositories
services.AddScoped<UserServices>();
services.AddScoped<IUserRepository, UserRepository>();


// Authentication configuration
services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle(googleOptions =>
{
    googleOptions.ClientId = configuration["Authentication:Google:ClientId"] ?? string.Empty;
    googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"] ?? string.Empty;
});


// Database context configuration
services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(o =>
    {
        o.OAuthClientId(configuration["SwaggerGoogleOauth:ClientId"]);
        o.OAuthClientSecret(configuration["SwaggerGoogleOauth:Secret"]);
        o.OAuthUsePkce();
    });
}


app.UseRouting();
app.UseHttpsRedirection();
app.UseCors(it => it.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();
