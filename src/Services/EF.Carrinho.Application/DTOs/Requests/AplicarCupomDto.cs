using System.ComponentModel.DataAnnotations;

namespace EF.Carrinho.Application.DTOs.Requests;

public class AplicarCupomDto
{
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    public string Codigo { get; set; }
}