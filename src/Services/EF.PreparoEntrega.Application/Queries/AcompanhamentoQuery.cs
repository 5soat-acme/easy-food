using AutoMapper;
using EF.PreparoEntrega.Application.DTOs.Responses;
using EF.PreparoEntrega.Application.Queries.Interfaces;
using EF.PreparoEntrega.Domain.Repository;

namespace EF.PreparoEntrega.Application.Queries;

public class AcompanhamentoQuery : IAcompanhamentoQuery
{
    private readonly IMapper _mapper;
    private readonly IPedidoRepository _pedidoRepository;

    public AcompanhamentoQuery(IPedidoRepository pedidoRepository, IMapper mapper)
    {
        _pedidoRepository = pedidoRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PedidoAcompanhamentoDto>> ObterPedidos()
    {
        var pedidos = await _pedidoRepository.ObterPedidosEmAberto();
        return _mapper.Map<IEnumerable<PedidoAcompanhamentoDto>>(pedidos);
    }
}