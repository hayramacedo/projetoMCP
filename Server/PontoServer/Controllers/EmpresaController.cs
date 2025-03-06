using PontoServer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace PontoServer.Controllers
{
    public class EmpresaController : ApiController
    {
        // GET api/values
        /// <summary>
        /// Traz todas as Empresa
        /// </summary>
        [HttpGet]
        [Route("api/Empresas")]
        public List<Empresa> Get()
        {
            Repositorio repositorio = new Repositorio();
            List<Empresa> empresas;
            using (var tabela = repositorio.GetDataTable("EMPRESA"))
            {
                empresas = new List<Empresa>();
                foreach (DataRow row in tabela.Rows)
                {
                    Empresa empresa = new Empresa
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        Razao_Social = row["Razao_Social"].ToString(),
                        Fantasia = row["Fantasia"].ToString(),
                        Cnpj = row["Cnpj"].ToString(),
                        Dh_Inclusao = (DateTime)row["Dh_Inclusao"]
                    };

                    empresas.Add(empresa);
                }
            }
            return empresas;
        }

        // GET api/values/5
        /// <summary>
        /// Traz o Empresa referente ao ID ou Razao Social
        /// </summary>
        [HttpGet]
        [Route("api/Empresas/{id?}/{razao?}")]
        public ListaEmpresaResponse Get(int id = 0, string razao = "")
        {
            Repositorio repositorio = new Repositorio();
            List<Empresa> empresas;
            try
            {
                using (var tabela = repositorio.GetByParametros("EMPRESA", id, "Razao_Social", razao))
                {
                    if (tabela.Rows.Count > 0)
                    {
                        empresas = new List<Empresa>();
                        foreach (DataRow row in tabela.Rows)
                        {
                            Empresa empresa = new Empresa
                            {
                                Id = Convert.ToInt32(row["Id"]),
                                Razao_Social = row["Razao_Social"].ToString(),
                                Fantasia = row["Fantasia"].ToString(),
                                Cnpj = row["Cnpj"].ToString(),
                                Dh_Inclusao = (DateTime)row["Dh_Inclusao"]
                            };

                            empresas.Add(empresa);
                        }
                        return (new ListaEmpresaResponse { Empresas = empresas, Mensagem = "Resultado OK", Sucesso = true });
                    }
                    else
                        return (new ListaEmpresaResponse { Empresas = null, Mensagem = "Empresa inválido", Sucesso = false });
                }

            }
            catch (Exception ex)
            {
                return (new ListaEmpresaResponse { Empresas = null, Mensagem = "Ocorreu um erro: " + ex.Message, Sucesso = false });
            }
        }

        // POST api/values
        /// <summary>
        /// Inserir Empresa
        /// </summary>
        [HttpPost]
        [Route("api/Empresas")]
        public EmpresaResponse Post([FromBody] Empresa empresa)
        {
            try
            {
                Repositorio repositorio = new Repositorio();
                var camposValores = new Dictionary<string, object>
                {
                    { "@razao_social", empresa.Razao_Social },
                    { "@fantasia", empresa.Fantasia },
                    { "@cnpj", empresa.Cnpj }
                };

                var resultado = repositorio.InsertRegistro("EMPRESA", camposValores);

                return (new EmpresaResponse { Empresa = empresa, Mensagem = resultado.Mensagem, Sucesso = resultado.Sucesso });
            }
            catch (Exception ex)
            {
                return (new EmpresaResponse { Empresa = null, Mensagem = "Ocorreu um erro: " + ex.Message, Sucesso = false });
            }
        }

        // PUT api/values/5
        /// <summary>
        /// Atualizar Empresa
        /// </summary>
        [HttpPut]
        [Route("api/Empresas")]
        public EmpresaResponse Put(int id, [FromBody] Empresa empresa)
        {
            try
            {
                Repositorio repositorio = new Repositorio();
                var camposValores = new Dictionary<string, object>
                {
                    { "@razao_social", empresa.Razao_Social },
                    { "@fantasia", empresa.Fantasia },
                    { "@cnpj", empresa.Cnpj }
                };

                var resultado = repositorio.UpdateRegistro("EMPRESA", camposValores, id);

                return (new EmpresaResponse { Empresa = empresa, Mensagem = resultado.Mensagem, Sucesso = resultado.Sucesso });
            }
            catch (Exception ex)
            {
                return (new EmpresaResponse { Empresa = null, Mensagem = "Ocorreu um erro: " + ex.Message, Sucesso = false });
            }
        }

        // DELETE api/values/5
        /// <summary>
        /// Deletar Empresa
        /// </summary>
        [HttpDelete]
        [Route("api/Empresas/{id}")]
        public EmpresaResponse Delete(int id)
        {
            try
            {
                Repositorio repositorio = new Repositorio();
                var resultado = repositorio.DeleteRegistro("EMPRESA", id);

                return (new EmpresaResponse { Empresa = null, Mensagem = resultado.Mensagem, Sucesso = resultado.Sucesso });
            }
            catch (Exception ex)
            {
                return (new EmpresaResponse { Empresa = null, Mensagem = "Ocorreu um erro: " + ex.Message, Sucesso = false });
            }

        }

        public class EmpresaResponse
        {
            public Empresa Empresa { get; set; }
            public string Mensagem { get; set; }
            public bool Sucesso { get; set; }
        }

        public class ListaEmpresaResponse
        {
            public List<Empresa> Empresas { get; set; }
            public string Mensagem { get; set; }
            public bool Sucesso { get; set; }
        }
    }
}