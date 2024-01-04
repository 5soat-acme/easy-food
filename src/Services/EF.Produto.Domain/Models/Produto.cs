using EF.Domain.Commons.DomainObjects;

namespace EF.Produtos.Domain.Models
{
    public enum ProdutoCategoria
    {
       Lanche = 0,
       Acompanhamento = 1,
       Bebida = 2
    }

    public class Produto: Entity, IAggregateRoot
    {
        public Produto(string nome, decimal valorUnitario, bool ativo, ProdutoCategoria categoria)
        {
            if (!ValidarNome(nome)) throw new DomainException("Nome inválido");
            if (!ValidarValorUnitario(valorUnitario)) throw new DomainException("Valor unitário inválido");
            if (!ValidarCategoria(categoria)) throw new DomainException("Categoria inválida");

            Nome = nome;
            ValorUnitario = valorUnitario;
            Ativo = ativo;
            Categoria = categoria;
        }

        public string Nome { get; private set; }
        public decimal ValorUnitario { get; private set; }
        public bool Ativo { get; private set; }
        public ProdutoCategoria Categoria { get; private set; }

        public bool ValidarNome(string nome)
        {
            return !string.IsNullOrEmpty(nome);
        }

        public bool ValidarValorUnitario(decimal valorUnitario)
        {
            return valorUnitario > 0;
        }

        public bool ValidarCategoria(ProdutoCategoria categoria)
        {
            return Enum.IsDefined(typeof(ProdutoCategoria), categoria);
        }
    }
}
