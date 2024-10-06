namespace AccountMicroService.Application.Models;

public class UserAuthModel
{
    public Guid Uid { get; set; }

    public string Username { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;
}
