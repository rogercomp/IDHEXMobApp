namespace IDHEXMobApp.Models.Request;

public class PedidoRequest
{
    public long PedidoId { get; set; }
    public long EmpresaId { get; set; }
    public string? NumRomaneio { get; set; }            
    public decimal NumNotaFiscal { get; set; }
    public string? ImgCanhoto { get; set; }
}
