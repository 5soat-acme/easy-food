using AutoMapper;
using EF.Pedidos.Application.DTOs.Adapters;
using EF.Pedidos.Application.Ports;

namespace EF.Pedidos.Application.Adapters;

public class ProdutoAdapter : IProdutoService
{
    private readonly IMapper _mapper;

    public ProdutoAdapter(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<ProdutoDto> ObterPorId(Guid id)
    {
        // TODO: Retirar este código e incluir chamada no contexto de produtos
        var source = new EF.Produtos.Application.DTOs.Responses.ProdutoDto
        {
            Id = id,
            Descricao = "Descrição do produto",
            Nome = "Nome do produto",
            ValorUnitario = 35.53m,
            TempoPreparoEstimado = 15
        };

        return _mapper.Map<ProdutoDto>(source);
    }
}