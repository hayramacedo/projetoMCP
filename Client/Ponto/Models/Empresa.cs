using System.ComponentModel.DataAnnotations;

namespace Ponto.Models
{
    public class Empresa
    {
        [Key]
        public int Id { get; set; }
        public string Razao_Social { get; set; }
        public string Fantasia { get; set; }
        public string Cnpj { get; set; }
        public DateTime Dh_Inclusao { get; set; }
        public string Endereco { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Municipio { get; set; }
        public string Uf { get; set; }
        public string Cep { get; set; }
        public string Telefone { get; set; }
    }
}
