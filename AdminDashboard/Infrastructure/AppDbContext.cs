using AdminDashboard.Entities;
using Microsoft.EntityFrameworkCore;

namespace AdminDashboard.Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<ExchangeRate> Rates => Set<ExchangeRate>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ExchangeRate>()
            .ToTable(t => t.HasCheckConstraint("CK_ExchangeRate_SingleRow", "Id = 1"));
    }
}