using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace HabitTracker.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder routes)
    {
        // Register new user
        routes.MapPost("/auth/register", async (UserManager<IdentityUser> userManager, RegisterDto dto) =>
        {
            IdentityUser user = new IdentityUser { UserName = dto.Email, Email = dto.Email };
            var result = await userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                return Results.BadRequest(result.Errors);
            }

            return Results.Ok("User registered");
        });

        // Log in and return JWT token
        routes.MapPost("/auth/login",
            async (UserManager<IdentityUser> userManager, IConfiguration config, LoginDto dto) =>
            {
                IdentityUser? user = await userManager.FindByNameAsync(dto.Email);
                if (user == null || !await userManager.CheckPasswordAsync(user, dto.Password))
                {
                    return Results.Unauthorized();
                }

                Claim[] claims = new[] { new Claim(ClaimTypes.NameIdentifier, user.Id) };

                SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtKey"]!));
                SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                JwtSecurityToken token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.UtcNow.AddDays(2),
                    signingCredentials: credentials);
                
                string? handler = new JwtSecurityTokenHandler().WriteToken(token);
                
                return Results.Ok(new { token = handler});
            });
    }

    private record RegisterDto(string Email, string Password);

    private record LoginDto(string Email, string Password);
}