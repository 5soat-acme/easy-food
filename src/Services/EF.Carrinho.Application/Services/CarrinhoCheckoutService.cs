using EF.Carrinho.Application.DTOs.Responses;
using EF.Carrinho.Application.Ports;
using EF.Carrinho.Application.Services.Interfaces;
using EF.Carrinho.Domain.Models;
using EF.Carrinho.Domain.Repository;
using EF.Domain.Commons.Communication;
using EF.Domain.Commons.Messages.Integrations;
using EF.WebApi.Commons.Users;
using FluentValidation.Results;

namespace EF.Carrinho.Application.Services;

public class CarrinhoCheckoutService : BaseCarrinhoService, ICarrinhoCheckoutService
{
    public CarrinhoCheckoutService(
        IUserApp user,
        ICarrinhoRepository carrinhoRepository,
        IEstoqueService estoqueService) : base(user, carrinhoRepository, estoqueService)
    {
    }

    public async Task<OperationResult<CheckoutRespostaDto>> IniciarCheckout()
    {
        var carrinho = await ObterCarrinho();

        if (carrinho is null) return OperationResult<CheckoutRespostaDto>.Failure("Carriho não encontrado");

        var validationResult = await Validar(carrinho);

        if (!validationResult.IsValid) return OperationResult<CheckoutRespostaDto>.Failure(validationResult);

        _carrinhoRepository.Remover(carrinho);

        // TODO: Confirmar com o time se adiamos essa decisão de trabalhar com eventos de domínio
        carrinho.AddEvent(new CheckoutIniciadoEvent
        {
            AggregateId = carrinho.Id,
            ValorTotal = carrinho.ValorTotal,
            ClienteId = carrinho.ClienteId ?? Guid.Empty,
            ValorFinal = carrinho.ValorFinal,
            Itens = carrinho.Itens.Select(i => new CheckoutIniciadoEvent.ItemCarrinhoFechado
            {
                ProdutoId = i.ProdutoId,
                NomeProduto = i.NomeProduto,
                Quantidade = i.Quantidade,
                ValorUnitario = i.ValorUnitario,
                Desconto = i.Desconto,
                ValorFinal = i.ValorFinal
            }).ToList()
        });

        await PersistirDados();

        return OperationResult<CheckoutRespostaDto>.Success(new CheckoutRespostaDto
        {
            CorrelacaoId = carrinho.Id
        });
    }

    private async Task<ValidationResult> Validar(CarrinhoCliente carrinho)
    {
        var validationResult = new ValidationResult();
        foreach (var item in carrinho.Itens)
            if (!await ValidarEstoque(item))
                validationResult.Errors.Add(new ValidationFailure("Estoque", $"{item.NomeProduto} sem estoque"));

        return validationResult;
    }
}