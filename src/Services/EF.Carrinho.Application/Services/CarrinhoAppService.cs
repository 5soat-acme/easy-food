using AutoMapper;
using EF.Carrinho.Application.DTOs;
using EF.Carrinho.Application.Services.Interfaces;
using EF.Carrinho.Domain.Models;
using EF.Carrinho.Domain.Repository;
using EF.Domain.Commons.Communication;
using EF.WebApi.Commons.Users;

namespace EF.Carrinho.Application.Services;

public class CarrinhoAppService(ICarrinhoRepository carrinhoRepository, IUserApp user, IMapper mapper)
    : ICarrinhoAppService
{
    private readonly Guid _clienteId = user.GetUserId();
    private readonly Guid _carrinhoId = user.ObterCarrinhoId();

    public async Task<CarrinhoClienteDto?> ObterCarrinhoCliente()
    {
        var carrinho = await ObterCarrinho();
        
        if (carrinho is null)
        {
            carrinho = new CarrinhoCliente(user.ObterCarrinhoId());
            carrinho.AssociarCliente(user.GetUserId());
        }

        return mapper.Map<CarrinhoClienteDto>(carrinho);
    }

    public async Task<OperationResult> AdicionarItemCarrinho(AdicionarItemDto itemDto)
    {
        // TODO: Obter produto da API de produtos. (Catálogo?) e validar estoque
        var carrinho = await ObterCarrinho();

        if (carrinho is null)
        {
            carrinho = AdicionarItemCarrinhoNovo(itemDto);
            carrinhoRepository.Criar(carrinho);
        }
        else
        {
            carrinho = AdicionarItemCarrinhoExistente(carrinho, itemDto);
            carrinhoRepository.Atualizar(carrinho);
        }

        await PersistirDados();

        return OperationResult.Success();
    }

    public async Task<OperationResult> AtualizarItem(AtualizarItemDto itemDto)
    {
        // TODO: Obter produto da API de produtos. (Catálogo?) e validar estoque
        var carrinho = await ObterCarrinho();

        if (carrinho is null)
        {
            return OperationResult.Failure("Carrinho não encontrado");
        }

        var item = carrinho.Itens.FirstOrDefault(f => f.Id == itemDto.ItemId);

        if (item is null)
        {
            return OperationResult.Failure("Item não encontrado");
        }

        carrinho.AtualizarQuantidadeItem(itemDto.ItemId, itemDto.Quantidade);

        carrinhoRepository.Atualizar(carrinho);

        await PersistirDados();

        return OperationResult.Success();
    }

    public async Task<OperationResult> RemoverItemCarrinho(Guid itemId)
    {
        var carrinho = await ObterCarrinho();

        if (carrinho is null)
        {
            return OperationResult.Failure("Carrinho não encontrado");
        }

        var item = carrinho.Itens.FirstOrDefault(f => f.Id == itemId);

        if (item is null)
        {
            return OperationResult.Success();
        }

        carrinho.RemoverItem(item);
        carrinhoRepository.RemoverItem(item);
        
        if (!carrinho.Itens.Any())
        {
            carrinhoRepository.Remover(carrinho);
        }
        else
        {
            carrinhoRepository.Atualizar(carrinho);
        }

        await PersistirDados();

        return OperationResult.Success();
    }

    public async Task<OperationResult> LimparCarrinho()
    {var carrinho = await ObterCarrinho();

        if (carrinho is null)
        {
            return OperationResult.Failure("Carrinho não encontrado");
        }

        carrinho.LimparCarrinho();

        carrinho.Itens.ToList().ForEach(i => carrinhoRepository.RemoverItem(i));
        carrinhoRepository.Remover(carrinho);

        await PersistirDados();

        return OperationResult.Success();
    }

    public async Task GerarPedido()
    {
        var carrinho = await ObterCarrinho();
        
    }
    
    private async Task<CarrinhoCliente?> ObterCarrinho()
    {
        if (_clienteId != Guid.Empty)
        {
            return await carrinhoRepository.ObterPorCliente(_clienteId);
        }

        return await carrinhoRepository.ObterPorId(_carrinhoId);
    }

    private CarrinhoCliente CriarCarrinhoCliente()
    {
        var carrinho = new CarrinhoCliente(_carrinhoId);

        if (_clienteId != Guid.Empty)
        {
            carrinho.AssociarCliente(_clienteId);
        }

        return carrinho;
    }

    private CarrinhoCliente AdicionarItemCarrinhoNovo(AdicionarItemDto itemDto)
    {
        var carrinho = CriarCarrinhoCliente();
        carrinho.AdicionarItem(new Item(itemDto.ProdutoId, "Produto Teste", 35.90m, itemDto.Quantidade));
        return carrinho;
    }

    private CarrinhoCliente AdicionarItemCarrinhoExistente(CarrinhoCliente carrinho, AdicionarItemDto itemDto)
    {
        carrinho.AdicionarItem(new Item(itemDto.ProdutoId, "Produto Teste", 35.90m, itemDto.Quantidade));
        return carrinho;
    }

    private async Task PersistirDados()
    {
        await carrinhoRepository.UnitOfWork.Commit();
    }
}