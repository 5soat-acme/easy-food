using EF.Carrinho.Application.Services.Interfaces;
using EF.Carrinho.Domain.Models;
using EF.Carrinho.Domain.Repository;
using EF.Domain.Commons.Communication;
using EF.WebApi.Commons.Users;
using FluentValidation.Results;

namespace EF.Carrinho.Application.Services;

public class CarrinhoAppService(ICarrinhoRepository carrinhoRepository, IUserApp user) : ICarrinhoAppService
{
    private readonly Guid _clienteId = user.GetUserId();
    private readonly Guid _carrinhoId = user.ObterCarrinhoId();
    private readonly List<string> _errors = new();

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
            PersistirDados();
        }
    }

    public async Task AdicionarItemCarrinho(Item item)
    {
        var carrinho = await ObterCarrinhoCliente() ?? CriarCarrinhoCliente();
        carrinho.AdicionarItem(item);
        carrinhoRepository.Atualizar(carrinho);
        PersistirDados();
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
        PersistirDados();
        
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
        
        carrinhoRepository.Criar(carrinho);

        return carrinho;
    }

    private void PersistirDados()
    {
        carrinhoRepository.UnitOfWork.Commit();
    }
}