using EF.Carrinho.Application.DTOs;
using EF.Carrinho.Application.Services.Interfaces;
using EF.Carrinho.Domain.Models;
using EF.Carrinho.Domain.Repository;
using EF.Domain.Commons.Communication;
using EF.WebApi.Commons.Users;

namespace EF.Carrinho.Application.Services;

public class CarrinhoAppService(ICarrinhoRepository carrinhoRepository, IUserApp user) : ICarrinhoAppService
{
    private readonly Guid _clienteId = user.GetUserId();
    private readonly Guid _carrinhoId = user.ObterCarrinhoId();

    public async Task<CarrinhoCliente?> ObterCarrinhoCliente()
    {
        if (_clienteId != Guid.Empty)
        {
            return await carrinhoRepository.ObterPorCliente(_clienteId);
        }

        return await carrinhoRepository.ObterPorId(_carrinhoId);
    }

    public async Task LimparCarrinho()
    {
        var carrinho = await ObterCarrinhoCliente();

        if (carrinho is not null)
        {
            carrinho.LimparCarrinho();
            carrinhoRepository.Atualizar(carrinho);
            await PersistirDados();
        }
    }

    public async Task AdicionarItemCarrinho(AdicionarItemDto itemDto)
    {
        // TODO: Obter produto da API de produtos. (Catálogo?) e validar estoque
        var carrinho = await ObterCarrinhoCliente();

        if (carrinho is null)
        {
            carrinho = AdicionarItemCarrinhoNovo(itemDto);
            carrinhoRepository.Criar(carrinho);
        }
        else
        {
            AdicionarItemCarrinhoExistente(carrinho, itemDto);
            carrinhoRepository.Atualizar(carrinho);
        }
        
        await PersistirDados();
    }
    
    public async Task<Result<Item>> AtualizarItem(Item item)
    {
        var carrinho = await ObterCarrinhoCliente();
        
        if (carrinho is null)
        {
            return Result<Item>.Failure("Carrinho não encontrado");
        }
        
        var itemCarrinho = carrinho.Itens.FirstOrDefault(f => f.Id == item.Id);
        
        if (itemCarrinho is null)
        {
            return Result<Item>.Failure("Item não encontrado");
        }
        
        itemCarrinho.AtualizarQuantidade(item.Quantidade);
        
        carrinhoRepository.Atualizar(carrinho);
        await PersistirDados();
        
        return Result<Item>.Success();
    }

    public Task RemoverItemCarrinho(Item item)
    {
        throw new NotImplementedException();
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
        carrinho.AdicionarItem(new Item(itemDto.ProdutoId, 35.90m, itemDto.Quantidade));
        return carrinho;
    }

    private CarrinhoCliente AdicionarItemCarrinhoExistente(CarrinhoCliente carrinho, AdicionarItemDto itemDto)
    {
        carrinho.AdicionarItem(new Item(itemDto.ProdutoId, 35.90m, itemDto.Quantidade));
        return carrinho;
    }
    
    private async Task PersistirDados()
    {
        await carrinhoRepository.UnitOfWork.Commit();
    }
}