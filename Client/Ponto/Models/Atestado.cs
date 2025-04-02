using System.ComponentModel.DataAnnotations;

namespace Ponto.Models
{
    public class Atestado
    {
        [Key]
        public int Id { get; set; }
        public byte[] Documento { get; set; }
        public int Id_Funcionario { get; set; }
        public int Id_Folha { get; set; }
        public DateTime Dh_Inclusao { get; set; }
        public string Nm_Documento { get; set; }
        public int Id_Solicitacao { get; set; }
    }
}
