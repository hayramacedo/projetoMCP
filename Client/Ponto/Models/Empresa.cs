using System.ComponentModel.DataAnnotations;

namespace Ponto.Models
{
    public class Empresa
    {
        [Key]
        public int Id { get; set; }
        public string Razao_Social { get; set; }
        public string Fantasia { get; set; }
        public string Cnpj { get; set; }
        public DateTime Dh_Inclusao { get; set; }
    }
}
