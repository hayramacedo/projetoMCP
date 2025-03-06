using System.ComponentModel.DataAnnotations;

namespace Ponto.Models
{
    public class SolicitacaoAjuste
    {
        [Key]
        public int Id { get; set; }
        public int Id_Funcionario { get; set; }
        public int Id_Folha { get; set; }
        public DateTimeOffset? Entrada { get; set; }
        public DateTimeOffset? Pausa { get; set; }
        public DateTimeOffset? Retorno { get; set; }
        public DateTimeOffset? Saida { get; set; }
        public string Ds_Causa { get; set; }
        public string St_Ajuste { get; set; }
        public DateTime Dh_Inclusao { get; set; }
        public string Replica_Adm { get; set; }
        public DateTime Data_Folha { get; set; }
    }
}
