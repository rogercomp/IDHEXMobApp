using Flurl;
using Flurl.Http;
using IDHEXMobApp.Helpers;
using IDHEXMobApp.Models.Response;
using System.Net;
using System.Text.Encodings.Web;

namespace IDHEXMobApp.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        public async Task<IEnumerable<PedidoResponse>> GetPedidosAsync()
        {

            Int32 motoristaId = Preferences.Get("motoristaId", 0);
            Int32 transportadoraId = Preferences.Get("transportadoraId", 0);
            Int32 unidade = 1;      

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

        public async Task<bool> AtualizaPedidoAsync(long pedidoId, long empresaId, string ocorrenciaId, string imgCanhoto, string? dtChegada)
        {
            var data = new Object();

            //string? dtChegadaParam = dtChegada == null ? null : $"?dtChegada={dtChegada}";

            string chamada = $"/IntegraMAUI/{pedidoId}/{empresaId}/{ocorrenciaId}/{imgCanhoto}"; //{dtChegadaParam}";

            var response = await Constantes.BaseUrl
               .AppendPathSegment(WebUtility.HtmlEncode(chamada))
               .WithOAuthBearerToken(Preferences.Get("token", string.Empty))
               .PutJsonAsync(data);

            return response.ResponseMessage.IsSuccessStatusCode;
        }
    }
}
