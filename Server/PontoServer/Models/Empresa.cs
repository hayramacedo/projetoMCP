using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PontoServer.Models
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