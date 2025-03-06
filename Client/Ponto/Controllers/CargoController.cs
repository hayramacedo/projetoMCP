using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Ponto.Models;
using Ponto.Views.Cadastro.Cargo;
using Ponto.Views.Cadastro.Departamento;
using System.Text;

namespace Ponto.Controllers
{
    public class CargoController : Controller
    {
        private static readonly HttpClient client = new HttpClient();

        private readonly Routes _routes;
        public CargoController(Routes routes)
        {
            _routes = routes;
        }

        [HttpGet]
        [Route("api/Cargo/Cargo/Cargo")]
        public async Task<IActionResult> Cargo(string username)
        {
            if (User.Identity.IsAuthenticated)
            {
                var response = await client.GetAsync(_routes.rota_cargo);

                var result = await response.Content.ReadAsStringAsync();
                var Cargos = JsonConvert.DeserializeObject<List<Cargo>>(result);

                foreach (var cargo in Cargos)
                {
                    int idDepartamento = cargo.Id_Departamento;
                    string dsDepartamento = "";

                    var rotaDepartamento = _routes.rota_departamento + idDepartamento.ToString();
                    var response2 = await client.GetAsync(rotaDepartamento);
                    var result2 = await response2.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponseListaDepto>(result2);
                    if (apiResponse.Departamentos != null && apiResponse.Departamentos.Count > 0)
                    {
                        var departamento = apiResponse.Departamentos[0]; 
                        cargo.Departamento = departamento.Descricao;
                    }
                }

                var model = new CargoModel
                {
                    Cargos = Cargos
                };

                var response3 = await client.GetAsync(_routes.rota_departamento);

                var result3 = await response3.Content.ReadAsStringAsync();
                var Departamentos = JsonConvert.DeserializeObject<List<Departamento>>(result3);

                ViewData["Title"] = "Cadastro de Cargo";

                ViewBag.Username = User.Identity.Name;
                ViewBag.Departamentos = Departamentos;
                return View("/Views/Cadastro/Cargo/Cargo.cshtml", model);
            }
            return View("/Views/Cadastro/Cargo/Cargo.cshtml");
        }


        [HttpGet]
        [Route("api/Cargo/Cargo/Pesquisar/{id?}/{descricao?}")]
        public async Task<IActionResult> Pesquisar(int id = 0, string descricao = "")
        {
            if (User.Identity.IsAuthenticated)
            {
                var rota = _routes.rota_cargo + id.ToString();

                if (descricao != "")
                    rota += "/" + descricao;

                var response = await client.GetAsync(rota);

                var result = await response.Content.ReadAsStringAsync();
                var response2 = JsonConvert.DeserializeObject<ApiResponseLista>(result);


                if (response2.Sucesso == false)
                {
                    ViewBag.ErrorMessage = "Cargo não encontrado ou ID inválido.";
                    return View("/Views/Cadastro/Cargo/Cargo.cshtml", new CargoModel { Cargos = new List<Cargo>() });
                }

                var model = new CargoModel
                {
                    Cargos = response2.Cargos
                };

                var response3 = await client.GetAsync(_routes.rota_departamento);

                var result3 = await response3.Content.ReadAsStringAsync();
                var Departamentos = JsonConvert.DeserializeObject<List<Departamento>>(result3);

                ViewData["Title"] = "Cadastro de Cargo";

                ViewBag.Username = User.Identity.Name;
                ViewBag.Departamentos = Departamentos;
                return View("/Views/Cadastro/Cargo/Cargo.cshtml", model);
            }
            return View("/Views/Cadastro/Cargo/Cargo.cshtml");
        }



