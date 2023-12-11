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
        if (!ValidarCliente()) throw new DomainException("Cliente inválido");
        
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
        ClienteId = clienteId;
    }
    
    public void AssociarCarrinho(Guid id)
    {
        Id = id;
    }
    
    public void AdicionarItem(Item item)
    {
        _itens.Add(item);
        AtualizarValorTotal();
    }

    public decimal AtualizarValorTotal()
    {
        ValorTotal = Itens.Sum(i => i.ValorUnitario * i.Quantidade);
        return ValorTotal;
    }
    
    public bool ValidarCliente()
    {
        if(ClienteId == Guid.Empty) return false;

        return true;
    }
}