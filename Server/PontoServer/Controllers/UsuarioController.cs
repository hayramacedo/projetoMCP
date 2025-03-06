using PontoServer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web.Http;


namespace PontoServer.Controllers
{
    public class UsuarioController : ApiController
    {
        // GET api/values
        /// <summary>
        /// Traz todos Usuarios
        /// </summary>
        [HttpGet]
        [Route("api/Usuarios")]
        public List<Usuario> Get()
        {
            Repositorio repositorio = new Repositorio();
            List<Usuario> usuarios;
            using (var tabela = repositorio.GetDataTable("USUARIO"))
            {
                usuarios = new List<Usuario>();
                foreach (DataRow row in tabela.Rows)
                {                 
                    Usuario usuario = new Usuario
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        Username = row["Username"].ToString(),
                        Password = row["Password"].ToString(),
                        Dh_Inclusao = (DateTime)row["Dh_Inclusao"],
                        Nickname = row["Nickname"].ToString(),
                        Ativo = (char)row["Ativo"]
                    };

                    usuarios.Add(usuario);
                }
            }
            return usuarios;
        }

        // GET api/values/5
        /// <summary>
        /// Traz o Usuario referente ao ID
        /// </summary>
        [HttpGet]
        [Route("api/Usuarios/{id}")]
        public UsuarioResponse Get(int id)
        {
            Repositorio repositorio = new Repositorio();
            Usuario usuario;
            try
            {
                using (var tabela = repositorio.GetByID("USUARIO", id))
                {
                    if (tabela.Rows.Count > 0)
                    {
                        usuario = new Usuario
                        {
                            Id = Convert.ToInt32(tabela.Rows[0]["Id"]),
                            Username = tabela.Rows[0]["Username"].ToString(),
                            Password = tabela.Rows[0]["Password"].ToString(),
                            Dh_Inclusao = (DateTime)tabela.Rows[0]["Dh_Inclusao"],
                            Nickname = tabela.Rows[0]["Nickname"].ToString(),
                            Ativo = (char)tabela.Rows[0]["Ativo"]
                        };
                        return (new UsuarioResponse{ Usuario = usuario, Mensagem = "Resultado OK", Sucesso = true });
                    }
                    else
                       return (new UsuarioResponse { Usuario = null, Mensagem = "Usuário inválido", Sucesso = false });
                }
               
            }
            catch (Exception ex)
            {
                return (new UsuarioResponse { Usuario = null, Mensagem = "Ocorreu um erro: " + ex.Message, Sucesso = false});
            }            
        }

        // POST api/values
        /// <summary>
        /// Inserir Usuário
        /// </summary>
        [HttpPost]
        [Route("api/Usuarios")]
        public UsuarioResponse Post([FromBody] Usuario usuario)
        {
            try
            {
                Repositorio repositorio = new Repositorio();
                var camposValores = new Dictionary<string, object>
                {
                    { "@username", usuario.Username },
                    { "@password", usuario.Password },
                    { "@nickname", usuario.Nickname }
                };

                var resultado = repositorio.InsertRegistro("USUARIO", camposValores);
                var data = repositorio.GetByUsername(usuario.Username);

                Usuario usuarioInserido = new Usuario() 
                {
                    Id = Convert.ToInt32(data.Rows[0]["Id"]),
                    Username = data.Rows[0]["Username"].ToString(),
                    Password = data.Rows[0]["Password"].ToString(),
                    Dh_Inclusao = (DateTime)data.Rows[0]["Dh_Inclusao"],
                    Nickname = data.Rows[0]["Nickname"].ToString(),
                    Ativo = (char)data.Rows[0]["Ativo"]
                };

            return (new UsuarioResponse { Usuario = usuarioInserido, Mensagem = resultado.Mensagem, Sucesso = resultado.Sucesso });
            }
            catch (Exception ex)
            {
                return (new UsuarioResponse { Usuario = null, Mensagem = "Ocorreu um erro: " + ex.Message, Sucesso = false });
            }
        }

