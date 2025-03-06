using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Ponto.Models;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Text;
using Ponto.Views.Home;
using Ponto.Views.Cadastro.Departamento;

namespace Ponto.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private static readonly HttpClient client = new HttpClient();

        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        private readonly Routes _routes;

        public HomeController(ILogger<HomeController> logger, Routes routes)
        {
            _logger = logger;
            _routes = routes;
        }

        public async Task<IActionResult> Index(string username)
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.Username = User.Identity.Name;

                if (User.Identity.Name != "Administrador")
                {
                    var response = await client.GetAsync(_routes.rota_usuario_nickname + User.Identity.Name);
                    var result = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(result);

                    var response2 = await client.GetAsync(_routes.rota_funcionariobyUsu + apiResponse.Usuario.Id);
                    var result2 = await response2.Content.ReadAsStringAsync();
                    var apiResponse2 = JsonConvert.DeserializeObject<ApiResponseFunc>(result2);

                    await AtualizarFolha(apiResponse2.Funcionarios[0].Id);

                    string diaAtual = DateTime.Today.ToString("yyyy-MM-dd");
                    var responseFolha = await client.GetAsync(_routes.rota_folha + $"0/{apiResponse2.Funcionarios[0].Id}/{diaAtual}/{diaAtual}");
                    var result3 = await responseFolha.Content.ReadAsStringAsync();
                    var apiResponseFolha = JsonConvert.DeserializeObject<ListaFolhaResponse>(result3);

                    string entrada = apiResponseFolha.Folhas[0].Entrada.HasValue ? apiResponseFolha.Folhas[0].Entrada.Value.ToString("HH:mm:ss") : "00:00:00";
                    string pausa = apiResponseFolha.Folhas[0].Pausa.HasValue ? apiResponseFolha.Folhas[0].Pausa.Value.ToString("HH:mm:ss") : "00:00:00";
                    string retorno = apiResponseFolha.Folhas[0].Retorno.HasValue ? apiResponseFolha.Folhas[0].Retorno.Value.ToString("HH:mm:ss") : "00:00:00";
                    string saida = apiResponseFolha.Folhas[0].Saida.HasValue ? apiResponseFolha.Folhas[0].Saida.Value.ToString("HH:mm:ss") : "00:00:00";

                    ViewBag.Entrada = entrada;
                    ViewBag.Pausa = pausa;
                    ViewBag.Retorno = retorno;
                    ViewBag.Saida = saida;
              
                }
            }
            return View();
        }


        public IActionResult Login()
        {
            ViewBag.Username = null;
            return View();
        }

        [HttpGet]
        [Route("api/AtualizarFolha/{id_funcionario?}")]
        public async Task<ApiResponseFolha> AtualizarFolha(int id_funcionario = 0)
        {
            if (User.Identity.IsAuthenticated)
            {
                try
                { 
                    var rota = _routes.rota_AtualizarFolha + id_funcionario.ToString();

                    var response = await client.GetAsync(rota);

                    var result = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponseFolha>(result);

                    ViewBag.Username = User.Identity.Name;
                    return (new ApiResponseFolha { Mensagem = "", Sucesso = true });
                }
                catch (Exception ex)
                {
                    return (new ApiResponseFolha { Mensagem = "Ocorreu um erro: " + ex.Message, Sucesso = false });
                }
            }
            else
            {
                return (new ApiResponseFolha { Mensagem = "", Sucesso = false });
            }
        }

        [HttpPut]
        [Route("api/RegistrarPonto")]
        public async Task<IActionResult> RegistroPonto([FromBody] Folha folha)
        {
            try
            {
                if (folha == null)
                {
                    return BadRequest("Dados do registro inválidos.");
                }

                var response3 = await client.GetAsync(_routes.rota_usuario_nickname + "/" + User.Identity.Name);
                var result3 = await response3.Content.ReadAsStringAsync();
                var apiResponse3 = JsonConvert.DeserializeObject<ApiResponse>(result3);

                var response4 = await client.GetAsync(_routes.rota_funcionariobyUsu + "/" + apiResponse3.Usuario.Id);
                var result4 = await response4.Content.ReadAsStringAsync();
                var apiResponse4 = JsonConvert.DeserializeObject<ApiResponseFunc>(result4);
             
                string diaAtual = DateTime.Today.ToString("yyyy-MM-dd");
                var responseFolha = await client.GetAsync(_routes.rota_folha + $"0/{apiResponse4.Funcionarios[0].Id}/{diaAtual}/{diaAtual}");
                var result = await responseFolha.Content.ReadAsStringAsync();
                var apiResponseFolha = JsonConvert.DeserializeObject<ListaFolhaResponse>(result);

                if (apiResponseFolha != null)
                {
                    var id_folha = apiResponseFolha.Folhas[0].Id;

                    var json = JsonConvert.SerializeObject(folha);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PutAsync(_routes.rota_RegistroPonto + id_folha.ToString(), content);
                    var result2 = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponseFolha>(result2);

                    if (apiResponse.Sucesso)
                    {
                        ViewBag.Username = User.Identity.Name;
                        return Ok(RedirectToAction("Index", User.Identity.Name ));
                    }
                    else
                    {
                        return View("Index", User.Identity.Name);
                    }
                }
                else
                {
                    return BadRequest("Houve um erro ao Registrar o Ponto");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Houve um erro ao Registrar o Ponto: " + ex);
            }
        }


        [HttpPost]
        public async Task<IActionResult> LoginAsync(Login model)
        {
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(_routes.rota_login, content);
            var result = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(result);

            if (apiResponse.Sucesso)
            {
                var response2 = await client.GetAsync(_routes.rota_usuario_username + model.Username);
                var result2 = await response2.Content.ReadAsStringAsync();
                var apiResponse2 = JsonConvert.DeserializeObject<ApiResponse>(result2);

                var claims = new List<Claim>
                {
                   new Claim(ClaimTypes.Name, apiResponse2.Usuario.Nickname)
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", new { username = apiResponse2.Usuario.Nickname });
            }
            else
            {
                ViewBag.ErrorMessage = apiResponse.Mensagem;
                return View("Login");
            }

        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public class ApiResponse
        {
            public Usuario Usuario { get; set; }
            public string Mensagem { get; set; }
            public bool Sucesso { get; set; }
        }

        public class ApiResponseFunc
        {
            public List<Funcionario> Funcionarios { get; set; }
            public string Mensagem { get; set; }
            public bool Sucesso { get; set; }
        }

        public class ApiResponseFolha
        {
            public string Mensagem { get; set; }
            public bool Sucesso { get; set; }
        }

        public class ListaFolhaResponse
        {
            public List<Folha> Folhas { get; set; }
            public string Mensagem { get; set; }
            public bool Sucesso { get; set; }
        }
    }
}
