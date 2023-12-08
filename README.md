# Migrations

## Secrtes
{
"ConnectionStrings": {
"PedidosDb": "Server=localhost;Database=BASE;Port=5432;User Id=USER;Password=SENHA"
}
}
## Criar migrations
dotnet ef migrations add BaseInicialPedidos -p ./src/Services/EF.Pedidos.Infra -s ./src/Presentation/EF.Api -c PedidoDbContext
## Atualizar banco
dotnet ef database update -p ./src/Services/EF.Pedidos.Infra -s ./src/Presentation/EF.Api -c PedidoDbContext