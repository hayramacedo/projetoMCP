using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PontoServer.Models
{
    public class Funcionario
    {
        [Key]
        public int Id { get; set; }
        public string Nome_Completo { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public DateTime? Dt_Nasc { get; set; }
        public int Id_Usuario { get; set; }
        public char Ativo { get; set; }
        public int Id_Empresa { get; set; }
        public int Id_Cargo { get; set; }
        public string Matricula { get; set; }
        public DateTime Dh_Inclusao { get; set; }
        public DateTime? Dt_Admissao { get; set; }
        public int Id_Escala { get; set; }
        public DateTime? Dt_Inic_BancoH { get; set; }
        public string Pis { get; set; }

    }
}