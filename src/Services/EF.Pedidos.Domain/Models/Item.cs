using EF.Domain.Commons.DomainObjects;

namespace EF.Pedidos.Domain.Models;

public class Item : Entity
{
    // EF
    protected Item()
    {
    }

    public Item(Guid produtoId, string nomeProduto, decimal valorUnitario, int quantidade, decimal? desconto)
    {
        if (!ValidarProduto(produtoId)) throw new DomainException("Produto inválido");
        if (!ValidarValorUnitario(valorUnitario)) throw new DomainException("Valor unitário inválido");
        if (!ValidarQuantidade(quantidade)) throw new DomainException("Quantidade inválida");

        ProdutoId = produtoId;
        NomeProduto = nomeProduto;
        ValorUnitario = valorUnitario;
        Quantidade = quantidade;
        Desconto = desconto;
    }

    public Guid PedidoId { get; private set; }
    public decimal ValorUnitario { get; private set; }
    public decimal? Desconto { get; private set; }
    public decimal ValorFinal { get; private set; }
    public int Quantidade { get; private set; }
    public Guid ProdutoId { get; private set; }
    public string NomeProduto { get; private set; }
    public Pedido Pedido { get; }

    public bool ValidarProduto(Guid produtoId)
    {
        if (produtoId == Guid.Empty) return false;

        return true;
    }

    public bool ValidarValorUnitario(decimal valorUnitario)
    {
        if (valorUnitario <= 0) return false;

        return true;
    }

    public bool ValidarQuantidade(int quantidade)
    {
        if (quantidade <= 0) return false;

        return true;
    }

    public void CalcularValorFinal()
    {
        if (Desconto is not null)
            ValorFinal = ValorUnitario - ValorUnitario * Desconto.Value;
        else
            ValorFinal = ValorUnitario;
    }
}