namespace AdminDashboard.Entities;

public class Payment(Guid clientId, decimal amount, DateTime date)
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid ClientId { get; private set; } = clientId;
    public decimal Amount { get; private set; } = amount;
    public DateTime Date { get; private set; } = date;
}