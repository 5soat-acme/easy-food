using AutoMapper;
using EF.Pagamentos.Application.DTOs.Responses;
using EF.Pagamentos.Application.Queries.Interfaces;
using EF.Pagamentos.Domain.Repository;

namespace EF.Pagamentos.Application.Queries;

public class FormaPagamentoQuery : IFormaPagamentoQuery
{
    private readonly IFormaPagamentoRepository _formaPagamentoRepository;
    private readonly IMapper _mapper;
    public FormaPagamentoQuery(IFormaPagamentoRepository formaPagamentoRepository,
        IMapper mapper)
    {
        _formaPagamentoRepository = formaPagamentoRepository;
        _mapper = mapper;
    }

    public async Task<IList<FormaPagamentoDto>> ObterFormasPagamento(CancellationToken cancellationToken)
    {
        var listaFormas = await _formaPagamentoRepository.BuscarTodas(cancellationToken);
        return _mapper.Map<List<FormaPagamentoDto>>(listaFormas);
    }
}
