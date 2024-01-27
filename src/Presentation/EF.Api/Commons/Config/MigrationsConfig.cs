using EF.Carrinho.Infra.Data;
using EF.Clientes.Infra.Data;
using EF.Cupons.Infra;
using EF.Estoques.Infra;
using EF.Identidade.Infra.Data;
using EF.Pagamentos.Infra;
using EF.Pedidos.Infra.Data;
using EF.PreparoEntrega.Infra.Data;
using EF.Produtos.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace EF.Api.Commons.Config;

public static class MigrationsConfig
{
    public static WebApplication RunMigrations(this WebApplication app)
    {
        try
        {
            using var scope = app.Services.CreateScope();

            var carrinho = scope.ServiceProvider.GetRequiredService<CarrinhoDbContext>();
            carrinho.Database.Migrate();

            var clientes = scope.ServiceProvider.GetRequiredService<ClienteDbContext>();
            clientes.Database.Migrate();

            var cupons = scope.ServiceProvider.GetRequiredService<CupomDbContext>();
            cupons.Database.Migrate();

            var estoque = scope.ServiceProvider.GetRequiredService<EstoqueDbContext>();
            estoque.Database.Migrate();

            var identidade = scope.ServiceProvider.GetRequiredService<IdentidadeDbContext>();
            identidade.Database.Migrate();

            var pagamentos = scope.ServiceProvider.GetRequiredService<PagamentoDbContext>();
            pagamentos.Database.Migrate();

            var pedidos = scope.ServiceProvider.GetRequiredService<PedidoDbContext>();
            pedidos.Database.Migrate();

            var preparoEntrega = scope.ServiceProvider.GetRequiredService<PreparoEntregaDbContext>();
            preparoEntrega.Database.Migrate();

            var produtos = scope.ServiceProvider.GetRequiredService<ProdutoDbContext>();
            produtos.Database.Migrate();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        
        return app;
    }
}