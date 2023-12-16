dotnet ef database update -p ../src/Services/EF.Identidade.Infra -s ../src/Presentation/EF.Api -c IdentidadeDbContext
dotnet ef database update -p ../src/Services/EF.Clientes.Infra -s ../src/Presentation/EF.Api -c ClienteDbContext
dotnet ef database update -p ../src/Services/EF.Pedidos.Infra -s ../src/Presentation/EF.Api -c PedidoDbContext
dotnet ef database update -p ../src/Services/EF.Carrinho.Infra -s ../src/Presentation/EF.Api -c CarrinhoDbContext