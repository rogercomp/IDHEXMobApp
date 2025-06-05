namespace IDHEXMobApp.Models.Response;

public class LoginResponse
{
    public bool flag { get; set; }
    public string token { get; set; } = string.Empty;    
    public string message { get; set; } = string.Empty;
    public bool baixaNF { get; set; }
    public int transportadoraId { get; set; }
}
