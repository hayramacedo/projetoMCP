using PontoServer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Http;
using static PontoServer.Controllers.EmpresaController;
using static PontoServer.Repositorio;

namespace PontoServer.Controllers
{
    public class FolhaController : ApiController
    {
        // GET api/values/5
        /// <summary>
        /// Traz a Folha referente aos parâmetros de entrada
        /// </summary>
        [HttpGet]
        [Route("api/Folha/{id?}/{id_funcionario?}/{data_inicio?}/{data_fim?}")]
        public ListaFolhaResponse Get(int? id, int? id_funcionario, string data_inicio, string data_fim)
        {
            Repositorio repositorio = new Repositorio();
            List<Folha> folhas;
            try
            {
                using (var tabela = repositorio.GetFolha(id, id_funcionario, data_inicio, data_fim))
                {
                    if (tabela.Rows.Count > 0)
                    {
                        folhas = new List<Folha>();
                        foreach (DataRow row in tabela.Rows)
                        {
                            Folha folha = new Folha
                            {
                                Id = Convert.ToInt32(row["Id"]),
                                Data_Folha = (DateTime?)row["Data_Folha"],
                                Entrada = row["Entrada"] != DBNull.Value ? (DateTimeOffset?)row["Entrada"] : null,
                                Pausa = row["Pausa"] != DBNull.Value ? (DateTimeOffset?)row["Pausa"] : null,
                                Retorno = row["Retorno"] != DBNull.Value ? (DateTimeOffset?)row["Retorno"] : null,
                                Saida = row["Saida"] != DBNull.Value ? (DateTimeOffset?)row["Saida"] : null,
                                Id_St_Jornada = Int32.Parse(row["Id_St_Jornada"].ToString()),
                                Id_Funcionario = Int32.Parse(row["Id_Funcionario"].ToString()),
                                Dh_Inclusao = (DateTime)row["Dh_Inclusao"],
                                Ult_Dia_Folga = (DateTime)row["Ult_Dia_Folga"],
                                St_Jornada = row["St_Jornada"].ToString()
                            };                           
                            folhas.Add(folha);
                        }
                        return (new ListaFolhaResponse { Folhas = folhas, Mensagem = "Resultado OK", Sucesso = true });
                    }
                    else
                        return (new ListaFolhaResponse { Folhas = null, Mensagem = "Registro inválido", Sucesso = false });
                }

            }
            catch (Exception ex)
            {
                return (new ListaFolhaResponse { Folhas = null, Mensagem = "Ocorreu um erro: " + ex.Message, Sucesso = false });
            }
        }

        // GET api/values/5
        /// <summary>
        /// Atualiza a folha do funcionario
        /// </summary>
        [HttpGet]
        [Route("api/AtualizarFolha/{id_funcionario}")]
        public ApiResponse Get(int id_funcionario)
        {
            Repositorio repositorio = new Repositorio();
            try
            {
                repositorio.AtualizarFolha(id_funcionario);
                return (new ApiResponse { Mensagem = "Resultado OK", Sucesso = true });

            }
            catch (Exception ex)
            {
                return (new ApiResponse { Mensagem = "Ocorreu um erro: " + ex.Message, Sucesso = false });
            }
        }

        // PUT api/values/5
        /// <summary>
        /// Atualiza a folha do funcionario
        /// </summary>
        [HttpPut]
        [Route("api/RegistroPonto/{id_folha}")]
        public ApiResponse RegistroPonto(int id_folha, [FromBody] Folha folha)
        {
            try
            {
                Repositorio repositorio = new Repositorio();
                var camposValores = new Dictionary<string, object>
                {
                    { "@entrada", folha.Entrada },
                    { "@pausa", folha.Pausa },
                    { "@retorno", folha.Retorno },
                    { "@Saida", folha.Saida }
                };

                if (folha.St_Jornada != null)
                {
                    camposValores.Add("@St_Jornada", folha.St_Jornada);
                }
                if (folha.Ult_Dia_Folga != null)
                {
                    camposValores.Add("@Ult_Dia_Folga", folha.Ult_Dia_Folga);
                }

                var resultado = repositorio.UpdateRegistro("FOLHA", camposValores, id_folha);
                return (new ApiResponse { Mensagem = "Resultado OK", Sucesso = true });

            }
            catch (Exception ex)
            {
                return (new ApiResponse { Mensagem = "Ocorreu um erro: " + ex.Message, Sucesso = false });
            }
        }

