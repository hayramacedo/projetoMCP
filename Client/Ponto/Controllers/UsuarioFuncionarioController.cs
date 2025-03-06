using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Ponto.Models;
using Ponto.Views.Cadastro.Cargo;
using Ponto.Views.Cadastro.UsuarioFuncionario;
using System.Text;
using static Ponto.Controllers.CargoController;

namespace Ponto.Controllers
{
    public class UsuarioFuncionarioController : Controller
    {
        private static readonly HttpClient client = new HttpClient();

        private readonly Routes _routes;
        public UsuarioFuncionarioController(Routes routes)
        {
            _routes = routes;
        }

        [HttpGet]
        [Route("api/UsuarioFuncionario/UsuarioFuncionario/UsuarioFuncionario")]
        public async Task<IActionResult> UsuarioFuncionario(string username)
        {
            ViewBag.Empresas = new List<Empresa> { };
            ViewBag.Cargos = new List<Cargo> { };
            ViewBag.Escalas = new List<Escala> { };

            if (User.Identity.IsAuthenticated)
            {
                var response = await client.GetAsync(_routes.rota_funcionario);

                var result = await response.Content.ReadAsStringAsync();
                var Funcionarios = JsonConvert.DeserializeObject<List<Funcionario>>(result);


                foreach (var funcionario in Funcionarios)
                {
                    int idusuario = funcionario.Id_Usuario;
 
                    var rotaUsuario = _routes.rota_usuario + idusuario.ToString();
                    var response2 = await client.GetAsync(rotaUsuario);
                    var result2 = await response2.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponseListaUsuario>(result2);
                    if (apiResponse.Usuario != null)
                    {
                        Usuario usuario = apiResponse.Usuario;
                        funcionario.Usuario = usuario;
                    }
                }

                var model = new UsuarioFuncionarioModel
                {
                    Funcionarios = Funcionarios
                };

                //Combobox Empresas
                var response3 = await client.GetAsync(_routes.rota_empresa);
                var result3 = await response3.Content.ReadAsStringAsync();
                var Empresas = JsonConvert.DeserializeObject<List<Empresa>>(result3);
                ViewBag.Empresas = Empresas;

                //Combobox Cargos
                var response4 = await client.GetAsync(_routes.rota_cargo);
                var result4 = await response4.Content.ReadAsStringAsync();
                var Cargos = JsonConvert.DeserializeObject<List<Cargo>>(result4);
                ViewBag.Cargos = Cargos;

                //Combobox Escalas
                var response5 = await client.GetAsync(_routes.rota_escala);
                var result5 = await response5.Content.ReadAsStringAsync();
                var Escalas = JsonConvert.DeserializeObject<List<Escala>>(result5);
                ViewBag.Escalas = Escalas;

                ViewData["Title"] = "Cadastro de Funcionario";

                ViewBag.Username = User.Identity.Name;
                return View("/Views/Cadastro/UsuarioFuncionario/UsuarioFuncionario.cshtml", model);
            }
            return View("/Views/Cadastro/UsuarioFuncionario/UsuarioFuncionario.cshtml");
        }


