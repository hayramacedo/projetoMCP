﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using Ponto.Models;
using Ponto.Views.Cadastro.Cargo;
using Ponto.Views.Folha;
using Ponto.Views.Home;
using System.Net.Http.Headers;
using System.Text;

namespace Ponto.Controllers
{
    public class FolhaController : Controller
    {
        private static readonly HttpClient client = new HttpClient();

        private readonly Routes _routes;
        public FolhaController(Routes routes)
        {
            _routes = routes;
        }
        public async Task<IActionResult> Folha(string username)
        {
            if (User.Identity.IsAuthenticated)
            {
                var response = await client.GetAsync(_routes.rota_usuario_nickname + User.Identity.Name);
                var result = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<ApiResponseUsu>(result);

                var response2 = await client.GetAsync(_routes.rota_funcionariobyUsu + apiResponse.Usuario.Id);
                var result2 = await response2.Content.ReadAsStringAsync();
                var apiResponse2 = JsonConvert.DeserializeObject<ApiResponseFunc>(result2);


                // Calculo data de início e fim do mês atual
                DateTime now = DateTime.Now;
                DateTime firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
                DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                ViewBag.DataInicio = firstDayOfMonth.ToString("yyyy-MM-dd");
                ViewBag.DataFim = lastDayOfMonth.ToString("yyyy-MM-dd");


                var rota = _routes.rota_folha + "0/"+ apiResponse2.Funcionarios[0].Id.ToString() + "/"+ firstDayOfMonth.ToString("yyyy-MM-dd") + "/"+ lastDayOfMonth.ToString("yyyy-MM-dd");

                ViewBag.Id_Funcionario = apiResponse2.Funcionarios[0].Id;

                PesquisarSaldoBanco(apiResponse2.Funcionarios[0].Id);

                var response3 = await client.GetAsync(rota);
                var result3 = await response3.Content.ReadAsStringAsync();
                var apiResponse3 = JsonConvert.DeserializeObject<ApiResponseLista>(result3);


                if (apiResponse3.Sucesso == false || apiResponse3.Folhas == null)
                {
                    ViewBag.ErrorMessage = "Registro não encontrado ou ID inválido.";
                    return View("/Views/Home/Folha.cshtml", new FolhaModel { Folhas = new List<Folha>() });
                }

                var response4 = await client.GetAsync(_routes.rota_solicitacao + "0/" + apiResponse2.Funcionarios[0].Id.ToString() + "/0");
                var result4 = await response4.Content.ReadAsStringAsync();
                var Solicitacoes = JsonConvert.DeserializeObject<ListaSolicitacaoAjusteResponse>(result4);

                var model = new FolhaModel
                {
                    Folhas = apiResponse3.Folhas ?? new List<Folha>(), 
                    Solicitacoes = Solicitacoes.Solicitacoes ?? new List<SolicitacaoAjuste>()
                };
               
                ViewData["Title"] = "Folha de Ponto";

                ViewBag.Username = User.Identity.Name;
                return View("/Views/Folha/Folha.cshtml", model);
            }
            return View("/Views/Folha/Folha.cshtml");
        }

        [HttpGet]
        [Route("api/Folha/Pesquisar/{id?}/{id_funcionario?}/{data_inicio?}/{data_fim?}")]
        public async Task<IActionResult> PesquisarFolha(int? id = 0, int id_funcionario = 0, string data_inicio = "", string data_fim = "")
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
                    ViewBag.ErrorMessage = "Registro não encontrado ou ID inválido.";
                    return View("/Views/Home/Folha.cshtml", new FolhaModel { Folhas = new List<Folha>() });
                }

                var response3 = await client.GetAsync(_routes.rota_solicitacao + "0/" + id_funcionario.ToString() + "/0");
                var result3 = await response3.Content.ReadAsStringAsync();
                var Solicitacoes = JsonConvert.DeserializeObject<ListaSolicitacaoAjusteResponse>(result3);

                var model = new FolhaModel
                {
                    Folhas = response2.Folhas ?? new List<Folha>(), 
                    Solicitacoes = Solicitacoes.Solicitacoes ?? new List<SolicitacaoAjuste>()
                };

                ViewData["Title"] = "Folha de Ponto";

                // Calculo data de início e fim do mês atual
                DateTime now = DateTime.Now;
                DateTime firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
                DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                if (data_inicio != null)
                    ViewBag.DataInicio = data_inicio;
                else ViewBag.DataInicio = firstDayOfMonth.ToString("yyyy-MM-dd");

                if (data_fim != null) 
                    ViewBag.DataFim = data_fim;
                else ViewBag.DataFim = lastDayOfMonth.ToString("yyyy-MM-dd");

                PesquisarSaldoBanco(id_funcionario);
                await PesquisarFolhaAjuste(0, id_funcionario, 0);

