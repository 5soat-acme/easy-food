using EF.Carrinho.Application.DTOs;
using EF.Carrinho.Application.DTOs.Requests;
using EF.Carrinho.Application.DTOs.Responses;
using EF.Carrinho.Domain.Models;
using EF.Domain.Commons.Communication;

namespace EF.Carrinho.Application.Services.Interfaces;

public interface ICarrinhoService
{
    Task<CarrinhoClienteDto?> ObterCarrinhoCliente();
    Task<OperationResult> LimparCarrinho();
    Task<OperationResult<Item>> AdicionarItemCarrinho(AdicionarItemDto itemDto);
    Task<OperationResult> AtualizarItem(AtualizarItemDto itemDto);
    Task<OperationResult> RemoverItemCarrinho(Guid itemId);
    Task<ResumoCarrinhoDto> ObterResumo();
    Task<OperationResult<CarrinhoFechadoRespostaDto>> FecharPedido();
}