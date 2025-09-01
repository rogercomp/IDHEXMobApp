
namespace IDHEXMobApp.Views;

public partial class PedidosBaixaPage : ContentPage
{
    private PedidosBaixaViewModel _viewModel;
    public PedidosBaixaPage(PedidosBaixaViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        DeviceDisplay.Current.KeepScreenOn = true;
        await _viewModel.InitiAsync();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        DeviceDisplay.Current.KeepScreenOn = false;
    }
}