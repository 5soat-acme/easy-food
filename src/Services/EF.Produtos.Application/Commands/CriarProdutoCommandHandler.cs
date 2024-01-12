using EF.Domain.Commons.Messages;
using EF.Produtos.Domain.Models;
using EF.Produtos.Domain.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Produtos.Application.Commands;

internal class CriarProdutoCommandHandler : CommandHandler,
    IRequestHandler<CriarProdutoCommand, CommandResult>
{

    private readonly IProdutoRepository _produtoRepository;

    public CriarProdutoCommandHandler(IProdutoRepository produtoRepository)
    {
        _produtoRepository = produtoRepository;
    }

    public async Task<CommandResult> Handle(CriarProdutoCommand request, CancellationToken cancellationToken)
    {
        var produto = new Produto(request.Nome, request.ValorUnitario, request.Categoria);
        await _produtoRepository.Criar(produto, cancellationToken);
        var result = await PersistData(_produtoRepository.UnitOfWork);
        return CommandResult.Create(result, produto.Id);
    }
}
