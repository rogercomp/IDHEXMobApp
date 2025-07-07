using IDHEXMobApp.ViewModels;

namespace IDHEXMobApp.Views;

public partial class NotasPage : ContentPage
{
    private NotaViewModel _viewModel;
    public NotasPage(NotaViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.InitiAsync();
    }

    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        _viewModel.FiltroPesquisa = e.NewTextValue.ToLower();
        _viewModel.AtualizarFiltroAsync();
    }
}