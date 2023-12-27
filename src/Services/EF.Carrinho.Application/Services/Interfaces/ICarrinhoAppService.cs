using EF.Carrinho.Application.DTOs;
using EF.Domain.Commons.Communication;

namespace EF.Carrinho.Application.Services.Interfaces;

public interface ICarrinhoAppService
{
    Task<CarrinhoClienteDto?> ObterCarrinhoCliente();
    Task<OperationResult> LimparCarrinho();
    Task<OperationResult> AdicionarItemCarrinho(AdicionarItemDto itemDto);
    Task<OperationResult> AtualizarItem(AtualizarItemDto itemDto);
    Task<OperationResult> RemoverItemCarrinho(Guid itemId);
    Task<ResumoCarrinhoDto> ObterResumo();
    Task<OperationResult<CarrinhoFechadoRespostaDto>> FecharPedido();
}