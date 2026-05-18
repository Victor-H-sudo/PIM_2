namespace EcoPoint.Models
{
    public class Reciclagem
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }

        public Usuario? Usuario { get; set; }

        public int EcopontoId { get; set; }

        public Ecoponto? Ecoponto { get; set; }

        public int TipoMaterialId { get; set; }

        public TipoMaterial? TipoMaterial { get; set; }

        public decimal Peso { get; set; }

        public int PontosGerados { get; set; }

        public DateTime Data { get; set; } = DateTime.Now;

        public string? FotoMaterial { get; set; } = string.Empty;

        public string? FotoPeso { get; set; } = string.Empty;
    }
}
