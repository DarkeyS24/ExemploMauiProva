namespace MobileApp.Models
{
    public class DiaCalendario
    {
        public DateTime Data { get; set; }
        public bool EhDoMesAtual { get; set; }
        public bool TemAtendimento { get; set; }
        public TipoData TipoData { get; set; }
        public string Numero => Data.Day.ToString();
        public Color CorFundo { get; set; } = Colors.Transparent;
        public Color CorTexto { get; set; } = Colors.Black;
    }

    public enum TipoData
    {
        Passada,
        Atual,
        Futura
    }
}
