namespace IDHEXMobApp.Models.Request;

public class LoginRequest
{
    public LoginRequest(string _name, string _password, string _sigla)
    {
        name = _name;
        password = _password;
        sigla = _sigla;      
    }

    public string sigla { get; private set; } = string.Empty;
    public string name { get; private set; } = string.Empty;
    public string password { get; private set; } = string.Empty;
    
}
