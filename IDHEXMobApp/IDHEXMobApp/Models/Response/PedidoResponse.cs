using System.Text.Json.Serialization;

namespace IDHEXMobApp.Models.Response
{
    public class PedidoResponse
    {
        public Guid Id { get; set; }
        public long PedidoId { get; set; }
        public long EmpresaId { get; set; }
        public long MotoristaId { get; set; }
        public string? NumRomaneio { get; set; }
        public DateTime? DataPrevisaoSaida { get; set; }
        public string? NomeTomador { get; set; }
        public long NumNotaFiscal { get; set; }
        public decimal? VlrNotaFiscal { get; set; }
        public Int32? Volumes { get; set; }
        public string? Nome { get; set; }
        public string? CNPJ { get; set; }
        public string? Logradouro { get; set; }
        public string? Bairro { get; set; }
        public string? Cidade { get; set; }
        public DateTime? DtImgCanhoto { get; set; }
        public string? CodOcorrencia { get; set; }
        public string? ImgCanhoto { get; set; }

        [JsonIgnore]
        public string Baixado
        {
            get
            {
                return ImgCanhoto == null ? "NÃO" : "SIM";
            }
        }


        //public byte[]? ImageBlob { get; init; }

        [JsonIgnore]
        public string Endereco
        {
            get
            {
                return Logradouro + ", " + Bairro + " " + Cidade;
            }
        }

        public PedidoResponse()
        {
            
        }

        public PedidoResponse(long pedidoId, long empresaId, string? numRomaneio, long numNotaFiscal, DateTime? dtImgCanhoto, string? codOcorrencia, string? imgCanhoto)
        {
            PedidoId = pedidoId;
            EmpresaId = empresaId;            
            NumRomaneio = numRomaneio;
            NumNotaFiscal = numNotaFiscal;
            DtImgCanhoto = dtImgCanhoto;
            CodOcorrencia = codOcorrencia;
            ImgCanhoto = imgCanhoto;
        }



        //ImageSource? GetImageStream()
        //{
        //    try
        //    {
        //        if (ImageBlob is null)
        //            return null;

        //        var imageByteArray = ImageBlob;

        //        return ImageSource.FromStream(() => new MemoryStream(imageByteArray));
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}
    }
}
