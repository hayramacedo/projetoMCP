using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using Ponto.Models;
using System.ComponentModel.DataAnnotations;

namespace Ponto.Views.Cadastro.Departamento
{
    public class DepartamentoModel : PageModel
    {
        public List<Models.Departamento> Departamentos { get; set; }
        public Models.Departamento Departamento { get; set; }
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
