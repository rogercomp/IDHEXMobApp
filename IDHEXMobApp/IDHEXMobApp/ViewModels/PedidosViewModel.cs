using IDHEXMobApp.Models.Response;
using IDHEXMobApp.Repositories;
using IDHEXMobApp.Repositories.Database;

namespace IDHEXMobApp.ViewModels
{

    public partial class PedidosViewModel : BaseViewModel
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IDatabaseRepository _databaseRepository;

        [ObservableProperty]
        private string baixados = "Entregues: 0";
        [ObservableProperty]
        private string sincronizados = "Sinc:";
        [ObservableProperty]
        private string _numRomaneio = string.Empty;
        [ObservableProperty]
        string filtroPesquisa;

        public ObservableCollection<RomaneioResponse> Romaneios { get; set; } = new ObservableCollection<RomaneioResponse>();
        public ObservableCollection<RomaneioResponse> RomaneiosFiltrados { get; } = new();
        public PedidosViewModel(IPedidoRepository pedidoRepository, IDatabaseRepository databaseRepository)
        {
            _pedidoRepository = pedidoRepository;
            _databaseRepository = databaseRepository;
            //AtualizarFiltroAsync();
        }

        internal async Task InitiAsync()
        {
            IsBusy = true;

            try
            {
                var pedidos = (await _pedidoRepository.GetPedidosAsync()).ToList();
                int total = pedidos.Count;

                if (total > 0)
                {
                    // Carregue todos os romaneios já existentes de uma vez
                    var pedidosDb = _databaseRepository.GetAll().ToList();

                    var romaneiosExistentes = new HashSet<string>(
                        pedidosDb.Select(p => p.NumRomaneio ?? string.Empty)
                    );

                    // Filtre apenas os pedidos que ainda não existem no banco local
                    var novosPedidos = pedidos
                        .Where(p => !romaneiosExistentes.Contains(p.NumRomaneio ?? string.Empty))
                        .GroupBy(p => p.NumRomaneio)
                        .SelectMany(g => g)
                        .ToList();

                    total = novosPedidos.Count;

                    int contador = 0;

                    foreach (var item in novosPedidos)
                    {
                        contador++;
                        Sincronizados = $"Sincronizado(s): {contador}/{total}";
                        _databaseRepository.Add(item);
                        await Task.Delay(50); // Pequeno atraso para simular o tempo de processamento
                    }

                    // *** Recarregue o banco local após inserir novos pedidos ***
                    pedidosDb = _databaseRepository.GetAll().ToList();

                    // Atualize a lista de romaneios filtrados
                    var retorno = pedidosDb
                        .Where(p => p.ImgCanhoto == null)
                        .GroupBy(p => p.NumRomaneio)
                        .Select(g => new RomaneioResponse
                        {
                            NumRomaneio = g.Key,
                            TotalNotas = g.Count()
                        }).ToList();

                    RomaneiosFiltrados.Clear();

                    foreach (var item in retorno)
                    {
                        RomaneiosFiltrados.Add(new RomaneioResponse
                        {
                            NumRomaneio = item.NumRomaneio!,
                            TotalNotas = item.TotalNotas
                        });
                    }
                }
                else
                {
                    await CarregaRomaneiosAsync();
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Atenção", $"Erro: {ex.Message} ", "OK");
            }

            IsBusy = false;
        }

        [RelayCommand]
        public async Task GoToPedidosCleanAsync()
             => await Shell.Current.GoToAsync(nameof(NotasCleanPage));

        [RelayCommand]
        public async Task GoToNotas()
        {
            await Shell.Current.GoToAsync("//NotasPage");
        }

        [RelayCommand]
        public async Task GoToEdit(RomaneioResponse romaneio)
        {
            if (romaneio is null)
                return;

            var navigationParams = new Dictionary<string, object>
            {
                {"Romaneio", romaneio }
            };

            await Shell.Current.GoToAsync("//NotasPage", navigationParams);
        }

        public async Task CarregaRomaneiosAsync()
        {
            IsBusy = true;

            //_databaseRepository.Delete(new PedidoResponse());

            var itens = _databaseRepository.GetAll();


            var resultado = (from p in itens.Where(p => p.Baixado == "NÃO")
                             group p by new { p.NumRomaneio, p.DataPrevisaoSaida } into g
                             select new RomaneioResponse
                             {
                                 NumRomaneio = g.Key.NumRomaneio,
                                 TotalNotas = g.Count(),
                                 DataPrevisaoSaida = g.Key.DataPrevisaoSaida
                             }).ToList();

            RomaneiosFiltrados.Clear();

            foreach (var item in resultado)
            {
                RomaneiosFiltrados.Add(new RomaneioResponse
                {
                    NumRomaneio = item.NumRomaneio!,
                    TotalNotas = item.TotalNotas,
                    DataPrevisaoSaida = item.DataPrevisaoSaida
                });
            }

            Romaneios = new ObservableCollection<RomaneioResponse>(RomaneiosFiltrados);

            IsBusy = false;

            await Task.CompletedTask;
        }

        public void AtualizarFiltroAsync()
        {

            RomaneiosFiltrados.Clear();
            var termo = FiltroPesquisa?.ToLower() ?? "";
            var filtrados = string.IsNullOrWhiteSpace(termo)
                ? Romaneios
                : Romaneios.Where(x =>
                    (x.NumRomaneio?.Contains(termo) ?? false)
                );
            foreach (var item in filtrados)
                RomaneiosFiltrados.Add(item);
        }
    }
}
