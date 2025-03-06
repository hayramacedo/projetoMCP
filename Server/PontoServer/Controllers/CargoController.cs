using PontoServer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Http;
using static PontoServer.Controllers.DepartamentoController;

namespace PontoServer.Controllers
{
    public class CargoController : ApiController
    {
        // GET api/values
        /// <summary>
        /// Traz todos os Cargos
        /// </summary>
        [HttpGet]
        [Route("api/Cargos")]
        public List<Cargo> Get()
        {
            Repositorio repositorio = new Repositorio();
            List<Cargo> cargos;
            using (var tabela = repositorio.GetDataTable("CARGO"))
            {
                cargos = new List<Cargo>();
                foreach (DataRow row in tabela.Rows)
                {
                    Cargo cargo = new Cargo
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        Descricao = row["Descricao"].ToString(),
                        Hora_Jornada = Int32.Parse(row["Hora_Jornada"].ToString()),
                        Dh_Inclusao = (DateTime)row["Dh_Inclusao"],
                        Id_Departamento = Int32.Parse(row["Id_Departamento"].ToString()),
                        Ativo = (char)row["Ativo"]

                    };

                    cargos.Add(cargo);
                }
            }
            return cargos;
        }

        //// GET api/values/5
        ///// <summary>
        ///// Traz o Cargo referente ao ID
        ///// </summary>
        //[HttpGet]
        //[Route("api/Cargos/{id}")]
        //public CargoResponse Get(int id)
        //{
        //    Repositorio repositorio = new Repositorio();
        //    Cargo cargo;
        //    try
        //    {
        //        using (var tabela = repositorio.GetByID("CARGO", id))
        //        {
        //            if (tabela.Rows.Count > 0)
        //            {
        //                cargo = new Cargo
        //                {
        //                    Id = Convert.ToInt32(tabela.Rows[0]["Id"]),
        //                    Descricao = tabela.Rows[0]["Descricao"].ToString(),
        //                    Hora_Jornada = Int32.Parse(tabela.Rows[0]["Hora_Jornada"].ToString()),
        //                    Dh_Inclusao = (DateTime)tabela.Rows[0]["Dh_Inclusao"],
        //                    Id_Departamento = Int32.Parse(tabela.Rows[0]["Id_Departamento"].ToString()),
        //                    Ativo = (char)tabela.Rows[0]["Ativo"]
        //                };
        //                return (new CargoResponse { Cargo = cargo, Mensagem = "Resultado OK", Sucesso = true });
        //            }
        //            else
        //                return (new CargoResponse { Cargo = null, Mensagem = "Cargo inválido", Sucesso = false });
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        return (new CargoResponse { Cargo = null, Mensagem = "Ocorreu um erro: " + ex.Message, Sucesso = false });
        //    }
        //}

        // GET api/values/5
        /// <summary>
        /// Traz o Cargo referente ao ID ou Descricao
        /// </summary>
        [HttpGet]
        [Route("api/Cargos/{id?}/{descricao?}")]
        public ListaCargoResponse Get(int id = 0, string descricao = "")
        {
            Repositorio repositorio = new Repositorio();
            List<Cargo> cargos;
            try
            {
                using (var tabela = repositorio.GetByParametros("CARGO", id, "Descricao", descricao))
                {
                    if (tabela.Rows.Count > 0)
                    {
                        cargos = new List<Cargo>();
                        foreach (DataRow row in tabela.Rows)
                        {
                            Cargo cargo = new Cargo
                            {
                                Id = Convert.ToInt32(row["Id"]),
                                Descricao = row["Descricao"].ToString(),
                                Hora_Jornada = Int32.Parse(row["Hora_Jornada"].ToString()),
                                Dh_Inclusao = (DateTime)row["Dh_Inclusao"],
                                Id_Departamento = Int32.Parse(row["Id_Departamento"].ToString()),
                                Ativo = (char)row["Ativo"]
                            };

                            cargos.Add(cargo);
                        }
                        return (new ListaCargoResponse { Cargos = cargos, Mensagem = "Resultado OK", Sucesso = true });
                    }
                    else
                        return (new ListaCargoResponse { Cargos = null, Mensagem = "Departamento inválido", Sucesso = false });
                }

            }
            catch (Exception ex)
            {
                return (new ListaCargoResponse { Cargos = null, Mensagem = "Ocorreu um erro: " + ex.Message, Sucesso = false });
            }
        }

        // POST api/values
        /// <summary>
        /// Inserir Cargo
        /// </summary>
        [HttpPost]
        [Route("api/Cargos")]
        public CargoResponse Post([FromBody] Cargo cargo)
        {
            try
            {
                Repositorio repositorio = new Repositorio();
                var camposValores = new Dictionary<string, object>
                {
                    { "@descricao", cargo.Descricao },
                    { "@hora_jornada", cargo.Hora_Jornada },
                    { "@id_departamento", cargo.Id_Departamento }
                };

                var resultado = repositorio.InsertRegistro("CARGO", camposValores);

                return (new CargoResponse { Cargo = cargo, Mensagem = resultado.Mensagem, Sucesso = resultado.Sucesso });
            }
            catch (Exception ex)
            {
                return (new CargoResponse { Cargo = null, Mensagem = "Ocorreu um erro: " + ex.Message, Sucesso = false });
            }
        }

        // PUT api/values/5
        /// <summary>
        /// Atualizar Empresa
        /// </summary>
        [HttpPut]
        [Route("api/Cargos")]
        public CargoResponse Put(int id, [FromBody] Cargo cargo)
        {
            try
            {
                Repositorio repositorio = new Repositorio();
                var camposValores = new Dictionary<string, object>
                {
                    { "@descricao", cargo.Descricao },
                    { "@hora_jornada", cargo.Hora_Jornada },
                    { "@id_departamento", cargo.Id_Departamento },
                    { "@ativo", cargo.Ativo }
                };

                var resultado = repositorio.UpdateRegistro("CARGO", camposValores, id);

                return (new CargoResponse { Cargo = cargo, Mensagem = resultado.Mensagem, Sucesso = resultado.Sucesso });
            }
            catch (Exception ex)
            {
                return (new CargoResponse { Cargo = null, Mensagem = "Ocorreu um erro: " + ex.Message, Sucesso = false });
            }
        }

        // DELETE api/values/5
        /// <summary>
        /// Deletar Empresa
        /// </summary>
        [HttpDelete]
        [Route("api/Cargos/{id}")]
        public CargoResponse Delete(int id)
        {
            try
            {
                Repositorio repositorio = new Repositorio();
                var resultado = repositorio.DeleteRegistro("CARGO", id);

                return (new CargoResponse { Cargo = null, Mensagem = resultado.Mensagem, Sucesso = resultado.Sucesso });
            }
            catch (Exception ex)
            {
                return (new CargoResponse { Cargo = null, Mensagem = "Ocorreu um erro: " + ex.Message, Sucesso = false });
            }

        }

        public class CargoResponse
        {
            public Cargo Cargo { get; set; }
            public string Mensagem { get; set; }
            public bool Sucesso { get; set; }
        }

        public class ListaCargoResponse
        {
            public List<Cargo> Cargos { get; set; }
            public string Mensagem { get; set; }
            public bool Sucesso { get; set; }
        }
    }
}