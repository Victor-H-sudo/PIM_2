using System.ComponentModel.DataAnnotations;

namespace EcoPoint.Models
{
public class Recompensa
{
public int Id { get; set; }


    [Required]
    public string Nome { get; set; } = string.Empty;

    public string Descricao { get; set; } = string.Empty;

    public int PontosNecessarios { get; set; }
}


}
