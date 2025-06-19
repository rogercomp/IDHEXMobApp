using System.Text.Json.Serialization;

namespace IDHEXMobApp.Models.Response
{
    public class PedidoResponse
    {
        public long PedidoId { get; set; }
        public long EmpresaId { get; set; }
        public long MotoristaId { get; set; }
        public string? NumRomaneio { get; set; }
        public DateTime? DataPrevisaoSaida { get; set; }
        public string? NomeTomador { get; set; }
        public decimal NumNotaFiscal { get; set; }
        public decimal? VlrNotaFiscal { get; set; }
        public Int32? Volumes { get; set; }
        public string? Nome { get; set; }
        public string? CNPJ { get; set; }
        public string? Logradouro { get; set; }
        public string? Bairro { get; set; }
        public string? Cidade { get; set; }
        public string? ImgCanhoto { get; set; }
        public DateTime? DtImgCanhoto { get; set; }
        public string? CodOcorrencia { get; set; }

        [JsonIgnore]
        public string Endereco
        { get
            {
                return Logradouro + ", " + Bairro + " " + Cidade;
            }
        }
    }
}
