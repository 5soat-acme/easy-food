using EF.Clientes.Application.DTOs;
using EF.Core.Commons.Communication;

namespace EF.Clientes.Application.UseCases.Interfaces;

public interface ICriarClienteUseCase
{
    Task<OperationResult<Guid>> Handle(CriarClienteDto clienteDto);
}