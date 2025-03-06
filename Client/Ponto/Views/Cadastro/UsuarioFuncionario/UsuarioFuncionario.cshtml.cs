using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Ponto.Views.Cadastro.UsuarioFuncionario
{
    public class UsuarioFuncionarioModel : PageModel
    {
        public List<Models.Funcionario> Funcionarios { get; set; }
        public Models.Funcionario Funcionario { get; set; }

        public List<Models.Usuario> Usuarios { get; set; }
        public Models.Usuario Usuario { get; set; }
        public void OnGet()
        {
        }
    }
}
