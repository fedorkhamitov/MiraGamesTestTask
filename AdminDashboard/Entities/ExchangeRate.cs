namespace AdminDashboard.Entities;

public class ExchangeRate(decimal rate = 10m)
{
    public int Id { get; private set; } = 1;
    public decimal Rate { get; set; } = rate;
}