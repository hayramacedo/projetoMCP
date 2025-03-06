using Azure;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Ponto.Models;
using Ponto.Views.Cadastro.Cargo;
using Ponto.Views.Cadastro.Departamento;
using Ponto.Views.Home;
using Ponto.Views.Solicitacao;
using System.Text;

namespace Ponto.Controllers
{
    public class SolicitacaoController : Controller
    {
        private static readonly HttpClient client = new HttpClient();

        private readonly Routes _routes;
        public SolicitacaoController(Routes routes)
        {
            _routes = routes;
        }
        public async Task<IActionResult> SolicitacaoAJuste()
        {
            var response = await client.GetAsync(_routes.rota_solicitacao + "/0/0/0");

            var result = await response.Content.ReadAsStringAsync();
            var Solicitacoes = JsonConvert.DeserializeObject<ListaSolicitacaoAjusteResponse>(result);

            if (Solicitacoes.Sucesso == false || Solicitacoes.Solicitacoes == null)
            {
                var model2 = new SolicitacaoAjusteModel
                {
                    SolicitacoesAjuste = new List<SolicitacaoAjuste>()
                };

                ViewBag.ErrorMessage = "Registro não encontrado ou ID inválido.";
                return View("/Views/Solicitacao/SolicitacaoAjuste.cshtml", model2);
            }

            var model = new SolicitacaoAjusteModel
            {
                SolicitacoesAjuste = Solicitacoes.Solicitacoes
            };

            ViewData["Title"] = "Solicitações de Ajuste";
            ViewBag.Username = User.Identity.Name;
            return View("/Views/Solicitacao/SolicitacaoAjuste.cshtml", model);
        }

        public async Task<IActionResult> FolhaAjuste()
        {
            // Calculo data de início e fim do mês atual
            DateTime now = DateTime.Now;
            DateTime firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            ViewBag.DataInicio = firstDayOfMonth.ToString("yyyy-MM-dd");
            ViewBag.DataFim = lastDayOfMonth.ToString("yyyy-MM-dd");

            ViewData["Title"] = "Ajuste de Folha de Ponto";
            ViewBag.Username = User.Identity.Name;

            var response3 = await client.GetAsync(_routes.rota_situacao);
            var result3 = await response3.Content.ReadAsStringAsync();
            var Situacoes = JsonConvert.DeserializeObject<List<Situacao_Jornada>>(result3);

            ViewBag.Situacoes = Situacoes;
            return View("/Views/Solicitacao/FolhaAjuste.cshtml");
        }

        [HttpGet]
        [Route("api/FolhaAjuste/PesquisarFolha/{id?}/{id_funcionario?}/{data_inicio?}/{data_fim?}")]
        public async Task<List<Folha>> PesquisarFolha(int? id = 0, int id_funcionario = 0, string data_inicio = "", string data_fim = "")
        {
            if (User.Identity.IsAuthenticated)
            {
                var rota = _routes.rota_folha + id.ToString() + "/" + id_funcionario.ToString();

                if (data_inicio != "")
                    rota += "/" + data_inicio;
                else rota += "/null";

                if (data_fim != "")
                    rota += "/" + data_fim;
                else rota += "/null";

                var response = await client.GetAsync(rota);

                var result = await response.Content.ReadAsStringAsync();
                var response2 = JsonConvert.DeserializeObject<ApiResponseLista>(result);


                if (response2.Sucesso == false || response2.Folhas == null)
                {
                    ViewBag.Situacoes = new List<Situacao_Jornada>();
                    ViewBag.ErrorMessage = "Registro não encontrado ou ID inválido.";
                    return  new List<Folha>();
                }

                var folhas = response2.Folhas ?? new List<Folha>(); 

                ViewData["Title"] = "Ajuste de Folha de Ponto";

                // Calculo data de início e fim do mês atual
                DateTime now = DateTime.Now;
                DateTime firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
                DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                ViewBag.DataInicio = firstDayOfMonth.ToString("yyyy-MM-dd");
                ViewBag.DataFim = lastDayOfMonth.ToString("yyyy-MM-dd");

                var response3 = await client.GetAsync(_routes.rota_situacao);
                var result3 = await response3.Content.ReadAsStringAsync();
                var Situacoes = JsonConvert.DeserializeObject<List<Situacao_Jornada>>(result3);

                ViewBag.Situacoes = Situacoes;
                ViewBag.Username = User.Identity.Name;
                return folhas;
            }
            return new List<Folha>();
        }

