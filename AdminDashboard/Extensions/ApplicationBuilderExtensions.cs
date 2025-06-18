using AdminDashboard.Authentication;
using AdminDashboard.Entities;
using AdminDashboard.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace AdminDashboard.Extensions;

public static class ApplicationBuilderExtensions
{
    public static void SeedData(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (db.Clients.Any()) return;
        db.Clients.AddRange(
            new Client("Сид Баррет", "one@test.com"),
            new Client("Чак Берри", "two@test.com"),
            new Client("Татьяна Буланова", "three@test.com"));

        db.SaveChanges();

        var clientIds = db.Clients.Select(c => c.Id).ToList();

        var random = new Random();
        for (var i = 1; i <= 5; i++)
        {
            var clientId = clientIds[random.Next(0, clientIds.Count)];
            db.Payments.Add(new Payment(clientId, random.Next(10, 1000), DateTime.Now.AddDays(-i)));
        }

        db.Rates.Add(new ExchangeRate());

        db.SaveChanges();
    }

    public static void SeedDataAuth(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Account>>();

        if (userManager.FindByEmailAsync("admin@mirra.dev").Result != null) return;
        var user = new Account
        {
            UserName = "admin@mirra.dev",
            Email = "admin@mirra.dev",
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString()
        };
        var result = userManager.CreateAsync(user, "admin123").Result;
        if (!result.Succeeded)
        {
            throw new Exception("Failed to create admin user: " + string.Join(", ",
                result.Errors.Select(e => e.Description)));
        }
    }
}