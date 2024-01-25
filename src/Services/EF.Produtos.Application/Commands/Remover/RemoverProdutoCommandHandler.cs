using EF.Domain.Commons.Messages;
using EF.Produtos.Domain.Repository;
using FluentValidation;
using MediatR;

namespace EF.Produtos.Application.Commands;

public class RemoverProdutoCommandHandler : CommandHandler,
    IRequestHandler<RemoverProdutoCommand, CommandResult>
{
    private readonly IProdutoRepository _produtoRepository;

    public RemoverProdutoCommandHandler(IProdutoRepository produtoRepository)
    {
        _produtoRepository = produtoRepository;
    }

    public async Task<CommandResult> Handle(RemoverProdutoCommand request, CancellationToken cancellationToken)
    {
        var produto = await _produtoRepository.BuscarPorId(request.ProdutoId);
        if (produto is null) throw new ValidationException("Produto não existe");
        _produtoRepository.Remover(produto!);
        var result = await PersistData(_produtoRepository.UnitOfWork);
        return CommandResult.Create(result);
    }
}