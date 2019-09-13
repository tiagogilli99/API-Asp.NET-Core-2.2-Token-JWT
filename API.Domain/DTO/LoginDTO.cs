using System.ComponentModel.DataAnnotations;

namespace API.Domain.DTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Email é um campo obrigatório para o login")]
        [EmailAddress(ErrorMessage = "Email em um formato inválido")]
        [StringLength(60, ErrorMessage = "Email deve ter no máximo {1} caracteres.")]
        public string Email { get; set; }
    }
}