        [HttpGet]
        [Route("api/UsuarioFuncionario/UsuarioFuncionario/Pesquisar/{id?}/{nome?}")]
        public async Task<IActionResult> Pesquisar(int id = 0, string nome = "")
        {
            ViewBag.Empresas = new List<Empresa> { };
            ViewBag.Cargos = new List<Cargo> { };
            ViewBag.Escalas = new List<Escala> { };

            if (User.Identity.IsAuthenticated)
            {
                var rota = _routes.rota_funcionario + id.ToString();

                if (nome != "")
                    rota += "/" + nome;

                var response = await client.GetAsync(rota);

                var result = await response.Content.ReadAsStringAsync();
                var response2 = JsonConvert.DeserializeObject<ApiResponseLista>(result);


                if (response2.Sucesso == false)
                {
                    ViewBag.ErrorMessage = "Cargo não encontrado ou ID inválido.";
                    return View("/Views/Cadastro/UsuarioFuncionario/UsuarioFuncionario.cshtml", new UsuarioFuncionarioModel { Funcionarios = new List<Funcionario>() });
                }

                foreach (var funcionario in response2.Funcionarios)
                {
                    int idusuario = funcionario.Id_Usuario;

                    var rotaUsuario = _routes.rota_usuario + idusuario.ToString();
                    var response3 = await client.GetAsync(rotaUsuario);
                    var result3 = await response3.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponseListaUsuario>(result3);
                    if (apiResponse.Usuario != null)
                    {
                        Usuario usuario = apiResponse.Usuario;
                        funcionario.Usuario = usuario;
                    }
                }

                var model = new UsuarioFuncionarioModel
                {
                    Funcionarios = response2.Funcionarios
                };

                //Combobox Empresas
                var response4 = await client.GetAsync(_routes.rota_empresa);
                var result4 = await response4.Content.ReadAsStringAsync();
                var Empresas = JsonConvert.DeserializeObject<List<Empresa>>(result4);
                ViewBag.Empresas = Empresas;

                //Combobox Cargos
                var response5 = await client.GetAsync(_routes.rota_cargo);
                var result5 = await response5.Content.ReadAsStringAsync();
                var Cargos = JsonConvert.DeserializeObject<List<Cargo>>(result5);
                ViewBag.Cargos = Cargos;

                //Combobox Escalas
                var response6 = await client.GetAsync(_routes.rota_escala);
                var result6 = await response6.Content.ReadAsStringAsync();
                var Escalas = JsonConvert.DeserializeObject<List<Escala>>(result6);
                ViewBag.Escalas = Escalas;

                ViewData["Title"] = "Cadastro de Funcionario";

                ViewBag.Username = User.Identity.Name;
                return View("/Views/Cadastro/UsuarioFuncionario/UsuarioFuncionario.cshtml", model);
            }
            return View("/Views/Cadastro/UsuarioFuncionario/UsuarioFuncionario.cshtml");
        }

        [HttpPost]
        [Route("api/UsuarioFuncionario/UsuarioFuncionario/Create")]
        public async Task<IActionResult> Create([FromBody] Funcionario model)
        {
            ViewBag.Empresas = new List<Empresa> { };
            ViewBag.Cargos = new List<Cargo> { };
            ViewBag.Escalas = new List<Escala> { };

            var json = JsonConvert.SerializeObject(model.Usuario);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(_routes.rota_usuario, content);
            var result = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponseUsu>(result);

            if (apiResponse.Sucesso)
            {
                var json2 = JsonConvert.SerializeObject(model);
                var content2 = new StringContent(json2, Encoding.UTF8, "application/json");

                var response2 = await client.PutAsync(_routes.rota_funcionarioAlt + apiResponse.Usuario.Id.ToString(), content2);
                var result2 = await response2.Content.ReadAsStringAsync();
                var apiResponse2 = JsonConvert.DeserializeObject<ApiResponse>(result2);


                if (apiResponse2.Sucesso)
                {

                    var response3 = await client.GetAsync(_routes.rota_funcionario);

                    var result3 = await response3.Content.ReadAsStringAsync();
                    var Funcionarios = JsonConvert.DeserializeObject<List<Funcionario>>(result3);

                    foreach (var funcionario in Funcionarios)
                    {
                        int idusuario = funcionario.Id_Usuario;

                        var rotaUsuario = _routes.rota_usuario + idusuario.ToString();
                        var response4 = await client.GetAsync(rotaUsuario);
                        var result4 = await response4.Content.ReadAsStringAsync();
                        var apiResponse3 = JsonConvert.DeserializeObject<ApiResponseListaUsuario>(result4);
                        if (apiResponse3.Usuario != null)
                        {
                            Usuario usuario = apiResponse3.Usuario;
                            funcionario.Usuario = usuario;
                        }
                    }

                    var model2 = new UsuarioFuncionarioModel
                    {
                        Funcionarios = Funcionarios
                    };

                    //Combobox Empresas
                    var response5 = await client.GetAsync(_routes.rota_empresa);
                    var result5 = await response5.Content.ReadAsStringAsync();
                    var Empresas = JsonConvert.DeserializeObject<List<Empresa>>(result5);
                    ViewBag.Empresas = Empresas;

                    //Combobox Cargos
                    var response6 = await client.GetAsync(_routes.rota_cargo);
                    var result6 = await response6.Content.ReadAsStringAsync();
                    var Cargos = JsonConvert.DeserializeObject<List<Cargo>>(result6);
                    ViewBag.Cargos = Cargos;

                    //Combobox Escalas
                    var response7 = await client.GetAsync(_routes.rota_escala);
                    var result7 = await response7.Content.ReadAsStringAsync();
                    var Escalas = JsonConvert.DeserializeObject<List<Escala>>(result7);
                    ViewBag.Escalas = Escalas;

                    ViewData["Title"] = "Cadastro de Funcionario";

                    ViewBag.Username = User.Identity.Name;
                    return View("/Views/Cadastro/UsuarioFuncionario/UsuarioFuncionario.cshtml", model2);
                }
                else
                {
                    ViewBag.ErrorMessage = apiResponse.Mensagem;
                    return View("/Views/Cadastro/UsuarioFuncionario/UsuarioFuncionario.cshtml");
                }
            }
            else
            {
                ViewBag.ErrorMessage = apiResponse.Mensagem;
                return View("/Views/Cadastro/UsuarioFuncionario/UsuarioFuncionario.cshtml");
            }
        }

