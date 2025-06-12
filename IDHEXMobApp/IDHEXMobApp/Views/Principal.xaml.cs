namespace IDHEXMobApp.Views;

public partial class PrincipalPage : ContentPage
{
	public PrincipalPage(PrincipalViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
   
}
