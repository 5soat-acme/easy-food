using EF.Domain.Commons.Communication;
using EF.Identidade.Application.DTOs;

namespace EF.Identidade.Application.Services.Interfaces;

public interface IAcessoAppService
{
    Task<Result<RespostaTokenAcesso>> CriarUsuario(NovoUsuario novoUsuario);
    RespostaTokenAcesso GerarTokenAcessoNaoIdentificado(string? cpf = null);
    Task<Result<RespostaTokenAcesso>> Autenticar(UsuarioLogin usuario);
}