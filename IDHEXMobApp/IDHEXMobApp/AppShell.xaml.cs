namespace IDHEXMobApp;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(BlankPage), typeof(BlankPage));
	}
}
