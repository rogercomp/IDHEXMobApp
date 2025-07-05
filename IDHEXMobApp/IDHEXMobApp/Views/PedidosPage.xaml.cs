namespace IDHEXMobApp.Views;

public partial class PedidosPage : ContentPage
{
    private PedidosViewModel _viewModel;
    public PedidosPage(PedidosViewModel viewModel)
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