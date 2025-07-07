using IDHEXMobApp.Helpers.Uteis;
using IDHEXMobApp.Repositories.Database;
using Microsoft.Extensions.Hosting;

namespace IDHEXMobApp.Repositories.Services
{
    public class PedidoService : BackgroundService
    {
        public PedidoService()
        {
            System.Diagnostics.Debug.WriteLine("PedidoService CONSTRUTOR chamado!");
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Sua lógica de background
                await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);
            }
        }
    }
}
    /*public class PedidoService : BackgroundService
    {
        readonly ILogger<PedidoService> _logger;

        public PedidoService(ILogger<PedidoService> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }/*
        //private readonly IDatabaseRepository _databaseRepository;
        //private readonly IPedidoRepository _pedidoRepository;
        //private Timer _timer = null;

        //public PedidoService(IDatabaseRepository databaseRepository, IPedidoRepository pedidoRepository)
        //{
        //    // Registre o serviço de background
        //    Debug.WriteLine("PedidoService CONSTRUTOR chamado!");
        //    _databaseRepository = databaseRepository;
        //    _pedidoRepository = pedidoRepository;
        //}

        //public Task StartAsync(CancellationToken cancellationToken)
        //{
        //    _timer = new Timer(ActionToBePerformed, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

        //    // Sua lógica de background aqui
        //    Debug.WriteLine("Serviço de background rodando: " + DateTime.Now);

        //    if (Conexao.CheckConnectivity())
        //    {
        //        var pedidos = _databaseRepository.GetAll().Where(p => p.Baixado == "NÃO" && p.ImgCanhoto is not null && p.CodOcorrencia is not null);
        //        foreach (var item in pedidos)
        //        {
        //            bool ok = _pedidoRepository.AtualizaPedidoAsync(item.PedidoId, item.EmpresaId, DateTime.Now.ToString("dd/MM/yyyy"), item.CodOcorrencia!, item.ImgCanhoto!).GetAwaiter().GetResult();
        //            if (ok)
        //                _databaseRepository.Delete(item);
        //        }
        //    }
        //    Debug.WriteLine("Serviço de background finalizado: " + DateTime.Now);

        //    return Task.CompletedTask;
        //}

        //void ActionToBePerformed(object state)
        //{
        //    Debug.WriteLine("Working behind the scenes...");
        //}

        //public Task StopAsync(CancellationToken cancellingToken)
        //{
        //    _timer?.Change(0, 2000);
        //    return Task.CompletedTask;
        //}
    }
}*/

    