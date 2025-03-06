using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Ponto.Models;
using Ponto.Views.Cadastro.Departamento;
using Ponto.Views.Cadastro.Empresa;
using System.Text;

namespace Ponto.Controllers
{
    public class EmpresaController : Controller
    {
        private static readonly HttpClient client = new HttpClient();

        private readonly Routes _routes;
        public EmpresaController(Routes routes)
        {
            _routes = routes;
        }

        [HttpGet]
        [Route("api/Empresa/Empresa/Empresa")]
        public async Task<IActionResult> Empresa(string username)
        {
            if (User.Identity.IsAuthenticated)
            {
                var response = await client.GetAsync(_routes.rota_empresa);

                var result = await response.Content.ReadAsStringAsync();
                var Empresas = JsonConvert.DeserializeObject<List<Empresa>>(result);

                var model = new EmpresaModel
                {
                    Empresas = Empresas
                };

                ViewData["Title"] = "Cadastro de Empresa";

                ViewBag.Username = User.Identity.Name;
                return View("/Views/Cadastro/Empresa/Empresa.cshtml", model);
            }
            return View("/Views/Cadastro/Empresa/Empresa.cshtml");
        }


        [HttpGet]
        [Route("api/Empresa/Empresa/Pesquisar/{id?}/{razao?}")]
        public async Task<IActionResult> Pesquisar(int id = 0, string razao = "")
        {
            if (User.Identity.IsAuthenticated)
            {
                var rota = _routes.rota_empresa + id.ToString();

                if (razao != "")
                    rota += "/" + razao;

                var response = await client.GetAsync(rota);

                var result = await response.Content.ReadAsStringAsync();
                var response2 = JsonConvert.DeserializeObject<ApiResponseLista>(result);


                if (response2.Sucesso == false)
                {
                    ViewBag.ErrorMessage = "Empresa não encontrada ou ID inválido.";
                    return View("/Views/Cadastro/Empresa/Empresa.cshtml", new EmpresaModel { Empresas = new List<Empresa>() });
                }

                var model = new EmpresaModel
                {
                    Empresas = response2.Empresas
                };

                ViewData["Title"] = "Cadastro de Empresa";

                ViewBag.Username = User.Identity.Name;
                return View("/Views/Cadastro/Empresa/Empresa.cshtml", model);
            }
            return View("/Views/Cadastro/Empresa/Empresa.cshtml");
        }

        [HttpPost]
        [Route("api/Empresa/Empresa/Create")]
        public async Task<IActionResult> Create([FromBody] Empresa model)
        {
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(_routes.rota_empresa, content);
            var result = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(result);

            if (apiResponse.Sucesso)
            {
                var response2 = await client.GetAsync(_routes.rota_empresa);

                var result2 = await response2.Content.ReadAsStringAsync();
                var Empresas = JsonConvert.DeserializeObject<List<Empresa>>(result2);

                var model2 = new EmpresaModel
                {
                    Empresas = Empresas
                };

                ViewData["Title"] = "Cadastro de Empresa";
                ViewBag.Username = User.Identity.Name;
                return View("/Views/Cadastro/Empresa/Empresa.cshtml", model2);
            }
            else
            {
                ViewBag.ErrorMessage = apiResponse.Mensagem;
                return View("/Views/Cadastro/Empresa/Empresa.cshtml");
            }
        }

        [HttpDelete]
        [Route("api/Empresa/Empresa/Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            var response = await client.DeleteAsync(_routes.rota_empresa + id.ToString());
            var result = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(result);

            if (apiResponse.Sucesso)
            {
                var response2 = await client.GetAsync(_routes.rota_empresa);

                var result2 = await response2.Content.ReadAsStringAsync();
                var Empresas = JsonConvert.DeserializeObject<List<Empresa>>(result2);

                var model2 = new EmpresaModel
                {
                    Empresas = Empresas
                };

                ViewData["Title"] = "Cadastro de Empresa";
                ViewBag.Username = User.Identity.Name;
                return View("/Views/Cadastro/Empresa/Empresa.cshtml", model2);
            }
            else
            {
                ViewBag.ErrorMessage = apiResponse.Mensagem;
                return View("/Views/Cadastro/Empresa/Empresa.cshtml");
            }
        }

        [HttpPut]
        [Route("api/Empresa/Empresa/Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Empresa model)
        {
            if (model == null || model.Id != id)
            {
                return BadRequest("Dados da empresa inválidos.");
            }

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync(_routes.rota_empresaAlt + id.ToString(), content);
            var result = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(result);

            if (apiResponse.Sucesso)
            {
                var response2 = await client.GetAsync(_routes.rota_empresa);

                var result2 = await response2.Content.ReadAsStringAsync();
                var Empresas = JsonConvert.DeserializeObject<List<Empresa>>(result2);

                var model2 = new EmpresaModel
                {
                    Empresas = Empresas
                };

                ViewData["Title"] = "Cadastro de Empresa";
                ViewBag.Username = User.Identity.Name;
                return View("/Views/Cadastro/Empresa/Empresa.cshtml", model2);
            }
            else
            {
                ViewBag.ErrorMessage = apiResponse.Mensagem;
                return View("/Views/Cadastro/Empresa/Empresa.cshtml");
            }
        }


        public class ApiResponse
        {
            public Empresa Empresa { get; set; }
            public string Mensagem { get; set; }
            public bool Sucesso { get; set; }
        }

        public class ApiResponseLista
        {
            public List<Empresa> Empresas { get; set; }
            public string Mensagem { get; set; }
            public bool Sucesso { get; set; }
        }
    }
}
