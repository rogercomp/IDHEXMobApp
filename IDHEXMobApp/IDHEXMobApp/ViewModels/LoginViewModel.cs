using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using IDHEXMobApp.Contratos;
using IDHEXMobApp.Models.Request;
using IDHEXMobApp.Repositories.Login;
using System.Text;

namespace IDHEXMobApp.ViewModels;

public partial class LoginViewModel: BaseViewModel
{

    [ObservableProperty]
    private string name;

    [ObservableProperty]
    private string password;

    [ObservableProperty]
    private string sigla;    

    private readonly ILoginRepository _loginRepository;

    public LoginViewModel(ILoginRepository repository)
    {
        _loginRepository = repository;
    }
    
    [RelayCommand]
    public async Task LoginAsync()
    {        
        try
        {
            var loginRequest = new LoginRequest(Name, Password, Sigla);

            var contract = new LoginContract(loginRequest);

            if (!contract.IsValid)
            {
                var messages = contract.Notifications.Select(x=> x.Message);
                var sb = new StringBuilder();
                foreach (var message in messages)
                    sb.AppendLine($"{message}\n");

                await Shell.Current.DisplayAlert("Atenção", sb.ToString(), "OK");

                return;
            }

            var result = await _loginRepository.LoginAsync(loginRequest);

            if (result is null || String.IsNullOrEmpty(result.token))
            {
                var toast = Toast.Make("Falha ao realizar login", ToastDuration.Short);
                await toast.Show();
                return;
            }
        }
        finally
        {
         
        }
    }

}
