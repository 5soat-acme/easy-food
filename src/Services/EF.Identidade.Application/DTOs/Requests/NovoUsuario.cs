using System.ComponentModel.DataAnnotations;
using EF.WebApi.Commons.ModelStateValidations;

namespace EF.Identidade.Application.DTOs.Requests;

public class NovoUsuario
{
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    public string Sobrenome { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [CpfValidation]
    public string Cpf { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
    public string Email { get; set; }
}