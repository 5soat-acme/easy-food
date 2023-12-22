using EF.Domain.Commons.DomainObjects;

namespace EF.Carrinho.Domain.Models;

public class CarrinhoCliente : Entity, IAggregateRoot
{
    // Necessário para o EF
    protected CarrinhoCliente()
    {
    }

    public CarrinhoCliente(Guid id)
    {
        _itens = new List<Item>();
        Id = id;
    }

    public Guid? ClienteId { get; private set; }
    public decimal ValorTotal { get; private set; }
    private readonly List<Item> _itens;
    public IReadOnlyCollection<Item> Itens => _itens;

    public void AssociarCliente(Guid clienteId)
    {
        if (!ValidarCliente(clienteId)) throw new DomainException("Cliente inválido");
        ClienteId = clienteId;
    }

    public void AssociarCarrinho(Guid id)
    {
        if (id == Guid.Empty) throw new DomainException("Id do carrinho inválido");

        Id = id;
    }

    public void AdicionarItem(Item item)
    {
        item.AssociarCarrinho(Id);
        var itemExistente = ObterItemPorProdutoId(item.ProdutoId);

        if (itemExistente is not null)
        {
            itemExistente.AtualizarQuantidade(itemExistente.Quantidade + item.Quantidade);
            item = itemExistente;
            _itens.Remove(itemExistente);
        }
        
        _itens.Add(item);
        AtualizarValorTotal();
    }

    public bool ProdutoExiste(Guid produtoId)
    {
        return _itens.Any(p => p.ProdutoId == produtoId);
    }
    
    public Item? ObterItemPorProdutoId(Guid produtoId)
    {
        return _itens.FirstOrDefault(p => p.ProdutoId == produtoId);
    }

    public void RemoverItem(Item item)
    {
        _itens.Remove(item);
        AtualizarValorTotal();
    }

    public void LimparCarrinho()
    {
        _itens.Clear();
        AtualizarValorTotal();
    }

    public decimal AtualizarValorTotal()
    {
        ValorTotal = Itens.Sum(i => i.ValorUnitario * i.Quantidade);
        return ValorTotal;
    }

    public bool ValidarCliente(Guid clienteId)
    {
        if (clienteId == Guid.Empty) return false;

        return true;
    }

    public void AtualizarQuantidadeItem(Guid itemId, int quantidade)
    {
        var item = _itens.FirstOrDefault(f => f.Id == itemId);

        if (item is null)
        {
            throw new DomainException("Item não econtrado");
        }

        item.AtualizarQuantidade(quantidade);

        AtualizarValorTotal();
    }
}