        // PUT api/values/5
        /// <summary>
        /// Atualizar Folha
        /// </summary>
        [HttpPut]
        [Route("api/Folha/{id_folha}")]
        public ApiResponse Put(int id_folha, [FromBody] Folha folha)
        {
            try
            {
                Repositorio repositorio = new Repositorio();
                var camposValores = new Dictionary<string, object>
                {
                    { "@Entrada", folha.Entrada },
                    { "@Pausa", folha.Pausa },
                    { "@Retorno", folha.Retorno },
                    { "@Saida", folha.Saida },
                    { "@Id_St_Jornada", folha.Id_St_Jornada },
                    { "@Ult_Dia_Folga", folha.Ult_Dia_Folga }
                };

                var resultado = repositorio.UpdateRegistro("FOLHA", camposValores, id_folha);

                return (new ApiResponse { Mensagem = resultado.Mensagem, Sucesso = resultado.Sucesso });
            }
            catch (Exception ex)
            {
                return (new ApiResponse { Mensagem = "Ocorreu um erro: " + ex.Message, Sucesso = false });
            }
        }

        // GET api/values
        /// <summary>
        /// Traz todas Situacoes de Jornada
        /// </summary>
        [HttpGet]
        [Route("api/Situacoes")]
        public List<Situacao_Jornada> Get()
        {
            Repositorio repositorio = new Repositorio();
            List<Situacao_Jornada> situacoes;
            using (var tabela = repositorio.GetDataTable("SITUACAO_JORNADA"))
            {
                situacoes = new List<Situacao_Jornada>();
                foreach (DataRow row in tabela.Rows)
                {
                    Situacao_Jornada situacao = new Situacao_Jornada
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        Descricao = row["Descricao"].ToString(),
                        Dh_Inclusao = (DateTime)row["Dh_Inclusao"]
                    };

                    situacoes.Add(situacao);
                }
            }
            return situacoes;
        }

        // GET api/values
        /// <summary>
        /// Traz Saldo do Banco de Horas referente ao Funcionário
        /// </summary>
        [HttpGet]
        [Route("api/BancoHoras_Saldo/{id_funcionario}")]
        public SaldoBancoH GetSaldo(int id_funcionario = 0)
        {
            Repositorio repositorio = new Repositorio();
            var resultado = repositorio.BancoHorasSaldo(id_funcionario);
              
            return resultado;
        }


        [HttpGet]
        [Route("api/Atestados/{id_solicitacao}")]
        public AtestadoResponse GetAtestado(int id_solicitacao)
        {
            Repositorio repositorio = new Repositorio();
            try
            {
                var tabela = repositorio.GetByParametrosInt("ATESTADO",0,"id_solicitacao", id_solicitacao.ToString());
                Atestado atestado = new Atestado
                {
                    Id = Convert.ToInt32(tabela.Rows[0]["Id"]),
                    Documento = tabela.Rows[0]["Documento"] != DBNull.Value && tabela.Rows[0]["Documento"] != null ? (byte[])tabela.Rows[0]["Documento"] : new byte[] { },
                    Id_Funcionario = Convert.ToInt32(tabela.Rows[0]["Id_Funcionario"]),
                    Id_Folha = Convert.ToInt32(tabela.Rows[0]["Id_Folha"]),
                    Dh_Inclusao = (DateTime)tabela.Rows[0]["Dh_Inclusao"],
                    Nm_Documento = tabela.Rows[0]["Nm_Documento"].ToString(),
                    Id_Solicitacao = Convert.ToInt32(tabela.Rows[0]["Id_Solicitacao"])
                };
                return (new AtestadoResponse { Atestado = atestado, Mensagem = "Resultado OK", Sucesso = true });

            }
            catch (Exception ex)
            {
                return (new AtestadoResponse { Atestado = null, Mensagem = "Ocorreu um erro: " + ex.Message, Sucesso = false });
            }
        }

        // POST api/values
        /// <summary>
        /// Inserir Atestado
        /// </summary>
        [HttpPost]
        [Route("api/Atestados")]
        public ApiResponse Post([FromBody] Atestado atestado)
        {
            try
            {
                Repositorio repositorio = new Repositorio();
                var camposValores = new Dictionary<string, object>
                {
                    { "@documento", atestado.Documento },
                    { "@id_funcionario", atestado.Id_Funcionario },
                    { "@id_folha", atestado.Id_Folha },
                    { "@nm_documento", atestado.Nm_Documento },
                    { "@id_solicitacao", atestado.Id_Solicitacao }
                };

                var resultado = repositorio.InsertRegistro("ATESTADO", camposValores);

                return (new ApiResponse { Mensagem = resultado.Mensagem, Sucesso = resultado.Sucesso });
            }
            catch (Exception ex)
            {
                return (new ApiResponse { Mensagem = "Ocorreu um erro: " + ex.Message, Sucesso = false });
            }
        }

        public class ListaFolhaResponse
        {
            public List<Folha> Folhas { get; set; }
            public string Mensagem { get; set; }
            public bool Sucesso { get; set; }
        }

        public class AtestadoResponse
        {
            public Atestado Atestado { get; set; }
            public string Mensagem { get; set; }
            public bool Sucesso { get; set; }
        }

        public class ApiResponse
        {
            public string Mensagem { get; set; }
            public bool Sucesso { get; set; }
        }

    }
}