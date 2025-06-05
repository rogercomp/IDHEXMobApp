using Flurl;
using Flurl.Http;
using IDHEXMobApp.Helpers;
using IDHEXMobApp.Models.Request;
using IDHEXMobApp.Models.Response;


namespace IDHEXMobApp.Repositories.Login;

public class LoginRepository: ILoginRepository
{
    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var response = await Constantes.BaseUrl
            .AppendPathSegment("/Account/login")
            .PostJsonAsync(request);

        if(response.ResponseMessage.IsSuccessStatusCode)
        {
          var content = await response.ResponseMessage.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<LoginResponse>(content) ?? new LoginResponse();

        }

        return new LoginResponse();
    }
}
