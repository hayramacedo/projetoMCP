using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Ponto.Views.Home
{
    public class FolhaModel : PageModel
    {
        public List<Models.Folha> Folhas { get; set; } = new List<Models.Folha>();
        public Models.Folha Folha { get; set; }

        public List<Models.SolicitacaoAjuste> Solicitacoes { get; set; } = new List<Models.SolicitacaoAjuste>();

        public void OnGet()
        {
        }
    }
}
