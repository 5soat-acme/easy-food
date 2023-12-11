using System.ComponentModel.DataAnnotations;
using EF.Domain.Commons.ValueObjects;

namespace EF.Identidade.Application.DTOs;

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

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
    public string Senha { get; set; }

    [Compare("Senha", ErrorMessage = "As senhas não conferem.")]
    public string SenhaConfirmacao { get; set; }
}

public class CpfValidationAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is not string cpf) throw new ArgumentException("CPF deve ser uma string");

        if (!Cpf.Validar(cpf)) return new ValidationResult("CPF inválido");

        return ValidationResult.Success!;
    }
}