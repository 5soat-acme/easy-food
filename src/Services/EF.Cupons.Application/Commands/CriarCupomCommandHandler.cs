using EF.Cupons.Domain.Models;
using EF.Cupons.Domain.Repository;
using EF.Domain.Commons.Messages;
using MediatR;

namespace EF.Cupons.Application.Commands
{
    public class CriarCupomCommandHandler : CommandHandler,
        IRequestHandler<CriarCupomCommand, CommandResult>
    {
        private readonly ICupomRepository _cupomRepository;

        public CriarCupomCommandHandler(ICupomRepository cupomRepository)
        {
            _cupomRepository = cupomRepository;
        }
        public async Task<CommandResult> Handle(CriarCupomCommand request, CancellationToken cancellationToken)
        {
            var cupom = GetCupom(request);
            if (!await ValidarCupom(cupom, cancellationToken)) return CommandResult.Create(ValidationResult);
            await _cupomRepository.Criar(cupom, cancellationToken);
            var result = await PersistData(_cupomRepository.UnitOfWork);
            return CommandResult.Create(result, cupom.Id);
        }

        private Cupom GetCupom(CriarCupomCommand command)
        {
            var cupom = new Cupom(command.DataInicio, command.DataFim, command.CodigoCupom, command.PorcentagemDesconto, CupomStatus.Ativo);
            foreach (var prod in command.Produtos)
            {
                cupom.AdicionarProduto(new CupomProduto(cupom.Id, prod.ProdutoId));
            }
            return cupom;
        }

        private async Task<bool> ValidarCupom(Cupom cupom, CancellationToken cancellationToken)
        {
            var cupomExistente = await _cupomRepository.BuscarCupomVigenteEmPeriodo(cupom.Id, cupom.CodigoCupom, cupom.DataInicio, cupom.DataFim, cancellationToken);
            if (cupomExistente.Count > 0)
            {
                AddError("Já existe cupom com este código em vigência para o mesmo período");
                return false;
            }
            return true;
        }
    }
}
