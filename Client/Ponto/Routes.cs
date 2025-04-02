using static System.Net.WebRequestMethods;

namespace Ponto
{
    public class Routes
    {
        public IConfiguration Configuration { get; }
        public string EnderecoApi { get; }

        public Routes(IConfiguration configuration)
        {
            Configuration = configuration;
            EnderecoApi = Configuration["AppSettings:EnderecoApi"];
        }

        public string rota_login => $"{EnderecoApi}/login";
        public string rota_usuario_username => $"{EnderecoApi}/Usuarios/selecionar/";
        public string rota_usuario_nickname => $"{EnderecoApi}/Usuarios/selecionarNickname/";
        public string rota_departamento => $"{EnderecoApi}/Departamentos/";
        public string rota_departamentoAlt => $"{EnderecoApi}/Departamentos?id=";
        public string rota_cargo => $"{EnderecoApi}/Cargos/";
        public string rota_cargoAlt => $"{EnderecoApi}/Cargos?id=";
        public string rota_folha => $"{EnderecoApi}/Folha/";
        public string rota_folhaAlt => $"{EnderecoApi}/Folha?id=";
        public string rota_empresa => $"{EnderecoApi}/Empresas/";
        public string rota_empresaAlt => $"{EnderecoApi}/Empresas?id=";
        public string rota_funcionario => $"{EnderecoApi}/Funcionarios/";
        public string rota_funcionarioAlt => $"{EnderecoApi}/Funcionarios?id=";
        public string rota_usuario => $"{EnderecoApi}/Usuarios/";
        public string rota_usuarioAlt => $"{EnderecoApi}/Usuarios?id=";
        public string rota_escala => $"{EnderecoApi}/Escalas/";
        public string rota_escalaAlt => $"{EnderecoApi}/Escalas?id=";
        public string rota_funcionariobyUsu => $"{EnderecoApi}/FuncionariosxUsuarios/";
        public string rota_AtualizarFolha => $"{EnderecoApi}/AtualizarFolha/";
        public string rota_RegistroPonto => $"{EnderecoApi}/RegistroPonto/";
        public string rota_situacao => $"{EnderecoApi}/Situacoes/";
        public string rota_solicitacao_proximoID => $"{EnderecoApi}/SolicitacaoAjuste/ProximoID";
        public string rota_solicitacao => $"{EnderecoApi}/SolicitacaoAjuste/";
        public string rota_solicitacaoAlt => $"{EnderecoApi}/SolicitacaoAjuste?id=";
        public string rota_SaldoBancoHoras => $"{EnderecoApi}/BancoHoras_Saldo/";
        public string rota_atestado => $"{EnderecoApi}/Atestados/";
    }
}
