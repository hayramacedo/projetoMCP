using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PontoServer.Models
{
    public class Cargo
    {
        [Key]
        public int Id { get; set; }
        public string Descricao { get; set; }
        public int Hora_Jornada { get; set; }
        public DateTime Dh_Inclusao { get; set; }
        public int Id_Departamento { get; set; }
        public char Ativo { get; set; }
    }
}