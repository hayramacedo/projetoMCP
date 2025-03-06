using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PontoServer.Models
{
    public class Folha
    {
        [Key]
        public int Id { get; set; }
        public DateTime? Data_Folha { get; set; }
        public DateTimeOffset? Entrada { get; set; }
        public DateTimeOffset? Pausa { get; set; }
        public DateTimeOffset? Retorno { get; set; }
        public DateTimeOffset? Saida { get; set; }
        public int Id_St_Jornada { get; set; }
        public int Id_Funcionario { get; set; }
        public DateTime Dh_Inclusao { get; set; }
        public DateTime? Ult_Dia_Folga { get; set; }
        public string St_Jornada { get; set; }

    }
}