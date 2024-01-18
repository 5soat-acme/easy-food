using EF.Domain.Commons.Messages;
using EF.Produtos.Domain.Repository;
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

    public async Task<CommandResult> Handle(AtualizarProdutoCommand request, CancellationToken cancellationToken)
    {
        var produto = await _produtoRepository.BuscarPorId(request.ProdutoId, cancellationToken);
        produto!.AlterarProduto(request.Nome, request.ValorUnitario, request.Categoria, request.Ativo);
        _produtoRepository.Atualizar(produto, cancellationToken);

        var result = await PersistData(_produtoRepository.UnitOfWork);
        return CommandResult.Create(result);
    }

    public async Task<CommandResult> Handle(RemoverProdutoCommand request, CancellationToken cancellationToken)
    {
        var produto = await _produtoRepository.BuscarPorId(request.ProdutoId, cancellationToken);
        // IMPLEMENTAR CASO O PRODUTO NÃO EXISTA
        _produtoRepository.Remover(produto!, cancellationToken);
        var result = await PersistData(_produtoRepository.UnitOfWork);
        return CommandResult.Create(result);
    }
}
