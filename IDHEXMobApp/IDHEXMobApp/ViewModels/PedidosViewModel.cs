using IDHEXMobApp.Models.Response;
using IDHEXMobApp.Repositories.Pedido;


namespace IDHEXMobApp.ViewModels
{
    public partial class PedidosViewModel : BaseViewModel
    {       

        public ObservableCollection<RomaneioResponse> Romaneios { get; set; }
            = new ObservableCollection<RomaneioResponse>();
        private readonly IPedidoRepository _pedidoRepository;

        public PedidosViewModel(IPedidoRepository pedidoRepository)
        {            
            _pedidoRepository = pedidoRepository;            
        }

        internal async Task InitiAsync()
        {
            IsBusy = true;

            var pedidos = await _pedidoRepository.GetPedidosAsync();

            var romaneios = (from p in pedidos
                             group p by new { p.NumRomaneio, p.DataPrevisaoSaida } into g
                             select new RomaneioResponse
                             {
                                 NumRomaneio = g.Key.NumRomaneio,
                                 TotalNotas = g.Count(),
                                 DataPrevisaoSaida = g.Key.DataPrevisaoSaida
                             }).ToList();

            Romaneios.Clear();

            foreach (RomaneioResponse romaneio in romaneios)
            {
                Romaneios.Add(romaneio);
            }

            IsBusy = false;
        }

        [RelayCommand]
        public async Task GoToNotasCommand()
            => await Shell.Current.GoToAsync(nameof(NotasPage));       
    }
}