        [HttpPut]
        [Route("api/FolhaAjuste/Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Folha model)
        {
            if (model == null || model.Id != id)
            {
                return BadRequest("Dados do cargo inválidos.");
            }

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync(_routes.rota_folha + id.ToString(), content);
            var result = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(result);

            if (apiResponse.Sucesso)
            {
                var response3 = await client.GetAsync(_routes.rota_situacao);
                var result3 = await response3.Content.ReadAsStringAsync();
                var Situacoes = JsonConvert.DeserializeObject<List<Situacao_Jornada>>(result3);

                ViewBag.Situacoes = Situacoes;
                ViewData["Title"] = "Ajuste de Folha de Ponto";
                ViewBag.Username = User.Identity.Name;
                return Ok(apiResponse);
            }
            else
            {
                ViewBag.Situacoes = new List<Situacao_Jornada>();
                ViewBag.ErrorMessage = apiResponse.Mensagem;
                ViewBag.Username = User.Identity.Name;
                return BadRequest("Erro");
            }
        }

        [HttpGet]
        [Route("api/Solicitacoes/Pesquisar/{id?}/{id_funcionario?}/{id_folha?}")]
        public async Task<IActionResult> SolicitacoesPesquisar(int id = 0, int id_funcionario = 0, int id_folha = 0)
        {
            if (User.Identity.IsAuthenticated)
            {
                var response = await client.GetAsync(_routes.rota_solicitacao + "/" + id.ToString() + "/" + id_funcionario.ToString() + "/" + id_folha.ToString());

                var result = await response.Content.ReadAsStringAsync();
                var Solicitacoes = JsonConvert.DeserializeObject<ListaSolicitacaoAjusteResponse>(result);

                if (Solicitacoes.Sucesso == false || Solicitacoes.Solicitacoes == null)
                {
                    var model2 = new SolicitacaoAjusteModel
                    {
                        SolicitacoesAjuste = new List<SolicitacaoAjuste>()
                    };

                    ViewBag.ErrorMessage = "Registro não encontrado ou ID inválido.";
                    return View("/Views/Solicitacao/SolicitacaoAjuste.cshtml", model2);
                }


                var model = new SolicitacaoAjusteModel
                {
                    SolicitacoesAjuste = Solicitacoes.Solicitacoes
                };

                ViewData["Title"] = "Solicitações de Ajuste";

                ViewBag.Username = User.Identity.Name;
                return View("/Views/Solicitacao/SolicitacaoAjuste.cshtml", model);
            }
            return View("/Views/Solicitacao/SolicitacaoAjuste.cshtml");
        }

        [HttpPut]
        [Route("api/Solicitacoes/Update/{id}")]
        public async Task<IActionResult> SolicitacoesUpdate(int id, [FromBody] SolicitacaoAjuste model)
        {
            if (model == null || model.Id != id)
            {
                return BadRequest("Dados inválidos.");
            }

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync(_routes.rota_solicitacao + id.ToString(), content);
            var result = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(result);

            if (apiResponse.Sucesso)
            {                 
                ViewData["Title"] = "Solicitações de Ajuste";
                ViewBag.Username = User.Identity.Name;
                return Ok(apiResponse.Sucesso);
            }
            else
            {
                ViewBag.ErrorMessage = apiResponse.Mensagem;
                ViewBag.Username = User.Identity.Name;
                return BadRequest("Erro");
            }
        }
        public class ListaSolicitacaoAjusteResponse
        {
            public List<SolicitacaoAjuste> Solicitacoes { get; set; }
            public string Mensagem { get; set; }
            public bool Sucesso { get; set; }
        }

        public class ApiResponseLista
        {
            public List<Folha> Folhas { get; set; }
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
