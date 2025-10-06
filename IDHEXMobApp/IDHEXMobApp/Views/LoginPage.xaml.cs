namespace IDHEXMobApp.Views;

public partial class LoginPage : ContentPage
{
    private LoginViewModel _viewModel;
    public LoginPage(LoginViewModel viewModel)
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