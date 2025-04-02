using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Ponto.Views.Folha
{
    public class RelEspelhoPontoModel : PageModel
    {
        public List<Models.Folha> Folhas { get; set; } = new List<Models.Folha>();
        public Models.Folha Folha { get; set; }
        public Models.Empresa Empresa { get; set; }
        public Models.Funcionario Funcionario { get; set; }
        public void OnGet()
        {
        }
    }
}
