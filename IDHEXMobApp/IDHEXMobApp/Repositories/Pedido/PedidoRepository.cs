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
            
            Int32 motoristaId = 274;
            Int32 transp = Preferences.Get("transp", 7);
            string unidade = Preferences.Get("unidade", "1");


            return await Constantes.BaseUrl
                .AppendPathSegment($"/IntegraMAUI/{motoristaId}/{transp}/{unidade}")
                .WithOAuthBearerToken(Preferences.Get("token", string.Empty))
                .GetJsonAsync<IEnumerable<PedidoResponse>>();
        }
    }
}
