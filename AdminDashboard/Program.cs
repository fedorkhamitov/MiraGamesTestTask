using System.Text;
using AdminDashboard.Authentication;
using AdminDashboard.Endpoints;
using AdminDashboard.Endpoints.Auth;
using AdminDashboard.Endpoints.Rates;
using AdminDashboard.Extensions;
using AdminDashboard.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddDatabase(builder.Configuration)
    .AddJwtAuthentication(builder.Configuration)
    .AddIdentityConfiguration()
    .AddSwagger();

builder.Services.AddCors(options =>
{
    options.AddPolicy("ForSpecialFrontend", cpBuilder =>
    {
        cpBuilder.WithOrigins("http://localhost:5173")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseCors("ForSpecialFrontend");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.SeedData();
app.SeedDataAuth();

app.MapPost("/auth/login", AuthEndpoint.Login);

app.MapGet("/clients", async (AppDbContext db) => await db.Clients.ToListAsync()).RequireAuthorization();

app.MapGet("/payments", async (AppDbContext db, int take = 5) =>
    await db.Payments
        .OrderByDescending(p => p.Date)
        .Take(take)
        .ToListAsync()
).RequireAuthorization();

app.MapGet("/rate", async (AppDbContext db) =>
{
    var rate = await db.Rates.FindAsync(1);
    return rate is not null ? Results.Ok(rate.Rate) : Results.NotFound();
}).RequireAuthorization();

app.MapPost("/rate", RateEndpoint.UpdateRate).RequireAuthorization();

app.Run();