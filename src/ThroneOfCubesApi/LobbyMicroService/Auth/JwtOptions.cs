namespace LobbyMicroService.Auth;

public class JwtOptions
{
    public string Authority { get; set; } = default!; // http://accountmicroservice:8080
    public string Audience { get; set; } = "lobby.api";
    public bool RequireHttpsMetadata { get; set; } = false;
}
