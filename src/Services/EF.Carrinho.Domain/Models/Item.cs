using EF.Domain.Commons.DomainObjects;

namespace EF.Carrinho.Domain.Models;

public class Item : Entity
{
    // Necessário para o Entity Framework
    protected Item()
    {
    }

    public Item(Guid produtoId, decimal valorUnitario, int quantidade)
    {
        if (!ValidarProduto(produtoId)) throw new DomainException("Produto inválido");
        if (!ValidarValorUnitario(valorUnitario)) throw new DomainException("Valor unitário inválido");
        if (!ValidarQuantidade(quantidade)) throw new DomainException("Quantidade inválida");
        
        ProdutoId = produtoId;
        ValorUnitario = valorUnitario;
        Quantidade = quantidade;
    }

    public decimal ValorUnitario { get; private set; }
    public int Quantidade { get; private set; }
    public Guid ProdutoId { get; private set; }
    public Guid CarrinhoId { get; private set; }
    public CarrinhoCliente Carrinho { get; private set; }
    
    public void AtualizarQuantidade(int quantidade)
    {
        if (!ValidarQuantidade(quantidade)) throw new DomainException("Quantidade inválida");
        Quantidade = quantidade;
    }
    
    public bool ValidarProduto(Guid produtoId)
    {
        if(produtoId == Guid.Empty) return false;

        return true;
    }
    
    public bool ValidarValorUnitario(decimal valorUnitario)
    {
        if(valorUnitario <= 0) return false;

        return true;
    }
    
    public bool ValidarQuantidade(int quantidade)
    {
        if(quantidade <= 0) return false;

        return true;
    }
}