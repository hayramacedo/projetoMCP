using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PontoServer.Models
{
    public class Atestado
    {
        [Key]
        public int Id { get; set; }
        public byte[] Documento { get; set; }
        public int Id_Funcionario { get; set; }
        public int Id_Folha { get; set; }
        public DateTime Dh_Inclusao { get; set; }
    }
}