
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
        await _viewModel.InitiAsync();
    }
}