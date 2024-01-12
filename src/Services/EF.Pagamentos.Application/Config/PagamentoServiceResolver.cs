using EF.Domain.Commons.DomainObjects;
using EF.Pagamentos.Domain.Models;
using EF.Pagamentos.Domain.Ports;
using EF.Pagamentos.Infra.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EF.Pagamentos.Application.Config;

public class PagamentoServiceResolver
{
    private readonly IServiceProvider _provider;

    public PagamentoServiceResolver(IServiceProvider provider)
    {
        _provider = provider;
    }

    public IPagamentoService GetService(Tipo tipo)
    {
        switch (tipo)
        {
            case Tipo.PayPal:
                return _provider.GetService<PagamentoPayPalService>()!;
            // TODO: Incluir demais tipos que vamos usar
            default:
                throw new DomainException("Tipo de pagamento inválido");
        }
    }
}