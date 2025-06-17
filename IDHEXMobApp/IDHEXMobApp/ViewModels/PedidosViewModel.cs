using IDHEXMobApp.Models.Response;
using IDHEXMobApp.Repositories;
using System.Windows.Input;


namespace IDHEXMobApp.ViewModels
{
    public partial class PedidosViewModel : BaseViewModel
    {
        private readonly IPedidoRepository _pedidoRepository;
        public ObservableCollection<RomaneioResponse> Romaneios { get; set; } = new ObservableCollection<RomaneioResponse>();
        public ICommand NotasCommand { get; }

        public PedidosViewModel(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
            NotasCommand = new Command<RomaneioResponse>(async (romaneio) => await OnNotasAsync(romaneio));
        }

        internal async Task InitiAsync()
        {
            IsBusy = true;

            var pedidos = await _pedidoRepository.GetPedidosAsync();

            var resultado = (from p in pedidos
                             group p by new { p.NumRomaneio, p.DataPrevisaoSaida } into g
                             select new RomaneioResponse
                             {
                                 NumRomaneio = g.Key.NumRomaneio,
                                 TotalNotas = g.Count(),
                                 DataPrevisaoSaida = g.Key.DataPrevisaoSaida
                             }).ToList();

            Romaneios.Clear();

            foreach (var item in resultado)
            {
                Romaneios.Add(new RomaneioResponse
                {
                    NumRomaneio = item.NumRomaneio!,
                    TotalNotas = item.TotalNotas,
                    DataPrevisaoSaida = item.DataPrevisaoSaida
                });

            }

            IsBusy = false;
        }

        [RelayCommand]
        public async Task GoToNotasCommand()
            => await Shell.Current.GoToAsync(nameof(NotasPage));

        private async Task OnNotasAsync(RomaneioResponse romaneio)
        {
            await Shell.Current.DisplayAlert("Atenção", romaneio.NumRomaneio, "OK");
        }
    }
}
