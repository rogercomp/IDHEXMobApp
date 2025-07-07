using IDHEXMobApp.Repositories.Services;


namespace IDHEXMobApp.ViewModels;

public partial class PrincipalViewModel: BaseViewModel
{
    public PrincipalViewModel()
    {        

    }

    [RelayCommand]
    public async Task GoToPedidosAsync()
         => await Shell.Current.GoToAsync(nameof(PedidosPage));

    [RelayCommand]
    public async Task GoToNotasAsync()
         => await Shell.Current.GoToAsync(nameof(NotasPage));
}
