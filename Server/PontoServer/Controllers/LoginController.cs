using Microsoft.Ajax.Utilities;
using PontoServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace PontoServer.Controllers
{
    public class LoginController : ApiController
    {
        private Repositorio _repository;

        public LoginController()
        {
            _repository = new Repositorio();
        }

        /// <summary>
        /// Validação do Login
        /// </summary>
        [HttpPost]
        [Route("api/login")]
        public IHttpActionResult Login([FromBody] Login login)
        {

            if (login == null)
            {
                return BadRequest("Campos em branco!");
            }

            bool isAuthenticated = _repository.VerificaAutenticacao(login);

            if (isAuthenticated)
            {
                return Ok(new { Mensagem = "Login efetuado com sucesso!", Sucesso = true } );
            }
            else
            {
                return Ok(new { Mensagem = "Usuário ou Senha inválidos!", Sucesso = false });//Unauthorized();
            }
        }
    }
}