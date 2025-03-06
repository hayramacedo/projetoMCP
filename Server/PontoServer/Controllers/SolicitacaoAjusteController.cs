using PontoServer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Http;
using static PontoServer.Controllers.DepartamentoController;
using static PontoServer.Controllers.FolhaController;

namespace PontoServer.Controllers
{
    public class SolicitacaoAjusteController : ApiController
    {
        // GET api/values/5
        /// <summary>
        /// Traz o Ajuste referente aos parâmetros de entrada
        /// </summary>
        [HttpGet]
        [Route("api/SolicitacaoAjuste/{id?}/{id_funcionario?}/{id_folha?}")]
        public ListaSolicitacaoAjusteResponse Get(int? id, int? id_funcionario, int? id_folha)
        {
            Repositorio repositorio = new Repositorio();
            List<SolicitcaoAjuste> folhas;
            try
            {
                using (var tabela = repositorio.GetFolhaAjuste(id, id_funcionario, id_folha))
                {
                    if (tabela.Rows.Count > 0)
                    {
                        folhas = new List<SolicitcaoAjuste>();
                        foreach (DataRow row in tabela.Rows)
                        {
                            SolicitcaoAjuste folha = new SolicitcaoAjuste
                            {
                                Id = Convert.ToInt32(row["Id"]),
                                Id_Funcionario = Int32.Parse(row["Id_Funcionario"].ToString()),
                                Id_Folha = Int32.Parse(row["Id_Folha"].ToString()),
                                Entrada = row["Entrada"] != DBNull.Value ? (DateTimeOffset?)row["Entrada"] : null,
                                Pausa = row["Pausa"] != DBNull.Value ? (DateTimeOffset?)row["Pausa"] : null,
                                Retorno = row["Retorno"] != DBNull.Value ? (DateTimeOffset?)row["Retorno"] : null,
                                Saida = row["Saida"] != DBNull.Value ? (DateTimeOffset?)row["Saida"] : null,
                                Ds_Causa = row["Ds_Causa"].ToString(),
                                St_Ajuste = row["St_Ajuste"].ToString(),
                                Dh_Inclusao = (DateTime)row["Dh_Inclusao"],
                                Replica_Adm = row["Replica_Adm"].ToString(),
                                Data_Folha = (DateTime)row["Data_Folha"]
                            };
                            folhas.Add(folha);
                        }
                        return (new ListaSolicitacaoAjusteResponse { Solicitacoes = folhas, Mensagem = "Resultado OK", Sucesso = true });
                    }
                    else
                        return (new ListaSolicitacaoAjusteResponse { Solicitacoes = null, Mensagem = "Registro inválido", Sucesso = false });
                }

            }
            catch (Exception ex)
            {
                return (new ListaSolicitacaoAjusteResponse { Solicitacoes = null, Mensagem = "Ocorreu um erro: " + ex.Message, Sucesso = false });
            }
        }

        // POST api/values
        /// <summary>
        /// Inserir Ajuste
        /// </summary>
        [HttpPost]
        [Route("api/SolicitacaoAjuste")]
        public SolicitacaoAjusteResponse Post([FromBody] SolicitcaoAjuste folhaAjuste)
        {
            try
            {
                Repositorio repositorio = new Repositorio();
                var camposValores = new Dictionary<string, object>
                {
                    { "@id_funcionario", folhaAjuste.Id_Funcionario },
                    { "@id_folha", folhaAjuste.Id_Folha },
                    { "@entrada", folhaAjuste.Entrada },                   
                    { "@pausa", folhaAjuste.Pausa },
                    { "@retorno", folhaAjuste.Retorno },
                    { "@saida", folhaAjuste.Saida },
                    { "@ds_causa", folhaAjuste.Ds_Causa },
                    { "@st_ajuste", folhaAjuste.St_Ajuste },
                    { "@replica_adm", folhaAjuste.Replica_Adm },
                    { "@data_folha", folhaAjuste.Data_Folha },
                };

                var resultado = repositorio.InsertRegistro("SOLICITACAO_AJUSTE", camposValores);

                return (new SolicitacaoAjusteResponse { FolhaAjuste = folhaAjuste, Mensagem = resultado.Mensagem, Sucesso = resultado.Sucesso });
            }
            catch (Exception ex)
            {
                return (new SolicitacaoAjusteResponse { FolhaAjuste = null, Mensagem = "Ocorreu um erro: " + ex.Message, Sucesso = false });
            }
        }

        // PUT api/values/5
        /// <summary>
        /// Atualizar Ajuste
        /// </summary>
        [HttpPut]
        [Route("api/SolicitacaoAjuste/{id}")]
        public SolicitacaoAjusteResponse Put(int id, [FromBody] SolicitcaoAjuste folhaAjuste)
        {
            //st_ajuste (P-PENDENTE, R-REJEITADO, A-ACEITO, C-CANCELADO)

            try
            {
                Repositorio repositorio = new Repositorio();
                var camposValores = new Dictionary<string, object>
                {
                    { "@entrada", folhaAjuste.Entrada },
                    { "@pausa", folhaAjuste.Pausa },
                    { "@retorno", folhaAjuste.Retorno },
                    { "@saida", folhaAjuste.Saida },
                    { "@ds_causa", folhaAjuste.Ds_Causa },
                    { "@st_ajuste", folhaAjuste.St_Ajuste },
                    { "@replica_adm", folhaAjuste.Replica_Adm }
                };

                var resultado = repositorio.UpdateRegistro("SOLICITACAO_AJUSTE", camposValores, id);

                return (new SolicitacaoAjusteResponse { FolhaAjuste = folhaAjuste, Mensagem = resultado.Mensagem, Sucesso = resultado.Sucesso });
            }
            catch (Exception ex)
            {
                return (new SolicitacaoAjusteResponse { FolhaAjuste = null, Mensagem = "Ocorreu um erro: " + ex.Message, Sucesso = false });
            }
        }

        // DELETE api/values/5
        /// <summary>
        /// Deletar Ajuste
        /// </summary>
        [HttpDelete]
        [Route("api/SolicitacaoAjuste/{id}")]
        public SolicitacaoAjusteResponse Delete(int id)
        {
            try
            {
                Repositorio repositorio = new Repositorio();
                var resultado = repositorio.DeleteRegistro("SOLICITACAO_AJUSTE", id);

                return (new SolicitacaoAjusteResponse { FolhaAjuste = null, Mensagem = resultado.Mensagem, Sucesso = resultado.Sucesso });
            }
            catch (Exception ex)
            {
                return (new SolicitacaoAjusteResponse { FolhaAjuste = null, Mensagem = "Ocorreu um erro: " + ex.Message, Sucesso = false });
            }

        }

        public class ListaSolicitacaoAjusteResponse
        {
            public List<SolicitcaoAjuste> Solicitacoes { get; set; }
            public string Mensagem { get; set; }
            public bool Sucesso { get; set; }
        }
        public class SolicitacaoAjusteResponse
        {
            public SolicitcaoAjuste FolhaAjuste { get; set; }
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