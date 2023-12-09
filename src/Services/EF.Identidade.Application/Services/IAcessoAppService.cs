using EF.Identidade.Application.DTOs;
using FluentValidation.Results;

namespace EF.Identidade.Application.Services;

public interface IAcessoAppService
{
    Task<(ValidationResult ValidationResult, RespostaTokenAcesso? Token)> CriarUsuario(NovoUsuario novoUsuario);
    RespostaTokenAcesso GerarTokenAcessoNaoIdentificado(string? cpf = null);
    Task<(ValidationResult ValidationResult, RespostaTokenAcesso? Token)> Autenticar(UsuarioLogin usuario);
}