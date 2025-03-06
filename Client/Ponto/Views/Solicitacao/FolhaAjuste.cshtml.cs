using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Ponto.Views.Solicitacao
{ 
    public class FolhaAjusteModel : PageModel
    {
        public List<Models.Folha> FolhasAjuste { get; set; } = new List<Models.Folha>();
        public Models.Folha FolhaAjuste { get; set; }
        public void OnGet()
        {
        }
    }
}
