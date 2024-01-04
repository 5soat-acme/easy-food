using EF.Domain.Commons.Repository;
using EF.Produtos.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Produtos.Domain.Repository
{
    public interface IProdutoRepository: IRepository<Produto>
    {
        Task<Produto?> BuscarPorId(Guid produtoId, CancellationToken cancellationToken);
        Task<IList<Produto>> Buscar(ProdutoCategoria? categoria);
        Task<Produto> Criar(Produto produto, CancellationToken cancellationToken);
        Produto Atualizar(Produto produto, CancellationToken cancellationToken);
    }
}
