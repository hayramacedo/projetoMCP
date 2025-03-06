using PontoServer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace PontoServer.Controllers
{
    public class DepartamentoController : ApiController
    {
        // GET api/values
        /// <summary>
        /// Traz todos os Departamentos
        /// </summary>
        [HttpGet]
        [Route("api/Departamentos")]
        public List<Departamento> Get()
        {
            Repositorio repositorio = new Repositorio();
            List<Departamento> departamentos;
            using (var tabela = repositorio.GetDataTable("DEPARTAMENTO"))
            {
                departamentos = new List<Departamento>();
                foreach (DataRow row in tabela.Rows)
                {
                    Departamento departamento = new Departamento
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        Descricao = row["Descricao"].ToString(),
                        Dh_Inclusao = (DateTime)row["Dh_Inclusao"],
                        Ativo = (char)row["Ativo"]
                    };

                    departamentos.Add(departamento);
                }
            }
            return departamentos;
        }

        //// GET api/values/5
        ///// <summary>
        ///// Traz o Departamento referente ao ID
        ///// </summary>
        //[HttpGet]
        //[Route("api/Departamentos/{id}")]
        //public DepartamentoResponse Get(int id = 0)
        //{
        //    Repositorio repositorio = new Repositorio();
        //    Departamento departamento;
        //    try
        //    {
        //        using (var tabela = repositorio.GetByID("DEPARTAMENTO", id))
        //        {
        //            if (tabela.Rows.Count > 0)
        //            {
        //                departamento = new Departamento
        //                {
        //                    Id = Convert.ToInt32(tabela.Rows[0]["Id"]),
        //                    Descricao = tabela.Rows[0]["Descricao"].ToString(),
        //                    Dh_Inclusao = (DateTime)tabela.Rows[0]["Dh_Inclusao"],
        //                    Ativo = (char)tabela.Rows[0]["Ativo"]
        //                };
        //                return (new DepartamentoResponse { Departamento = departamento, Mensagem = "Resultado OK", Sucesso = true });
        //            }
        //            else
        //                return (new DepartamentoResponse { Departamento = null, Mensagem = "Departamento inválido", Sucesso = false });
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        return (new DepartamentoResponse { Departamento = null, Mensagem = "Ocorreu um erro: " + ex.Message, Sucesso = false });
        //    }
        //}

        // GET api/values/5
        /// <summary>
        /// Traz o Departamento referente ao ID ou Descricao
        /// </summary>
        [HttpGet]
        [Route("api/Departamentos/{id?}/{descricao?}")]
        public ListaDepartamentoResponse Get(int id = 0, string descricao = "")
        {
            Repositorio repositorio = new Repositorio();
            List<Departamento> departamentos;
            try
            {
                using (var tabela = repositorio.GetByParametros("DEPARTAMENTO", id, "Descricao", descricao))
                {
                    if (tabela.Rows.Count > 0)
                    {
                        departamentos = new List<Departamento>();
                        foreach (DataRow row in tabela.Rows)
                        {
                            Departamento departamento = new Departamento
                            {
                                Id = Convert.ToInt32(row["Id"]),
                                Descricao = row["Descricao"].ToString(),
                                Dh_Inclusao = (DateTime)row["Dh_Inclusao"],
                                Ativo = (char)row["Ativo"]
                            };

                            departamentos.Add(departamento);
                        }
                        return (new ListaDepartamentoResponse { Departamentos = departamentos, Mensagem = "Resultado OK", Sucesso = true });
                    }
                    else
                        return (new ListaDepartamentoResponse { Departamentos = null, Mensagem = "Departamento inválido", Sucesso = false });
                }

            }
            catch (Exception ex)
            {
                return (new ListaDepartamentoResponse { Departamentos = null, Mensagem = "Ocorreu um erro: " + ex.Message, Sucesso = false });
            }
        }

        // POST api/values
        /// <summary>
        /// Inserir Departamento
        /// </summary>
        [HttpPost]
        [Route("api/Departamentos")]
        public DepartamentoResponse Post([FromBody] Departamento departamento)
        {
            try
            {
                Repositorio repositorio = new Repositorio();
                var camposValores = new Dictionary<string, object>
                {
                    { "@descricao", departamento.Descricao }
                };

                var resultado = repositorio.InsertRegistro("DEPARTAMENTO", camposValores);

                return (new DepartamentoResponse { Departamento = departamento, Mensagem = resultado.Mensagem, Sucesso = resultado.Sucesso });
            }
            catch (Exception ex)
            {
                return (new DepartamentoResponse { Departamento = null, Mensagem = "Ocorreu um erro: " + ex.Message, Sucesso = false });
            }
        }

        // PUT api/values/5
        /// <summary>
        /// Atualizar Departamento
        /// </summary>
        [HttpPut]
        [Route("api/Departamentos")]
        public DepartamentoResponse Put(int id, [FromBody] Departamento departamento)
        {
            try
            {
                Repositorio repositorio = new Repositorio();
                var camposValores = new Dictionary<string, object>
                {
                    { "@descricao", departamento.Descricao },
                    { "@ativo", departamento.Ativo }
                };

                var resultado = repositorio.UpdateRegistro("DEPARTAMENTO", camposValores, id);

                return (new DepartamentoResponse { Departamento = departamento, Mensagem = resultado.Mensagem, Sucesso = resultado.Sucesso });
            }
            catch (Exception ex)
            {
                return (new DepartamentoResponse { Departamento = null, Mensagem = "Ocorreu um erro: " + ex.Message, Sucesso = false });
            }
        }

        // DELETE api/values/5
        /// <summary>
        /// Deletar Departamento
        /// </summary>
        [HttpDelete]
        [Route("api/Departamentos/{id}")]
        public DepartamentoResponse Delete(int id)
        {
            try
            {
                Repositorio repositorio = new Repositorio();
                var resultado = repositorio.DeleteRegistro("DEPARTAMENTO", id);

                return (new DepartamentoResponse { Departamento = null, Mensagem = resultado.Mensagem, Sucesso = resultado.Sucesso });
            }
            catch (Exception ex)
            {
                return (new DepartamentoResponse { Departamento = null, Mensagem = "Ocorreu um erro: " + ex.Message, Sucesso = false });
            }

        }

        public class DepartamentoResponse
        {
            public Departamento Departamento { get; set; }
            public string Mensagem { get; set; }
            public bool Sucesso { get; set; }
        }

        public class ListaDepartamentoResponse
        {
            public List<Departamento> Departamentos { get; set; }
            public string Mensagem { get; set; }
            public bool Sucesso { get; set; }
        }
    }
}