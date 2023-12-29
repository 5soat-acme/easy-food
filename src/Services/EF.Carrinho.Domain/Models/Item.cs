using EF.Domain.Commons.DomainObjects;

namespace EF.Carrinho.Domain.Models;

public class Item : Entity
{
    // Necessário para o Entity Framework
    protected Item()
    {
    }

    public Item(Guid produtoId, string nomeProduto, decimal valorUnitario, int tempoPreparoEstimado)
    {
        if (!ValidarProduto(produtoId)) throw new DomainException("Produto inválido");
        if (!ValidarValorUnitario(valorUnitario)) throw new DomainException("Valor unitário inválido");

        ProdutoId = produtoId;
        NomeProduto = nomeProduto;
        ValorUnitario = valorUnitario;
        ValorFinal = valorUnitario;
        TempoPreparoEstimado = tempoPreparoEstimado;
    }

    public decimal ValorUnitario { get; private set; }
    public decimal? Desconto { get; private set; }
    public decimal ValorFinal { get; private set; }
    public int Quantidade { get; private set; }
    public Guid ProdutoId { get; private set; }
    public string NomeProduto { get; private set; }
    public int TempoPreparoEstimado { get; private set; }
    public Guid CarrinhoId { get; private set; }
    public CarrinhoCliente Carrinho { get; }

    public void AtualizarQuantidade(int quantidade)
    {
        if (!ValidarQuantidade(quantidade)) throw new DomainException("Quantidade inválida");
        Quantidade = quantidade;
    }

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

    public void AssociarCarrinho(Guid carrinhoId)
    {
        if (carrinhoId == Guid.Empty) throw new DomainException("Id do carrinho inválido");
        CarrinhoId = carrinhoId;
    }

    public void AplicarDesconto(decimal desconto)
    {
        if (desconto < 0 || desconto > 100)
            throw new DomainException("Porcentagem de desconto deve estar entre 0 e 100.");

        Desconto = desconto;

        AtualizarValorFinal();
    }

    public void AtualizarValorFinal()
    {
        if (Desconto is not null)
            ValorFinal = ValorUnitario - ValorUnitario * Desconto.Value;
        else
            ValorFinal = ValorUnitario;
    }
}