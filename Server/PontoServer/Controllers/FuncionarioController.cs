using PontoServer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;

namespace PontoServer.Controllers
{
    public class FuncionarioController : ApiController
    {
        // GET api/values
        /// <summary>
        /// Traz todos os Funcionarios
        /// </summary>
        [HttpGet]
        [Route("api/Funcionarios")]
        public List<Funcionario> Get()
        {
            Repositorio repositorio = new Repositorio();
            List<Funcionario> funcionarios;
            using (var tabela = repositorio.GetDataTable("FUNCIONARIO"))
            {
                funcionarios = new List<Funcionario>();
                foreach (DataRow row in tabela.Rows)
                {
                    Funcionario funcionario = new Funcionario
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        Nome_Completo = row["Nome_Completo"].ToString(),
                        Cpf = row["Cpf"].ToString(),
                        Email = row["Email"].ToString(),
                        Dt_Nasc = (DateTime)row["Dt_Nasc"],
                        Id_Usuario = Int32.Parse(row["Id_Usuario"].ToString()),
                        Ativo = (char)row["Ativo"],
                        Id_Empresa = Int32.Parse(row["Id_Empresa"].ToString()),
                        Id_Cargo = Int32.Parse(row["Id_Cargo"].ToString()),
                        Matricula = row["Matricula"].ToString(),
                        Dh_Inclusao = (DateTime)row["Dh_Inclusao"],
                        Dt_Admissao = (DateTime)row["Dt_Admissao"],
                        Id_Escala = Int32.Parse(row["Id_Escala"].ToString()),
                        Dt_Inic_BancoH = (DateTime)row["Dt_Inic_BancoH"]
                    };

                    funcionarios.Add(funcionario);
                }
            }
            return funcionarios;
        }

        // GET api/values/5
        /// <summary>
        /// Traz o Funcionario referente ao ID ou Nome
        /// </summary>
        [HttpGet]
        [Route("api/Funcionarios/{id?}/{Nome?}")]
        public ListaFuncionarioResponse Get(int id = 0, string nome = "")
        {
            Repositorio repositorio = new Repositorio();
            List<Funcionario> funcionarios;
            try
            {
                using (var tabela = repositorio.GetByParametros("FUNCIONARIO", id, "nome_completo", nome))
                {
                    if (tabela.Rows.Count > 0)
                    {
                        funcionarios = new List<Funcionario>();
                        foreach (DataRow row in tabela.Rows)
                        {
                            Funcionario funcionario = new Funcionario
                            {
                                Id = Convert.ToInt32(row["Id"]),
                                Nome_Completo = row["Nome_Completo"].ToString(),
                                Cpf = row["Cpf"].ToString(),
                                Email = row["Email"].ToString(),
                                Dt_Nasc = (DateTime)row["Dt_Nasc"],
                                Id_Usuario = Int32.Parse(row["Id_Usuario"].ToString()),
                                Ativo = (char)row["Ativo"],
                                Id_Empresa = Int32.Parse(row["Id_Empresa"].ToString()),
                                Id_Cargo = Int32.Parse(row["Id_Cargo"].ToString()),
                                Matricula = row["Matricula"].ToString(),
                                Dh_Inclusao = (DateTime)row["Dh_Inclusao"],
                                Dt_Admissao = (DateTime)row["Dt_Admissao"],
                                Id_Escala = Int32.Parse(row["Id_Escala"].ToString()),
                                Dt_Inic_BancoH = (DateTime)row["Dt_Inic_BancoH"]
                            };

                            funcionarios.Add(funcionario);
                        }
                        return (new ListaFuncionarioResponse { Funcionarios = funcionarios, Mensagem = "Resultado OK", Sucesso = true });
                    }
                    else
                        return (new ListaFuncionarioResponse { Funcionarios = null, Mensagem = "Funcionário inválido", Sucesso = false });
                }

            }
            catch (Exception ex)
            {
                return (new ListaFuncionarioResponse { Funcionarios = null, Mensagem = "Ocorreu um erro: " + ex.Message, Sucesso = false });
            }
        }

