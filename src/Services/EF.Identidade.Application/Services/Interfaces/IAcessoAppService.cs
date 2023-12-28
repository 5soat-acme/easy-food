using EF.Domain.Commons.Communication;
using EF.Identidade.Application.DTOs;
using EF.Identidade.Application.DTOs.Requests;
using EF.Identidade.Application.DTOs.Responses;

namespace EF.Identidade.Application.Services.Interfaces;

public interface IAcessoAppService
{
    Task<OperationResult<RespostaTokenAcesso>> CriarUsuario(NovoUsuario novoUsuario);
    RespostaTokenAcesso GerarTokenAcessoNaoIdentificado(string? cpf = null);
    Task<OperationResult<RespostaTokenAcesso>> Autenticar(UsuarioLogin usuario);
}