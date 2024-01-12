using EF.Carrinho.Application.DTOs.Requests;
using EF.Domain.Commons.Communication;

namespace EF.Carrinho.Application.Services.Interfaces;

public interface ICarrinhoManipulacaoService
{
    Task<OperationResult> AdicionarItemCarrinho(AdicionarItemDto itemDto, CarrinhoSessaoDto carrinhoSessao);
    Task<OperationResult> AtualizarItem(AtualizarItemDto itemDto, CarrinhoSessaoDto carrinhoSessao);
    Task<OperationResult> RemoverItemCarrinho(Guid itemId, CarrinhoSessaoDto carrinhoSessao);
    Task RemoverCarrinho(Guid carrinhoId);
    Task RemoverCarrinhoPorClienteId(Guid clienteId);
}