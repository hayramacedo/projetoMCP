using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PontoServer.Models
{
    public class Situacao_Jornada
    {
        [Key]
        public int Id { get; set; }
        public string Descricao { get; set; }
        public DateTime Dh_Inclusao { get; set; }
    }
}