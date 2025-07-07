using Flurl;
using Flurl.Http;
using IDHEXMobApp.Helpers;
using IDHEXMobApp.Models.Response;

namespace IDHEXMobApp.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        public async Task<IEnumerable<PedidoResponse>> GetPedidosAsync()
        {

            Int32 motoristaId = Preferences.Get("motoristaId", 0);
            Int32 transportadoraId = Preferences.Get("transportadoraId", 0);
            string unidade = Preferences.Get("unidade", "1");

            return await Constantes.BaseUrl
                .AppendPathSegment($"/IntegraMAUI/{motoristaId}/{transportadoraId}/{unidade}")
                .WithOAuthBearerToken(Preferences.Get("token", string.Empty))
                .GetJsonAsync<IEnumerable<PedidoResponse>>();
        }
        public async Task<bool> AtualizaSincronismoAsync(long pedidoId, long empresaId)
        {
            var data = new Object();

            var response = await Constantes.BaseUrl
               .AppendPathSegment($"/IntegraMAUI/{pedidoId}/{empresaId}")
               .WithOAuthBearerToken(Preferences.Get("token", string.Empty))
               .PutJsonAsync(data);

            return response.ResponseMessage.IsSuccessStatusCode;
        }

        public async Task<bool> AtualizaPedidoAsync(long pedidoId, long empresaId, string chegada, string ocorrenciaId, string imgCanhoto)
        {
            var data = new Object();

            var response = await Constantes.BaseUrl
               .AppendPathSegment($"/IntegraMAUI/{pedidoId}/{empresaId}/{chegada}/{ocorrenciaId}/{imgCanhoto}")
               .WithOAuthBearerToken(Preferences.Get("token", string.Empty))
               .PutJsonAsync(data);

            return response.ResponseMessage.IsSuccessStatusCode;
        }
    }
}
