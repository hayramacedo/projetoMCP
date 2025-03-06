using System.ComponentModel.DataAnnotations;

namespace Ponto.Models
{
    public class Login
    {
        public required string Username { get; set; }

        public required string Password { get; set; }
    }
}
