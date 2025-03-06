using System.ComponentModel.DataAnnotations;

namespace Ponto.Models
{
    public class Escala
    {
        [Key]
        public int Id { get; set; }
        public string Descricao { get; set; }
        public int Trabalho { get; set; }
        public int Folga { get; set; }
        public string Carga { get; set; }
        public DateTime Dh_Inclusao { get; set; }
    }
}
