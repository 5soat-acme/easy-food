using EF.Domain.Commons.Messages;
using EF.Estoques.Domain.Models;
using EF.Estoques.Domain.Repository;
using MediatR;

namespace EF.Estoques.Application.Commands
{
    public class AtualizarEstoqueCommandHandler : CommandHandler,
        IRequestHandler<AtualizarEstoqueCommand, CommandResult>
    {
        private readonly IEstoqueRepository _estoqueRepository;

        public AtualizarEstoqueCommandHandler(IEstoqueRepository estoqueRepository)
        {
            _estoqueRepository = estoqueRepository;
        }

        public async Task<CommandResult> Handle(AtualizarEstoqueCommand command, CancellationToken cancellationToken)
        {
            var estoque = await GetEstoque(command, cancellationToken);
            estoque.AdicionarMovimentacao(new MovimentacaoEstoque(estoque.Id, command.ProdutoId, command.Quantidade, command.TipoMovimentacao,
                                                                  command.OrigemMovimentacao, DateTime.Now));
            await _estoqueRepository.Salvar(estoque, cancellationToken);
            var result = await PersistData(_estoqueRepository.UnitOfWork);
            return CommandResult.Create(result, estoque.Id);
        }

        private async Task<Estoque> GetEstoque(AtualizarEstoqueCommand command, CancellationToken cancellationToken)
        {
            var estoqueExistente = await _estoqueRepository.Buscar(command.ProdutoId, cancellationToken);
            var estoque = estoqueExistente ?? new Estoque(command.ProdutoId, 0);
            estoque.AtualizarQuantidade(command.Quantidade, command.TipoMovimentacao);
            return estoque;
        }
    }
}