using System.ComponentModel.DataAnnotations;

namespace EF.Identidade.Application.DTOs.Requests;

public class UsuarioLogin
{
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
    public string Email { get; set; }
}