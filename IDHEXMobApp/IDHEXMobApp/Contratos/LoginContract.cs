using Flunt.Validations;
using IDHEXMobApp.Models.Request;
using System.Diagnostics.Contracts;

namespace IDHEXMobApp.Contratos;

public class LoginContract: Contract<LoginRequest>
{
    public LoginContract(LoginRequest request)
    {
        Requires()
            .IsNotNullOrEmpty(request.name, "name", "CPF não pode ser vazio!")
            .IsNotNullOrEmpty(request.password, "password", "Senha não pode ser vazia!")
            .IsNotNullOrEmpty(request.sigla, "sigla", "Sigla não pode ser vaiza!")
            .IsTrue(request.sigla.Length == 3, "sigla", "Sigla deve ter 3 caracteres");
    }
}
