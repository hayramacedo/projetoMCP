using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PontoServer.Models
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