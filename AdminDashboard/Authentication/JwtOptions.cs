namespace AdminDashboard.Authentication;

public class JwtOptions
{
    public string Key { get; set; } = string.Empty;
    public int ExpireMinutes { get; set; }
}