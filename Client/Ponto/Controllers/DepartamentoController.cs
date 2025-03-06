using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Ponto.Models;
using Ponto.Views.Cadastro.Departamento;
using System.Data;
using System.Security.Claims;
using System.Text;
using static Ponto.Controllers.DepartamentoController;
using static Ponto.Controllers.HomeController;

namespace Ponto.Controllers
{
    public class DepartamentoController : Controller
    {
        private static readonly HttpClient client = new HttpClient();

        private readonly Routes _routes;
        public DepartamentoController(Routes routes)
        {
            _routes = routes;
        }

        [HttpGet]
        [Route("api/Departamento/Departamento/Departamento")]
        public async Task<IActionResult> Departamento(string username)
        {
            if (User.Identity.IsAuthenticated)
            {
                var response = await client.GetAsync(_routes.rota_departamento);

                var result = await response.Content.ReadAsStringAsync();
                var Departamentos = JsonConvert.DeserializeObject<List<Departamento>>(result);

                var model = new DepartamentoModel
                {
                    Departamentos = Departamentos
                };

                //var model = new DepartamentoModel
                //{
                //    Departamentos = new List<Departamento>
                //    {
                //    new Departamento { Id = 1, Descricao = "teste", Ativo = 'S' },
                //    new Departamento { Id = 2, Descricao = "teste2", Ativo = 'S' }
                //    }
                //};

                ViewData["Title"] = "Cadastro de Departamento";

                ViewBag.Username = User.Identity.Name;
                ViewBag.Departamentos = Departamentos;
                return View("/Views/Cadastro/Departamento/Departamento.cshtml", model);
            }
            return View("/Views/Cadastro/Departamento/Departamento.cshtml");
        }


        [HttpGet]
        [Route("api/Departamento/Departamento/Pesquisar/{id?}/{descricao?}")]
        public async Task<IActionResult> Pesquisar(int id = 0, string descricao = "")
        {
            if (User.Identity.IsAuthenticated)
            {
                var rota = _routes.rota_departamento + id.ToString();

                if (descricao != "")
                    rota += "/" + descricao;

                var response = await client.GetAsync(rota);

                var result = await response.Content.ReadAsStringAsync();
                var response2 = JsonConvert.DeserializeObject<ApiResponseLista>(result);


                if (response2.Sucesso == false)
                {
                    ViewBag.ErrorMessage = "Departamento não encontrado ou ID inválido.";
                    return View("/Views/Cadastro/Departamento/Departamento.cshtml", new DepartamentoModel { Departamentos = new List<Departamento>() });
                }

                var model = new DepartamentoModel
                {
                    Departamentos = response2.Departamentos
                };

                ViewData["Title"] = "Cadastro de Departamento";

                ViewBag.Username = User.Identity.Name;
                return View("/Views/Cadastro/Departamento/Departamento.cshtml", model);
            }
            return View("/Views/Cadastro/Departamento/Departamento.cshtml");
        }

        [HttpPost]
        [Route("api/Departamento/Departamento/Create")]
        public async Task<IActionResult> Create([FromBody] Departamento model)
        {
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(_routes.rota_departamento, content);
            var result = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(result);

            if (apiResponse.Sucesso)
            {
                var response2 = await client.GetAsync(_routes.rota_departamento);

                var result2 = await response2.Content.ReadAsStringAsync();
                var Departamentos = JsonConvert.DeserializeObject<List<Departamento>>(result2);

                var model2 = new DepartamentoModel
                {
                    Departamentos = Departamentos
                };

                ViewData["Title"] = "Cadastro de Departamento";
                ViewBag.Username = User.Identity.Name;
                return View("/Views/Cadastro/Departamento/Departamento.cshtml", model2);
            }
            else
            {
                ViewBag.ErrorMessage = apiResponse.Mensagem;
                return View("/Views/Cadastro/Departamento/Departamento.cshtml");
            }
        }

        [HttpDelete]
        [Route("api/Departamento/Departamento/Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            var response = await client.DeleteAsync(_routes.rota_departamento + id.ToString());
            var result = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(result);

            if (apiResponse.Sucesso)
            {
                var response2 = await client.GetAsync(_routes.rota_departamento);

                var result2 = await response2.Content.ReadAsStringAsync();
                var Departamentos = JsonConvert.DeserializeObject<List<Departamento>>(result2);

                var model2 = new DepartamentoModel
                {
                    Departamentos = Departamentos
                };

                ViewData["Title"] = "Cadastro de Departamento";
                ViewBag.Username = User.Identity.Name;
                return View("/Views/Cadastro/Departamento/Departamento.cshtml", model2);
            }
            else
            {
                ViewBag.ErrorMessage = apiResponse.Mensagem;
                return View("/Views/Cadastro/Departamento/Departamento.cshtml");
            }
        }

        [HttpPut]
        [Route("api/Departamento/Departamento/Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Departamento model)
        {
            if (model == null || model.Id != id)
            {
                return BadRequest("Dados do departamento inválidos.");
            }

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync(_routes.rota_departamentoAlt + id.ToString(), content);
            var result = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(result);

            if (apiResponse.Sucesso)
            {
                var response2 = await client.GetAsync(_routes.rota_departamento);

                var result2 = await response2.Content.ReadAsStringAsync();
                var Departamentos = JsonConvert.DeserializeObject<List<Departamento>>(result2);

                var model2 = new DepartamentoModel
                {
                    Departamentos = Departamentos
                };

                ViewData["Title"] = "Cadastro de Departamento";
                ViewBag.Username = User.Identity.Name;
                return View("/Views/Cadastro/Departamento/Departamento.cshtml", model2);
            }
            else
            {
                ViewBag.ErrorMessage = apiResponse.Mensagem;
                return View("/Views/Cadastro/Departamento/Departamento.cshtml");
            }
        }


        public class ApiResponse
        {
            public Departamento Departamento { get; set; }
            public string Mensagem { get; set; }
            public bool Sucesso { get; set; }
        }

        public class ApiResponseLista
        {
            public List<Departamento> Departamentos { get; set; }
            public string Mensagem { get; set; }
            public bool Sucesso { get; set; }
        }
    }
}
