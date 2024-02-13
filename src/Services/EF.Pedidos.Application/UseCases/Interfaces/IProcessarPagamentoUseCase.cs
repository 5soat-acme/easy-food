using EF.Core.Commons.Communication;
using EF.Pedidos.Application.DTOs.Requests;

namespace EF.Pedidos.Application.UseCases.Interfaces;

public interface IProcessarPagamentoUseCase
{
    Task<OperationResult<Guid>> Handle(ProcessarPagamentoDto processarPagamentoDto);
}