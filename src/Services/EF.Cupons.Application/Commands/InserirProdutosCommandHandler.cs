using EF.Cupons.Domain.Models;
using EF.Cupons.Domain.Repository;
using EF.Domain.Commons.Messages;
using MediatR;

namespace EF.Cupons.Application.Commands
{
    public class InserirProdutosCommandHandler : CommandHandler,
        IRequestHandler<InserirProdutosCommand, CommandResult>
    {
        private readonly ICupomRepository _cupomRepository;

        public InserirProdutosCommandHandler(ICupomRepository cupomRepository)
        {
            _cupomRepository = cupomRepository;
        }

        public async Task<CommandResult> Handle(InserirProdutosCommand request, CancellationToken cancellationToken)
        {
            var produtos = GetProdutos(request);
            if (!await ValidarProdutos(request.CupomId, produtos, cancellationToken)) return CommandResult.Create(ValidationResult);
            await _cupomRepository.InserirProdutos(produtos, cancellationToken);
            var result = await PersistData(_cupomRepository.UnitOfWork);
            return CommandResult.Create(result);
        }

        private IList<CupomProduto> GetProdutos(InserirProdutosCommand command)
        {
            var produtos = new List<CupomProduto>();
            foreach(var p in command.Produtos)
            {
                produtos.Add(new CupomProduto(command.CupomId, p));
            }
            return produtos;
        }

        private async Task<bool> ValidarProdutos(Guid cupomId, IList<CupomProduto> produtos, CancellationToken cancellationToken)
        {
            if (produtos.Count == 0)
            {
                AddError("Nenhum produto informado para inserção");
                return false;
            }

            var cupom = await _cupomRepository.Buscar(cupomId, cancellationToken);
            if (cupom is null)
            {
                AddError("Cupom não encontrado");
                return false;
            }

            var produtosValidos = true;
            foreach (var p in produtos)
            {
                
                var cupomExistente = await _cupomRepository.BuscarCupomProduto(cupomId, p.ProdutoId, cancellationToken);
                if (cupomExistente is not null)
                {
                    AddError($"Produto {p.ProdutoId} já está inserido no cupom");
                    produtosValidos = false;
                }
            }
            if (!produtosValidos) return false;

            return true;
        }
    }
}
