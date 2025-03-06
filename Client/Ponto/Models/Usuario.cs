using System.ComponentModel.DataAnnotations;

namespace Ponto.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime Dh_Inclusao { get; set; }
        public string Nickname { get; set; }
        public char Ativo { get; set; }
    }
}
