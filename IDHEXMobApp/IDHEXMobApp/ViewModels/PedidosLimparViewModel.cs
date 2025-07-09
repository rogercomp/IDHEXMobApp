using CommunityToolkit.Maui.Core.Extensions;
using IDHEXMobApp.Repositories.Database;

namespace IDHEXMobApp.ViewModels
{
    public partial class PedidosLimparViewModel : BaseViewModel
    {
        private readonly IDatabaseRepository _databaseRepository;
        public PedidosLimparViewModel(IDatabaseRepository databaseRepository)
        {
            _databaseRepository = databaseRepository;
        }

        internal async Task InitiAsync()
        {
            IsBusy = true;

            _databaseRepository.DeleteAll(new Models.Response.PedidoResponse());

            IsBusy = false;

            await Shell.Current.DisplayAlert("Limpeza", "Todos os pedidos foram limpos com sucesso.", "OK");

        }
    }
}
