namespace IDHEXMobApp.ViewModels;

public partial class PrincipalViewModel: BaseViewModel
{
    
    [RelayCommand]
    public async Task GoToPedidosAsync()
         => await Shell.Current.GoToAsync(nameof(PedidosPage));

    [RelayCommand]
    public async Task GoToNotasAsync()
         => await Shell.Current.GoToAsync(nameof(NotasPage));
}
