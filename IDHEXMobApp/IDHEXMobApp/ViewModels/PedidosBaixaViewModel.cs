using CommunityToolkit.Maui.Core.Extensions;
using IDHEXMobApp.Models.Response;
using IDHEXMobApp.Repositories;
using IDHEXMobApp.Repositories.Database;

namespace IDHEXMobApp.ViewModels
{
    public partial class PedidosBaixaViewModel : BaseViewModel
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IDatabaseRepository _databaseRepository;

        [ObservableProperty]
        private string pendentes = "Pendentes Envio: 0";
        [ObservableProperty]
        private string _numRomaneio = string.Empty;
        public ObservableCollection<PedidoResponse> Pedidos { get; set; } = new ObservableCollection<PedidoResponse>();

        public PedidosBaixaViewModel(IPedidoRepository pedidoRepository, IDatabaseRepository databaseRepository)
        {
            _pedidoRepository = pedidoRepository;
            _databaseRepository = databaseRepository;
        }

        internal async Task InitiAsync()
        {
            IsBusy = true;
            
            int contador = 0;
            Pedidos =  _databaseRepository.GetAll().Where(p=> p.Baixado == "SIM").ToObservableCollection<PedidoResponse>();

            int Total = Pedidos.Count;

            if (Total > 0)
            {
                contador++;
                await Task.Delay(1000);
                Pendentes = $"Pendentes Envio: {Total}";            
            }

            OnPropertyChanged(nameof(Pedidos));

            IsBusy = false;
        }
    }
}
