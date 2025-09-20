using IDHEXMobApp.Repositories.Services;


namespace IDHEXMobApp.ViewModels;

public partial class PrincipalViewModel: BaseViewModel
{
    public PrincipalViewModel()
    {        

    }

    [RelayCommand]
    public async Task GoToPedidosAsync()
         => await Shell.Current.GoToAsync("//PedidosPage");

    [RelayCommand]
    public async Task GoToNotasAsync()
         => await Shell.Current.GoToAsync("//NotasPage");

    [RelayCommand]
    public async Task GoToBaixarAsync()
      => await Shell.Current.GoToAsync("//PedidosBaixaPage");

    [RelayCommand]
    public async Task GoToLimparAsync()
        => await Shell.Current.GoToAsync("//PedidosLimparPage");
}