        [HttpPost]
        [Route("api/Cargo/Cargo/Create")]
        public async Task<IActionResult> Create([FromBody] Cargo model)
        {
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(_routes.rota_cargo, content);
            var result = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(result);

            if (apiResponse.Sucesso)
            {
                var response2 = await client.GetAsync(_routes.rota_cargo);

                var result2 = await response2.Content.ReadAsStringAsync();
                var Cargos = JsonConvert.DeserializeObject<List<Cargo>>(result2);

                var model2 = new CargoModel
                {
                    Cargos = Cargos
                };

                var response3 = await client.GetAsync(_routes.rota_departamento);

                var result3 = await response3.Content.ReadAsStringAsync();
                var Departamentos = JsonConvert.DeserializeObject<List<Departamento>>(result3);

                ViewData["Title"] = "Cadastro de Cargo";

                ViewBag.Username = User.Identity.Name;
                ViewBag.Departamentos = Departamentos;
                return View("/Views/Cadastro/Cargo/Cargo.cshtml", model2);
            }
            else
            {
                ViewBag.ErrorMessage = apiResponse.Mensagem;
                return View("/Views/Cadastro/Cargo/Cargo.cshtml");
            }
        }

        [HttpDelete]
        [Route("api/Cargo/Cargo/Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            var response = await client.DeleteAsync(_routes.rota_cargo + id.ToString());
            var result = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(result);

            if (apiResponse.Sucesso)
            {
                var response2 = await client.GetAsync(_routes.rota_cargo);

                var result2 = await response2.Content.ReadAsStringAsync();
                var Cargos = JsonConvert.DeserializeObject<List<Cargo>>(result2);

                var model2 = new CargoModel
                {
                    Cargos = Cargos
                };

                var response3 = await client.GetAsync(_routes.rota_departamento);

                var result3 = await response3.Content.ReadAsStringAsync();
                var Departamentos = JsonConvert.DeserializeObject<List<Departamento>>(result3);

                ViewData["Title"] = "Cadastro de Cargo";

                ViewBag.Username = User.Identity.Name;
                ViewBag.Departamentos = Departamentos;
                return View("/Views/Cadastro/Cargo/Cargo.cshtml", model2);
            }
            else
            {
                ViewBag.ErrorMessage = apiResponse.Mensagem;
                return View("/Views/Cadastro/Cargo/Cargo.cshtml");
            }
        }

        [HttpPut]
        [Route("api/Cargo/Cargo/Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Cargo model)
        {
            if (model == null || model.Id != id)
            {
                return BadRequest("Dados do cargo inválidos.");
            }

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync(_routes.rota_cargoAlt + id.ToString(), content);
            var result = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(result);

            if (apiResponse.Sucesso)
            {
                var response2 = await client.GetAsync(_routes.rota_cargo);

                var result2 = await response2.Content.ReadAsStringAsync();
                var Cargos = JsonConvert.DeserializeObject<List<Cargo>>(result2);

                var model2 = new CargoModel
                {
                    Cargos = Cargos
                };

                var response3 = await client.GetAsync(_routes.rota_departamento);

                var result3 = await response3.Content.ReadAsStringAsync();
                var Departamentos = JsonConvert.DeserializeObject<List<Departamento>>(result3);

                ViewData["Title"] = "Cadastro de Cargo";

                ViewBag.Username = User.Identity.Name;
                ViewBag.Departamentos = Departamentos;
                return View("/Views/Cadastro/Cargo/Cargo.cshtml", model2);
            }
            else
            {
                ViewBag.ErrorMessage = apiResponse.Mensagem;
                return View("/Views/Cadastro/Cargo/Cargo.cshtml");
            }
        }


        public class ApiResponse
        {
            public Cargo Cargo { get; set; }
            public string Mensagem { get; set; }
            public bool Sucesso { get; set; }
        }

        public class ApiResponseLista
        {
            public List<Cargo> Cargos { get; set; }
            public string Mensagem { get; set; }
            public bool Sucesso { get; set; }
        }

        public class ApiResponseListaDepto
        {
            public List<Departamento> Departamentos { get; set; }
            public string Mensagem { get; set; }
            public bool Sucesso { get; set; }
        }
    }
}
