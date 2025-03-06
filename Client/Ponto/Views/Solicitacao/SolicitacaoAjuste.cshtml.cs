using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Ponto.Views.Solicitacao
{
    public class SolicitacaoAjusteModel : PageModel
    {
        public List<Models.SolicitacaoAjuste> SolicitacoesAjuste { get; set; } = new List<Models.SolicitacaoAjuste>();
        public Models.SolicitacaoAjuste SolicitacaoAjuste { get; set; }
        public void OnGet()
        {
        }
    }
}
