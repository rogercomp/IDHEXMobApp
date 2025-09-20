using IDHEXMobApp.ViewModels;
using System.Collections.Specialized;

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
        //_viewModel.PedidosFiltrados.CollectionChanged += OnItemsCollectionChanged;
    }

    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        _viewModel.FiltroPesquisa = e.NewTextValue.ToLower();
        _viewModel.AtualizarFiltroAsync();
    }

    //private void OnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    //{
    //    // Rola para o final se a coleção for alterada (itens adicionados)
    //    if (e.Action == NotifyCollectionChangedAction.Add)
    //    {
    //        // Isso precisa ser feito no thread principal da UI
    //        Dispatcher.Dispatch(async () =>
    //        {
    //            // Atraso para garantir que a UI tenha sido atualizada
    //            await Task.Delay(50);
    //            await MyScrollView.ScrollToAsync(0, MyScrollView.ContentSize.Height, true);
    //        });
    //    }
    //}
}