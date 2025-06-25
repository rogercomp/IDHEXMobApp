namespace IDHEXMobApp.Models.Response
{
    public class RomaneioResponse
    {   
        public string NumRomaneio { get; set; } = string.Empty;
        public Int32 TotalNotas { get; set; }
        public DateTime? DataPrevisaoSaida { get; set; }
    }
}
