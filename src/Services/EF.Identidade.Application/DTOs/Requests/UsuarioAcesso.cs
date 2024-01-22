using System.ComponentModel.DataAnnotations;

namespace EF.Identidade.Application.DTOs.Requests;

public class UsuarioAcesso
{
    [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
    public string? Email { get; set; }
    
    public string? Cpf { get; set; }
}