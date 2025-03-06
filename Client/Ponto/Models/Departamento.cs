using System.ComponentModel.DataAnnotations;

namespace Ponto.Models
{
    public class Departamento
    {
        [Key]
        public int Id { get; set; }
        public string Descricao { get; set; }
        public DateTime Dh_Inclusao { get; set; }
        public char Ativo { get; set; }
    }
}