                ViewBag.Id_Funcionario = id_funcionario;
                ViewBag.Username = User.Identity.Name;
                return View("/Views/Folha/Folha.cshtml", model);
            }
            return View("/Views/Folha/Folha.cshtml");
        }

        [HttpGet]
        [Route("api/FolhaAjuste/Pesquisar/{id?}/{id_funcionario?}/{id_folha?}")]
        //public async Task<List<SolicitacaoAjuste>> PesquisarFolhaAjuste(int? id = 0, int id_funcionario = 0, int id_folha = 0)
        public async Task<IActionResult> PesquisarFolhaAjuste(int? id = 0, int id_funcionario = 0, int id_folha = 0)
        {
            var rota = _routes.rota_solicitacao + id.ToString() + "/" + id_funcionario.ToString() + "/" + id_folha.ToString();

            var response = await client.GetAsync(rota);

            var result = await response.Content.ReadAsStringAsync();
            var response2 = JsonConvert.DeserializeObject<ListaSolicitacaoAjusteResponse>(result);

            if (response2.Sucesso == false || response2.Solicitacoes == null)
            {
                //ViewBag.ErrorMessage = "Registro não encontrado ou ID inválido.";
                return null;
            }

            //var model = new FolhaModel
            //{
            //    Solicitacoes = response2.Solicitacoes ?? new List<SolicitacaoAjuste>()
            //};

            //return response2.Solicitacoes;
            if (response2.Sucesso == false || response2.Solicitacoes == null)
            {
                return Json(new { sucesso = false, mensagem = "Registro não encontrado ou ID inválido." });
            }

            return Json(new { sucesso = true, solicitacoes = response2.Solicitacoes });
        }


        [HttpGet]
        [Route("api/Folha/Pesquisar2/{id?}/{id_funcionario?}/{data_inicio?}/{data_fim?}")]
        public async Task<List<Folha>> PesquisarFolha2(int? id = 0, int id_funcionario = 0, string data_inicio = "", string data_fim = "")
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
                    ViewBag.ErrorMessage = "Registro não encontrado ou ID inválido.";
                    return null;
                }

                var resultado = response2.Folhas;

            return resultado;
        }


        [HttpPost]
        [Route("api/FolhaAjuste/Create")]
        //public async Task<IActionResult> Create([FromBody] SolicitacaoAjuste model)
        public async Task<IActionResult> Create([FromForm] SolicitacaoAjuste model, [FromForm] byte[] documento, [FromForm] string nm_documento)
        {
            var responseID = await client.GetAsync(_routes.rota_solicitacao_proximoID);
            var resultID = await responseID.Content.ReadAsStringAsync();
            var apiResponseID = JsonConvert.DeserializeObject<int>(resultID);

            model.Id = apiResponseID;

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(_routes.rota_solicitacao, content);
            var result = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(result);

            if (documento != null && documento.Length > 0)
            {
                Atestado atestado = new Atestado
                {
                    Documento = documento,
                    Id_Funcionario = model.Id_Funcionario,
                    Id_Folha = model.Id_Folha,         
                    Nm_Documento = nm_documento,
                    Id_Solicitacao = apiResponseID
                };

                var json2 = JsonConvert.SerializeObject(atestado);
                var content2 = new StringContent(json2, Encoding.UTF8, "application/json");

                var response3 = await client.PostAsync(_routes.rota_atestado, content2);
                var result3 = await response3.Content.ReadAsStringAsync();
                var apiResponse3 = JsonConvert.DeserializeObject<ApiResponse>(result3);
            }
            if (apiResponse.Sucesso)
            {
                var response2 = await client.GetAsync(_routes.rota_solicitacao);
                var result2 = await response2.Content.ReadAsStringAsync();
                var Solicitacoes = JsonConvert.DeserializeObject<ListaSolicitacaoAjusteResponse>(result2);

                var model2 = new FolhaModel
                {
                    Solicitacoes = Solicitacoes.Solicitacoes ?? new List<SolicitacaoAjuste>()
                };

                ViewBag.Username = User.Identity.Name;
                return View("/Views/Folha/Folha.cshtml", model2);
            }
            else
            {
                ViewBag.ErrorMessage = apiResponse.Mensagem;
                return BadRequest("Houve um erro: " + apiResponse.Mensagem);
            }
        }

        [HttpGet]
        [Route("api/Folha/Atestado/{id_solicitacao}")]
        public async Task<string> PesquisarAtestado(int id_solicitacao = 0)
        {
            var rota = _routes.rota_atestado + id_solicitacao.ToString();
            var response = await client.GetAsync(rota);

            var result = await response.Content.ReadAsStringAsync();
            var response2 = JsonConvert.DeserializeObject<AtestadoResponse>(result);

            if (response2.Sucesso == false)
            {
                ViewBag.ErrorMessage = "Ocorreu um erro: " + response2.Mensagem;
                ViewBag.BancoSaldo = "";
            }

            var nm_documento = "";
            if (response2.Atestado != null)
            {
                nm_documento = response2.Atestado.Nm_Documento;
            }

            return nm_documento;
        }

        [HttpGet]
        [Route("api/Folha/BancoHoras_Saldo/{id_funcionario}")]
        public async void PesquisarSaldoBanco(int id_funcionario = 0)
        {
            var rota = _routes.rota_SaldoBancoHoras + id_funcionario.ToString();
            var response = await client.GetAsync(rota);

            var result = await response.Content.ReadAsStringAsync();
            var response2 = JsonConvert.DeserializeObject<SaldoBancoH>(result);

            if (response2.Sucesso == false)
            {
                ViewBag.ErrorMessage = "Ocorreu um erro: " + response2.Mensagem;
                ViewBag.BancoSaldo = "";
            }
            ViewBag.BancoSaldo = ConvertMinutesToHHMM(response2.Saldo);
        }

        [HttpGet]
        public async Task<IActionResult> EspelhoPonto(DateTime dt_inic, DateTime dt_fim)
        {
            if (User.Identity.IsAuthenticated)
            {
                var response = await client.GetAsync(_routes.rota_usuario_nickname + User.Identity.Name);
                var result = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<ApiResponseUsu>(result);

                var response2 = await client.GetAsync(_routes.rota_funcionariobyUsu + apiResponse.Usuario.Id);
                var result2 = await response2.Content.ReadAsStringAsync();
                var apiResponse2 = JsonConvert.DeserializeObject<ApiResponseFunc>(result2);

                // Calculo data de início e fim do mês atual
                ViewBag.DataInicio = dt_inic.ToString("dd/MM/yyyy");
                ViewBag.DataFim = dt_fim.ToString("dd/MM/yyyy");

                var rota = _routes.rota_folha + "0/" + apiResponse2.Funcionarios[0].Id.ToString() + "/" + dt_inic.ToString("yyyy-MM-dd") + "/" + dt_fim.ToString("yyyy-MM-dd");

                ViewBag.Id_Funcionario = apiResponse2.Funcionarios[0].Id;

                PesquisarSaldoBanco(apiResponse2.Funcionarios[0].Id);

                var response3 = await client.GetAsync(rota);
                var result3 = await response3.Content.ReadAsStringAsync();
                var apiResponse3 = JsonConvert.DeserializeObject<ApiResponseLista>(result3);


                if (apiResponse3.Sucesso == false || apiResponse3.Folhas == null)
                {
                    ViewBag.ErrorMessage = "Registro não encontrado ou ID inválido.";
                    return View("/Views/Home/Folha.cshtml", new FolhaModel { Folhas = new List<Folha>() });
                }

                var response4 = await client.GetAsync(_routes.rota_empresa + apiResponse2.Funcionarios[0].Id_Empresa);
                var result4 = await response4.Content.ReadAsStringAsync();
                var Empresas = JsonConvert.DeserializeObject<ListaEmpresaResponse>(result4);

                var response5 = await client.GetAsync(_routes.rota_cargo + apiResponse2.Funcionarios[0].Id_Cargo);
                var result5 = await response5.Content.ReadAsStringAsync();
                var Cargo = JsonConvert.DeserializeObject<ListaCargoResponse>(result5);

                apiResponse2.Funcionarios[0].Cargo = Cargo.Cargos[0].Descricao;

                var model = new RelEspelhoPontoModel
                {
                    Folhas = apiResponse3.Folhas ?? new List<Folha>(), 
                    Empresa = Empresas.Empresas[0],
                    Funcionario = apiResponse2.Funcionarios[0]
                };

                ViewData["Title"] = "Espelho de Ponto";

                ViewBag.Username = User.Identity.Name;

                return View("/Views/Folha/RelEspelhoPonto.cshtml", model);
            }
            return View("/Views/Folha/RelEspelhoPonto.cshtml");
        }


        public string ConvertMinutesToHHMM(int minutes)
        {
            bool isNegative = minutes < 0;
            int absMinutes = Math.Abs(minutes);

            int hours = absMinutes / 60;
            int mins = absMinutes % 60;

            string hhmm = (isNegative ? "-" : "") +
                          hours.ToString("D2") + ":" +
                          mins.ToString("D2");

            return hhmm;
        }

        public class ApiResponse
        {
            public string Mensagem { get; set; }
            public bool Sucesso { get; set; }
        }

        public class AtestadoResponse
        {
            public Atestado Atestado { get; set; }
            public string Mensagem { get; set; }
            public bool Sucesso { get; set; }
        }

        public class ApiResponseLista
        {
            public List<Folha> Folhas { get; set; }
            public string Mensagem { get; set; }
            public bool Sucesso { get; set; }
        }

        public class ApiResponseUsu
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

        public class ListaSolicitacaoAjusteResponse
        {
            public List<SolicitacaoAjuste> Solicitacoes { get; set; }
            public string Mensagem { get; set; }
            public bool Sucesso { get; set; }
        }
        public class SaldoBancoH
        {
            public int Saldo { get; set; }
            public string Mensagem { get; set; }
            public bool Sucesso { get; set; }
        }

        public class ListaEmpresaResponse
        {
            public List<Empresa> Empresas { get; set; }
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
