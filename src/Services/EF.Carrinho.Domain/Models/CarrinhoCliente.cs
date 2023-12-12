using EF.Domain.Commons.DomainObjects;

namespace EF.Carrinho.Domain.Models;

public class CarrinhoCliente : Entity, IAggregateRoot
{
    public CarrinhoCliente()
    {
        _itens = new List<Item>();
    }

    public CarrinhoCliente(Guid clienteId)
    {
        if (!ValidarCliente(clienteId)) throw new DomainException("Cliente inválido");
        
        _itens = new List<Item>();
        ClienteId = clienteId;
    }
    
    public Guid? ClienteId { get; private set; }
    
    public string Nome { get; private set; }
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
        _itens.Add(item);
        AtualizarValorTotal();
    }
    
    public void RemoverItem(Item item)
    {
        _itens.Remove(item);
        AtualizarValorTotal();
    }

    public decimal AtualizarValorTotal()
    {
        ValorTotal = Itens.Sum(i => i.ValorUnitario * i.Quantidade);
        return ValorTotal;
    }
    
    public bool ValidarCliente(Guid clienteId)
    {
        if(clienteId == Guid.Empty) return false;

        return true;
    }
}