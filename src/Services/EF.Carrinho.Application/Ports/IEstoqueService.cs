namespace EF.Carrinho.Application.Ports;

public interface IEstoqueService
{
    Task<bool> VerificarEstoque(Guid produtoId, int quantidade);
}