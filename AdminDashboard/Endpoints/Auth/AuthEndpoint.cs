using AdminDashboard.Authentication;
using Microsoft.AspNetCore.Identity;

namespace AdminDashboard.Endpoints.Auth;

public static class AuthEndpoint
{
    public static async Task<IResult> Login(
        LoginRequest request,
        UserManager<Account> userManager,
        SignInManager<Account> signInManager,
        JwtService jwtService)
    {

        if (string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password))
            return Results.BadRequest("email or password is null or empty");
        
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user == null)
            return Results.Unauthorized();

        var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!result.Succeeded)
            return Results.Unauthorized();

        var token = jwtService.GenerateToken(user);
        return Results.Ok(new { token });
    }
}