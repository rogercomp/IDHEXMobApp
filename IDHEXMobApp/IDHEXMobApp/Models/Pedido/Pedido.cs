namespace IDHEXMobApp.Models.Pedido
{
    public class Pedido
    {
        public Int32? MotoristaId { get; set; }
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
    }
}
