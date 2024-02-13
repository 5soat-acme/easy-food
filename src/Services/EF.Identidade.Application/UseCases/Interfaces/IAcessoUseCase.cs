using EF.Core.Commons.Communication;
using EF.Identidade.Application.DTOs.Requests;
using EF.Identidade.Application.DTOs.Responses;

namespace EF.Identidade.Application.UseCases.Interfaces;

public interface IAcessoUseCase
{
    Task<OperationResult<RespostaTokenAcesso>> CriarUsuario(NovoUsuario novoUsuario);
    Task<OperationResult<RespostaTokenAcesso>> Identificar(UsuarioAcesso usuario);
}