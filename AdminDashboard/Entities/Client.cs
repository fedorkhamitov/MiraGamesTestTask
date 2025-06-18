namespace AdminDashboard.Entities;

public class Client(string name, string email)
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; } = name;
    public string Email { get; private set; } = email;
}