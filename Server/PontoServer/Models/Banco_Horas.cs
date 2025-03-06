using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PontoServer.Models
{
    public class Banco_Horas
    {
        [Key]
        public int Id { get; set; }
        public int Saldo { get; set; }
        public int Id_Funcionario { get; set; }
        public DateTime Dh_Inclusao { get; set; }
    }
}