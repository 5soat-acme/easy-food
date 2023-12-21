using EF.Cupons.Domain.Models;
using EF.Cupons.Domain.Repository;
using EF.Domain.Commons.Messages;
using MediatR;

namespace EF.Cupons.Application.Commands
{
    public class InserirProdutosCommandHandler : CupomCommandBase,
        IRequestHandler<InserirProdutosCommand, CommandResult>
    {

        public InserirProdutosCommandHandler(ICupomRepository cupomRepository) : base(cupomRepository) { }

        public async Task<CommandResult> Handle(InserirProdutosCommand request, CancellationToken cancellationToken)
        {
            if (!await ValidarCupom(request.CupomId, cancellationToken)) return CommandResult.Create(ValidationResult);

            var produtos = await GetProdutos(request, cancellationToken);
            if (produtos.Count > 0)
            {
                await _cupomRepository.InserirProdutos(produtos, cancellationToken);
                await PersistData(_cupomRepository.UnitOfWork);
            }

            return CommandResult.Create(ValidationResult);
        }

        private async Task<IList<CupomProduto>> GetProdutos(InserirProdutosCommand command, CancellationToken cancellationToken)
        {
            var produtos = new List<CupomProduto>();
            foreach (var p in command.Produtos)
            {
                var prodExiste = (await _cupomRepository.BuscarCupomProduto(command.CupomId, p, cancellationToken)) is not null;
                if (!prodExiste)
                    produtos.Add(new CupomProduto(command.CupomId, p));
            }
            return produtos;
        }
    }
}