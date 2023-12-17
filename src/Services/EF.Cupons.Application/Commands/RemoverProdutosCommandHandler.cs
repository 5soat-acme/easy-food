using EF.Cupons.Domain.Models;
using EF.Cupons.Domain.Repository;
using EF.Domain.Commons.Messages;
using MediatR;

namespace EF.Cupons.Application.Commands
{
    public class RemoverProdutosCommandHandler : CommandHandler,
        IRequestHandler<RemoverProdutosCommand, CommandResult>
    {
        private readonly ICupomRepository _cupomRepository;

        public RemoverProdutosCommandHandler(ICupomRepository cupomRepository)
        {
            _cupomRepository = cupomRepository;
        }

        public async Task<CommandResult> Handle(RemoverProdutosCommand request, CancellationToken cancellationToken)
        {
            var produtos = await GetProdutos(request, cancellationToken);
            if (!ValidarProdutos(produtos)) return CommandResult.Create(ValidationResult);
            _cupomRepository.RemoverProdutos(produtos, cancellationToken);
            var result = await PersistData(_cupomRepository.UnitOfWork);
            return CommandResult.Create(result);
        }

        private async Task<IList<CupomProduto>> GetProdutos(RemoverProdutosCommand command, CancellationToken cancellationToken)
        {
            var produtos = new List<CupomProduto>();
            foreach (var p in command.Produtos)
            {
                var cupomProd = await _cupomRepository.BuscarCupomProduto(command.CupomId, p, cancellationToken);
                if(cupomProd is not null)
                    produtos.Add(cupomProd);
            }
            return produtos;
        }

        private bool ValidarProdutos(IList<CupomProduto> produtos)
        {
            if (produtos.Count == 0)
            {
                AddError("Nenhum produto informado para ser removido");
                return false;
            }

            return true;
        }
    }
}