        [HttpDelete]
        [Route("api/UsuarioFuncionario/UsuarioFuncionario/Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.Empresas = new List<Empresa> { };
            ViewBag.Cargos = new List<Cargo> { };
            ViewBag.Escalas = new List<Escala> { };

            var response = await client.DeleteAsync(_routes.rota_funcionario + id.ToString());
            var result = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(result);

            if (apiResponse.Sucesso)
            {

                var response2 = await client.DeleteAsync(_routes.rota_usuario + id.ToString());
                var result2 = await response2.Content.ReadAsStringAsync();
                var apiResponse2 = JsonConvert.DeserializeObject<ApiResponse>(result2);

                if (apiResponse2.Sucesso)
                {

                    var response3 = await client.GetAsync(_routes.rota_funcionario);

                    var result3 = await response3.Content.ReadAsStringAsync();
                    var Funcionarios = JsonConvert.DeserializeObject<List<Funcionario>>(result3);


                    foreach (var funcionario in Funcionarios)
                    {
                        int idusuario = funcionario.Id_Usuario;

                        var rotaUsuario = _routes.rota_usuario + idusuario.ToString();
                        var response4 = await client.GetAsync(rotaUsuario);
                        var result4 = await response4.Content.ReadAsStringAsync();
                        var apiResponse3 = JsonConvert.DeserializeObject<ApiResponseListaUsuario>(result4);
                        if (apiResponse3.Usuario != null)
                        {
                            Usuario usuario = apiResponse3.Usuario;
                            funcionario.Usuario = usuario;
                        }
                    }

                    var model = new UsuarioFuncionarioModel
                    {
                        Funcionarios = Funcionarios
                    };

                    //Combobox Empresas
                    var response5 = await client.GetAsync(_routes.rota_empresa);
                    var result5 = await response5.Content.ReadAsStringAsync();
                    var Empresas = JsonConvert.DeserializeObject<List<Empresa>>(result5);
                    ViewBag.Empresas = Empresas;

                    //Combobox Cargos
                    var response6 = await client.GetAsync(_routes.rota_cargo);
                    var result6 = await response6.Content.ReadAsStringAsync();
                    var Cargos = JsonConvert.DeserializeObject<List<Cargo>>(result6);
                    ViewBag.Cargos = Cargos;

                    //Combobox Escalas
                    var response7 = await client.GetAsync(_routes.rota_escala);
                    var result7 = await response7.Content.ReadAsStringAsync();
                    var Escalas = JsonConvert.DeserializeObject<List<Escala>>(result7);
                    ViewBag.Escalas = Escalas;

                    ViewData["Title"] = "Cadastro de Funcionario";

                    ViewBag.Username = User.Identity.Name;
                    return View("/Views/Cadastro/UsuarioFuncionario/UsuarioFuncionario.cshtml", model);
                }
                else
                {
                    ViewBag.ErrorMessage = apiResponse.Mensagem;
                    return View("/Views/Cadastro/UsuarioFuncionario/UsuarioFuncionario.cshtml");
                }             
            }
            else
            {
                ViewBag.ErrorMessage = apiResponse.Mensagem;
                return View("/Views/Cadastro/UsuarioFuncionario/UsuarioFuncionario.cshtml");
            }
        }

        [HttpPut]
        [Route("api/UsuarioFuncionario/UsuarioFuncionario/Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Funcionario model)
        {
            ViewBag.Empresas = new List<Empresa> { };
            ViewBag.Cargos = new List<Cargo> { };
            ViewBag.Escalas = new List<Escala> { };

            if (model == null || model.Id != id)
            {
                return BadRequest("Dados do funcionario inválidos.");
            }

            var response8 = await client.GetAsync(_routes.rota_usuario_username + model.Usuario.Username);
            var result8 = await response8.Content.ReadAsStringAsync();
            var respUsu = JsonConvert.DeserializeObject<ApiResponseUsu>(result8);

            model.Id_Usuario = respUsu.Usuario.Id;
            model.Usuario.Id = respUsu.Usuario.Id;

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
         
            var response = await client.PutAsync(_routes.rota_funcionarioAlt + id.ToString(), content);
            var result = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(result);


            var json2 = JsonConvert.SerializeObject(model.Usuario);
            var content2 = new StringContent(json2, Encoding.UTF8, "application/json");

            var response2 = await client.PutAsync(_routes.rota_usuarioAlt + model.Usuario.Id.ToString(), content2);
            var result2 = await response2.Content.ReadAsStringAsync();
            var apiResponse2 = JsonConvert.DeserializeObject<ApiResponse>(result2);


            if (apiResponse.Sucesso && apiResponse2.Sucesso)
            {
                var response3 = await client.GetAsync(_routes.rota_funcionario);

                var result3 = await response3.Content.ReadAsStringAsync();
                var Funcionarios = JsonConvert.DeserializeObject<List<Funcionario>>(result3);


                foreach (var funcionario in Funcionarios)
                {
                    int idusuario = funcionario.Id_Usuario;

                    var rotaUsuario = _routes.rota_usuario + idusuario.ToString();
                    var response4 = await client.GetAsync(rotaUsuario);
                    var result4 = await response4.Content.ReadAsStringAsync();
                    var apiResponse3 = JsonConvert.DeserializeObject<ApiResponseListaUsuario>(result4);
                    if (apiResponse3.Usuario != null)
                    {
                        Usuario usuario = apiResponse3.Usuario;
                        funcionario.Usuario = usuario;
                    }
                }

                var model2 = new UsuarioFuncionarioModel
                {
                    Funcionarios = Funcionarios
                };

                //Combobox Empresas
                var response5 = await client.GetAsync(_routes.rota_empresa);
                var result5 = await response5.Content.ReadAsStringAsync();
                var Empresas = JsonConvert.DeserializeObject<List<Empresa>>(result5);
                ViewBag.Empresas = Empresas;

                //Combobox Cargos
                var response6 = await client.GetAsync(_routes.rota_cargo);
                var result6 = await response6.Content.ReadAsStringAsync();
                var Cargos = JsonConvert.DeserializeObject<List<Cargo>>(result6);
                ViewBag.Cargos = Cargos;

                //Combobox Escalas
                var response7 = await client.GetAsync(_routes.rota_escala);
                var result7 = await response7.Content.ReadAsStringAsync();
                var Escalas = JsonConvert.DeserializeObject<List<Escala>>(result7);
                ViewBag.Escalas = Escalas;

                ViewData["Title"] = "Cadastro de Funcionario";

                ViewBag.Username = User.Identity.Name;
                return View("/Views/Cadastro/UsuarioFuncionario/UsuarioFuncionario.cshtml", model2);
            }
            else
            {
                ViewBag.ErrorMessage = apiResponse.Mensagem;
                return View("/Views/Cadastro/UsuarioFuncionario/UsuarioFuncionario.cshtml");
            }
        }


        public class ApiResponse
        {
            public Funcionario Funcionario { get; set; }
            public string Mensagem { get; set; }
            public bool Sucesso { get; set; }
        }

        public class ApiResponseUsu
        {
            public Usuario Usuario { get; set; }
            public string Mensagem { get; set; }
            public bool Sucesso { get; set; }
        }

        public class ApiResponseLista
        {
            public List<Funcionario> Funcionarios { get; set; }
            public string Mensagem { get; set; }
            public bool Sucesso { get; set; }
        }

        public class ApiResponseListaUsuario
        {
            public Usuario Usuario { get; set; }
            public string Mensagem { get; set; }
            public bool Sucesso { get; set; }
        }
    }
}
