
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

    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        _viewModel.FiltroPesquisa = e.NewTextValue.ToLower();   
        _viewModel.AtualizarFiltroAsync();
    }

    //private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    //{
    //    var searchTerm = e.NewTextValue.ToLower();
    //    FilteredItems.Clear();

    //    if (string.IsNullOrEmpty(searchTerm))
    //    {
    //        foreach (var item in Items)
    //        {
    //            FilteredItems.Add(item);
    //        }
    //    }
    //    else
    //    {
    //        foreach (var item in Items)
    //        {
    //            if (item.Name.ToLower().Contains(searchTerm) || item.Description.ToLower().Contains(searchTerm))
    //            {
    //                FilteredItems.Add(item);
    //            }
    //        }
    //    }
    //}
}