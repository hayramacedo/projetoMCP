using System.ComponentModel.DataAnnotations;

namespace Ponto.Models
{
    public class Cargo
    {
        [Key]
        public int Id { get; set; }
        public string Descricao { get; set; }
        public int Hora_Jornada { get; set; }
        public DateTime Dh_Inclusao { get; set; }
        public int Id_Departamento { get; set; }
        public string Departamento { get; set; }
        public char Ativo { get; set; }
    }
}
