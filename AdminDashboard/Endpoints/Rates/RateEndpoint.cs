using AdminDashboard.Entities;
using AdminDashboard.Infrastructure;
using AdminDashboard.Infrastructure.Dtos;

namespace AdminDashboard.Endpoints.Rates;

public static class RateEndpoint
{
    public static async Task<IResult> UpdateRate(AppDbContext db, UpdateRateRequest request)
    {
        var rate = await db.Rates.FindAsync(1) ?? 
                   db.Rates.Add(new ExchangeRate(request.Rate)).Entity;
        rate.Rate = request.Rate;
        await db.SaveChangesAsync();
        return Results.Ok(rate.Rate);
    }
}