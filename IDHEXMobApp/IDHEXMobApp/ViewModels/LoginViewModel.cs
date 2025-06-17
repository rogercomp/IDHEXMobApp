using IDHEXMobApp.Contratos;
using IDHEXMobApp.Models.Request;
using IDHEXMobApp.Repositories;
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

            if (String.IsNullOrEmpty(result.token))
            {
                //var toast = Toast.Make(result.message, ToastDuration.Short);

                //CancellationTokenSource cancellation = new CancellationTokenSource();

                //await toast.Show(cancellation.Token);

                await Shell.Current.DisplayAlert("Atenção", result.message, "OK");

                return;
            }

            Preferences.Set("token", result.token);

            Preferences.Set("transportadoraId", result.transportadoraId);

            Preferences.Set("motoristaId", result.motoristaId);

            await Shell.Current.GoToAsync(nameof(PrincipalPage));

        }
        catch(Exception ex)
        {
            await Shell.Current.DisplayAlert("Atenção", $"Erro: {ex.Message} ", "OK");
        }
        finally
        {
         
        }
    }

}
