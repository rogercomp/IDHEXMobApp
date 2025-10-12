
namespace IDHEXMobApp.Views;

public partial class NotasCleanPage : ContentPage
{
    private NotaCleanViewModel _viewModel;
    public NotasCleanPage(NotaCleanViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }       

    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        _viewModel.FiltroPesquisa = e.NewTextValue.ToLower();   
        _viewModel.AtualizarFiltroAsync();
    }   
}