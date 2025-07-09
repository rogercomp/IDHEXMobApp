namespace IDHEXMobApp.Views;

public partial class PedidosLimparPage : ContentPage
{
    private PedidosLimparViewModel _viewModel;
    public PedidosLimparPage(PedidosLimparViewModel viewModel)
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