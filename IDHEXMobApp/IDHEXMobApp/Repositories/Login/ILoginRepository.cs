using IDHEXMobApp.Models.Request;
using IDHEXMobApp.Models.Response;

namespace IDHEXMobApp.Repositories.Login;

public interface ILoginRepository
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
    
}
