using EF.Cupons.Domain.Models;
using EF.Cupons.Domain.Repository;
using EF.Domain.Commons.Messages;
using MediatR;

namespace EF.Cupons.Application.Commands
{
    public class RemoverProdutosCommandHandler : CupomCommandBase,
        IRequestHandler<RemoverProdutosCommand, CommandResult>
    {

        public RemoverProdutosCommandHandler(ICupomRepository cupomRepository) : base(cupomRepository) { }

        public async Task<CommandResult> Handle(RemoverProdutosCommand request, CancellationToken cancellationToken)
        {
            if (!await ValidarCupom(request.CupomId, cancellationToken)) return CommandResult.Create(ValidationResult);

            var produtos = await GetProdutos(request, cancellationToken);
            if (produtos.Count > 0)
            {
                _cupomRepository.RemoverProdutos(produtos, cancellationToken);
                await PersistData(_cupomRepository.UnitOfWork);
            }

            return CommandResult.Create(ValidationResult);
        }

        private async Task<IList<CupomProduto>> GetProdutos(RemoverProdutosCommand command, CancellationToken cancellationToken)
        {
            var produtos = new List<CupomProduto>();
            foreach (var p in command.Produtos)
            {
                var cupomProd = await _cupomRepository.BuscarCupomProduto(command.CupomId, p, cancellationToken);
                if (cupomProd is not null)
                    produtos.Add(cupomProd);
            }
            return produtos;
        }
    }
}
