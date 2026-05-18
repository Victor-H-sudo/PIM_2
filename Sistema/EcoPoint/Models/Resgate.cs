namespace EcoPoint.Models
{
    public class Resgate
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        public int RecompensaId { get; set; }
        public Recompensa? Recompensa { get; set; }

        public DateTime DataResgate { get; set; } = DateTime.Now;
    }
}