using ThroneOfCubesApi.Application.Models;

namespace ThroneOfCubesApi.Application.Services;

public class AccountService(IHttpClientFactory httpClientFactory)
{
    public async Task<JwtResponse?> Login(LoginModel loginModel)
    {
        var client = httpClientFactory.CreateClient("Account");
        var response = await client.PostAsync("/account/login", JsonContent.Create(loginModel));
        response.EnsureSuccessStatusCode();
        if (response.IsSuccessStatusCode)
        {
            var jwt = await response.Content.ReadFromJsonAsync<JwtResponse>();
            return jwt;
        }

        return null;
    }

    public async Task<JwtResponse?> Register(RegisterModel registerModel)
    {
        var client = httpClientFactory.CreateClient("Account");
        var response = await client.PostAsync("/account/register", JsonContent.Create(registerModel));
        response.EnsureSuccessStatusCode();
        if (response.IsSuccessStatusCode)
        {
            var jwt = await response.Content.ReadFromJsonAsync<JwtResponse>();
            return jwt;
        }

        return null;
    }
}
