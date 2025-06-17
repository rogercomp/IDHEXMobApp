using IDHEXMobApp.Models.Request;
using IDHEXMobApp.Models.Response;

namespace IDHEXMobApp.Repositories;

public interface ILoginRepository
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
    
}
