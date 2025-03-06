using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PontoServer.Models
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