        // GET api/values/5
        /// <summary>
        /// Traz o Funcionario referente ao id do Usuario
        /// </summary>
        [HttpGet]
        [Route("api/FuncionariosxUsuarios/{id_usu?}")]
        public ListaFuncionarioResponse GetFuncionariobyUser(int id_usu = 0)
        {
            Repositorio repositorio = new Repositorio();
            List<Funcionario> funcionarios;
            try
            {
                using (var tabela = repositorio.GetFuncByUsuario(id_usu))
                {
                    if (tabela.Rows.Count > 0)
                    {
                        funcionarios = new List<Funcionario>();
                        foreach (DataRow row in tabela.Rows)
                        {
                            Funcionario funcionario = new Funcionario
                            {
                                Id = Convert.ToInt32(row["Id"]),
                                Nome_Completo = row["Nome_Completo"].ToString(),
                                Cpf = row["Cpf"].ToString(),
                                Email = row["Email"].ToString(),
                                Dt_Nasc = (DateTime)row["Dt_Nasc"],
                                Id_Usuario = Int32.Parse(row["Id_Usuario"].ToString()),
                                Ativo = (char)row["Ativo"],
                                Id_Empresa = Int32.Parse(row["Id_Empresa"].ToString()),
                                Id_Cargo = Int32.Parse(row["Id_Cargo"].ToString()),
                                Matricula = row["Matricula"].ToString(),
                                Dh_Inclusao = (DateTime)row["Dh_Inclusao"],
                                Dt_Admissao = (DateTime)row["Dt_Admissao"],
                                Id_Escala = Int32.Parse(row["Id_Escala"].ToString()),
                                Dt_Inic_BancoH = (DateTime)row["Dt_Inic_BancoH"]
                            };

                            funcionarios.Add(funcionario);
                        }
                        return (new ListaFuncionarioResponse { Funcionarios = funcionarios, Mensagem = "Resultado OK", Sucesso = true });
                    }
                    else
                        return (new ListaFuncionarioResponse { Funcionarios = null, Mensagem = "Funcionário inválido", Sucesso = false });
                }

            }
            catch (Exception ex)
            {
                return (new ListaFuncionarioResponse { Funcionarios = null, Mensagem = "Ocorreu um erro: " + ex.Message, Sucesso = false });
            }
        }

        // PUT api/values/5
        /// <summary>
        /// Atualizar Funcionario
        /// </summary>
        [HttpPut]
        [Route("api/Funcionarios")]
        public FuncionarioResponse Put(int id, [FromBody] Funcionario funcionario)
        {
            try
            {
                Repositorio repositorio = new Repositorio();
                var camposValores = new Dictionary<string, object>
                {
                    { "@nome_completo", funcionario.Nome_Completo },
                    { "@cpf", funcionario.Cpf },
                    { "@email", funcionario.Email },
                    { "@dt_nasc", funcionario.Dt_Nasc },
                    { "@ativo", funcionario.Ativo },
                    { "@id_empresa", funcionario.Id_Empresa },
                    { "@id_cargo", funcionario.Id_Cargo },
                    { "@matricula", funcionario.Matricula },
                    { "@dt_admissao", funcionario.Dt_Admissao },
                    { "@id_escala", funcionario.Id_Escala },
                    { "@dt_inic_bancoh", funcionario.Dt_Inic_BancoH }
                };

                var resultado = repositorio.UpdateRegistro("FUNCIONARIO", camposValores, id);

                return (new FuncionarioResponse { Funcionario = funcionario, Mensagem = resultado.Mensagem, Sucesso = resultado.Sucesso });
            }
            catch (Exception ex)
            {
                return (new FuncionarioResponse { Funcionario = null, Mensagem = "Ocorreu um erro: " + ex.Message, Sucesso = false });
            }
        }

        // DELETE api/values/5
        /// <summary>
        /// Deletar Funcionario
        /// </summary>
        [HttpDelete]
        [Route("api/Funcionarios/{id}")]
        public FuncionarioResponse Delete(int id)
        {
            try
            {
                Repositorio repositorio = new Repositorio();
                var resultado = repositorio.DeleteRegistro("CARGO", id);

                return (new FuncionarioResponse { Funcionario = null, Mensagem = resultado.Mensagem, Sucesso = resultado.Sucesso });
            }
            catch (Exception ex)
            {
                return (new FuncionarioResponse { Funcionario = null, Mensagem = "Ocorreu um erro: " + ex.Message, Sucesso = false });
            }

        }

        public class FuncionarioResponse
        {
            public Funcionario Funcionario { get; set; }
            public string Mensagem { get; set; }
            public bool Sucesso { get; set; }
        }

        public class ListaFuncionarioResponse
        {
            public List<Funcionario> Funcionarios { get; set; }
            public string Mensagem { get; set; }
            public bool Sucesso { get; set; }
        }

    }
}