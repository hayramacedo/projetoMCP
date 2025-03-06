using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Ponto.Views.Cadastro.Empresa
{
    public class EmpresaModel : PageModel
    {
        public List<Models.Empresa> Empresas { get; set; }
        public Models.Empresa Empresa { get; set; }
        public void OnGet()
        {
        }
    }
}
