using PontoServer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Http;
using static PontoServer.Controllers.FuncionarioController;

namespace PontoServer.Controllers
{
    public class EscalaController : ApiController
    {
        // GET api/values
        /// <summary>
        /// Traz todas as Escalas
        /// </summary>
        [HttpGet]
        [Route("api/Escalas")]
        public List<Escala> Get()
        {
            Repositorio repositorio = new Repositorio();
            List<Escala> escalas;
            using (var tabela = repositorio.GetDataTable("ESCALA"))
            {
                escalas = new List<Escala>();
                foreach (DataRow row in tabela.Rows)
                {
                    Escala escala = new Escala
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        Descricao = row["Descricao"].ToString(),
                        Trabalho = Int32.Parse(row["Trabalho"].ToString()),
                        Folga = Int32.Parse(row["Folga"].ToString()),
                        Carga = row["Carga"].ToString(),
                        Dh_Inclusao = (DateTime)row["Dh_Inclusao"]
                    };

                    escalas.Add(escala);
                }
            }
            return escalas;
        }

        // GET api/values/5
        /// <summary>
        /// Traz a Escala referente ao ID ou Descricao
        /// </summary>
        [HttpGet]
        [Route("api/Escalas/{id?}/{descricao?}")]
        public ListaEscalaResponse Get(int id = 0, string descricao = "")
        {
            Repositorio repositorio = new Repositorio();
            List<Escala> escalas;
            try
            {
                using (var tabela = repositorio.GetByParametros("ESCALA", id, "descricao", descricao))
                {
                    if (tabela.Rows.Count > 0)
                    {
                        escalas = new List<Escala>();
                        foreach (DataRow row in tabela.Rows)
                        {
                            Escala escala = new Escala
                            {
                                Id = Convert.ToInt32(row["Id"]),
                                Descricao = row["Descricao"].ToString(),
                                Trabalho = Int32.Parse(row["Trabalho"].ToString()),
                                Folga = Int32.Parse(row["Folga"].ToString()),
                                Carga = row["Carga"].ToString(),
                                Dh_Inclusao = (DateTime)row["Dh_Inclusao"]
                            };

                            escalas.Add(escala);
                        }
                        return (new ListaEscalaResponse { Escalas = escalas, Mensagem = "Resultado OK", Sucesso = true });
                    }
                    else
                        return (new ListaEscalaResponse { Escalas = null, Mensagem = "Funcionário inválido", Sucesso = false });
                }

            }
            catch (Exception ex)
            {
                return (new ListaEscalaResponse { Escalas = null, Mensagem = "Ocorreu um erro: " + ex.Message, Sucesso = false });
            }
        }

        public class EscalaResponse
        {
            public Escala Escala { get; set; }
            public string Mensagem { get; set; }
            public bool Sucesso { get; set; }
        }

        public class ListaEscalaResponse
        {
            public List<Escala> Escalas { get; set; }
            public string Mensagem { get; set; }
            public bool Sucesso { get; set; }
        }
    }
}