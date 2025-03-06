using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Ponto.Views.Cadastro.Cargo
{
    public class CargoModel : PageModel
    {
        public List<Models.Cargo> Cargos { get; set; }
        public Models.Cargo Cargo { get; set; }
        public string Tab { get; set; }
        public void OnGet()
        {
        }
        public void OnPost()
        {
            // Lógica para salvar o departamento
        }

        public void OnPut(int? id)
        {
            // Lógica para atualizar o departamento
        }

        public void OnDelete(int? id)
        {
            // Lógica para deletar o departamento
        }
    }
}
