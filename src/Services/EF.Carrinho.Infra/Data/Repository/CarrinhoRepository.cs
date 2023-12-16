using EF.Carrinho.Domain.Models;
using EF.Carrinho.Domain.Repository;
using EF.Domain.Commons.Repository;
using Microsoft.EntityFrameworkCore;

namespace EF.Carrinho.Infra.Data.Repository;

public sealed class CarrinhoRepository : ICarrinhoRepository
{
    private readonly CarrinhoDbContext _context;
    public IUnitOfWork UnitOfWork => _context;

    public CarrinhoRepository(CarrinhoDbContext context)
    {
        _context = context;
    }

    public async Task<CarrinhoCliente?> ObterPorCliente(Guid clienteId)
    {
        return await _context.Carrinhos
            .Include(c => c.Itens)
            .FirstOrDefaultAsync(c => c.ClienteId == clienteId);
    }

    public async Task<CarrinhoCliente?> ObterPorId(Guid id)
    {
        return await _context.Carrinhos
            .Include(c => c.Itens)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public CarrinhoCliente Criar(CarrinhoCliente carrinho)
    {
        var result =  _context.Carrinhos.Add(carrinho);
        return result.Entity;
    }

    public CarrinhoCliente Atualizar(CarrinhoCliente carrinho)
    {
        var result = _context.Carrinhos.Update(carrinho);
        return result.Entity;
    }

    public void Remover(CarrinhoCliente carrinho)
    {
        _context.Carrinhos.Remove(carrinho);
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}