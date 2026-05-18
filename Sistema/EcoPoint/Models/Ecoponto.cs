using System.ComponentModel.DataAnnotations;

namespace EcoPoint.Models
{
    public class Ecoponto
    {
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; } = string.Empty;

        [Required]
        public string CNPJ { get; set; } = string.Empty;

        public string Endereco { get; set; } = string.Empty;

        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [DataType(DataType.Password)]
        public string Senha { get; set; } = string.Empty;

        public string LinkMaps { get; set; } = string.Empty;
    }
}