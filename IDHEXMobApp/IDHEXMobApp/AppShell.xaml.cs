namespace IDHEXMobApp;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(PrincipalPage), typeof(PrincipalPage));
        Routing.RegisterRoute(nameof(PedidosPage), typeof(PedidosPage));
        Routing.RegisterRoute(nameof(NotasPage), typeof(NotasPage));
        
    }
}