        // PUT api/values/5
        /// <summary>
        /// Atualizar Usuário
        /// </summary>
        [HttpPut]
        [Route("api/Usuarios")]
        public UsuarioResponse Put(int id, [FromBody] Usuario usuario)
        {
            try
            {
                Repositorio repositorio = new Repositorio();
                var camposValores = new Dictionary<string, object>
                {
                    { "@username", usuario.Username },
                    { "@password", usuario.Password },
                    { "@nickname", usuario.Nickname },
                    { "@ativo", usuario.Ativo }
                };

                var resultado = repositorio.UpdateRegistro("USUARIO", camposValores, id);

                return (new UsuarioResponse { Usuario = usuario, Mensagem = resultado.Mensagem, Sucesso = resultado.Sucesso });
            }
            catch (Exception ex)
            {
                return (new UsuarioResponse { Usuario = null, Mensagem = "Ocorreu um erro: " + ex.Message, Sucesso = false });
            }
        }

        // DELETE api/values/5
        /// <summary>
        /// Deletar Usuário
        /// </summary>
        [HttpDelete]
        [Route("api/Usuarios/{id}")]
        public UsuarioResponse Delete(int id)
        {
            try
            {
                Repositorio repositorio = new Repositorio();
                var resultado = repositorio.DeleteRegistro("USUARIO",id);

                return (new UsuarioResponse { Usuario = null, Mensagem = resultado.Mensagem, Sucesso = resultado.Sucesso });
            }
            catch (Exception ex)
            {
                return (new UsuarioResponse { Usuario = null, Mensagem = "Ocorreu um erro: " + ex.Message, Sucesso = false });
            }

        }


        // GET api/values/5
        /// <summary>
        /// Traz o Usuario referente ao Username
        /// </summary>
        [HttpGet]
        [Route("api/Usuarios/selecionar/{Username}")]
        public UsuarioResponse Get(string username)
        {
            Repositorio repositorio = new Repositorio();
            Usuario usuario;
            try
            {
                using (var tabela = repositorio.GetByUsername(username))
                {
                    if (tabela.Rows.Count > 0)
                    {
                        usuario = new Usuario
                        {
                            Id = Convert.ToInt32(tabela.Rows[0]["Id"]),
                            Username = tabela.Rows[0]["Username"].ToString(),
                            Password = tabela.Rows[0]["Password"].ToString(),
                            Dh_Inclusao = (DateTime)tabela.Rows[0]["Dh_Inclusao"],
                            Nickname = tabela.Rows[0]["Nickname"].ToString(),
                            Ativo = (char)tabela.Rows[0]["Ativo"]
                        };
                        return (new UsuarioResponse { Usuario = usuario, Mensagem = "Resultado OK", Sucesso = true });
                    }
                    else
                        return (new UsuarioResponse { Usuario = null, Mensagem = "Usuário inválido", Sucesso = false });
                }

            }
            catch (Exception ex)
            {
                return (new UsuarioResponse { Usuario = null, Mensagem = "Ocorreu um erro: " + ex.Message, Sucesso = false });
            }
        }

        // GET api/values/5
        /// <summary>
        /// Traz o Usuario referente ao Nickname
        /// </summary>
        [HttpGet]
        [Route("api/Usuarios/selecionarNickname/{nickname}")]
        public UsuarioResponse GetNickname(string nickname)
        {
            Repositorio repositorio = new Repositorio();
            Usuario usuario;
            try
            {
                using (var tabela = repositorio.GetByNickname(nickname))
                {
                    if (tabela.Rows.Count > 0)
                    {
                        usuario = new Usuario
                        {
                            Id = Convert.ToInt32(tabela.Rows[0]["Id"]),
                            Username = tabela.Rows[0]["Username"].ToString(),
                            Password = tabela.Rows[0]["Password"].ToString(),
                            Dh_Inclusao = (DateTime)tabela.Rows[0]["Dh_Inclusao"],
                            Nickname = tabela.Rows[0]["Nickname"].ToString(),
                            Ativo = (char)tabela.Rows[0]["Ativo"]
                        };
                        return (new UsuarioResponse { Usuario = usuario, Mensagem = "Resultado OK", Sucesso = true });
                    }
                    else
                        return (new UsuarioResponse { Usuario = null, Mensagem = "Usuário inválido", Sucesso = false });
                }

            }
            catch (Exception ex)
            {
                return (new UsuarioResponse { Usuario = null, Mensagem = "Ocorreu um erro: " + ex.Message, Sucesso = false });
            }
        }



        public class UsuarioResponse
        {
            public Usuario Usuario { get; set; }
            public string Mensagem { get; set; }
            public bool Sucesso { get; set; }
        }
    }
}
