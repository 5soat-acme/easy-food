using EF.Domain.Commons.Messages;
using EF.Produtos.Domain.Repository;
using MediatR;

namespace EF.Produtos.Application.Commands;

public class BuscarProdutoCommandHandler : CommandHandler,
    IRequestHandler<AtualizarProdutoCommand, CommandResult>
{

    private readonly IProdutoRepository _produtoRepository;

    public BuscarProdutoCommandHandler(IProdutoRepository produtoRepository)
    {
        _produtoRepository = produtoRepository;
    }

    public async Task<CommandResult> Handle(AtualizarProdutoCommand request, CancellationToken cancellationToken)
    {
        var produtos = await _produtoRepository.Buscar(request.Categoria, cancellationToken);

        var result = await PersistData(_produtoRepository.UnitOfWork);
        return CommandResult.Create(result);
    }
}
