using Microsoft.AspNetCore.Identity;

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
    }
    
    
    
    public record RegisterDto(string Email, string Password);
    public record LoginDto(string Email, string Password);
}