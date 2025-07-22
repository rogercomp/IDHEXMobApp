using IDHEXMobApp.Repositories.Services;

namespace IDHEXMobApp.Views;

public partial class PrincipalPage : ContentPage
{
	private readonly PedidoService _pedidoService;
    public PrincipalPage(PrincipalViewModel viewModel, PedidoService pedidoService)
	{
		InitializeComponent();
		BindingContext = viewModel;
		_pedidoService = pedidoService;
    }   
}
