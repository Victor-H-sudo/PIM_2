using System.ComponentModel.DataAnnotations;

namespace EcoPoint.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        public string Nome { get; set; } = string.Empty;
        [Required]
        public string CPF { get; set; } = string.Empty;
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [DataType(DataType.Password)]
        public string Senha { get; set; } = string.Empty;
        public string TipoUsuario { get; set; } = "Usuario";        
        public int Pontos { get; set; } = 0;

        public DateTime DataCadastro { get; set; } = DateTime.Now;
    }
}