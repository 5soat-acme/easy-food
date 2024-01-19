# Migrations

## Secrtes
{
"ConnectionStrings": {
"PedidosDb": "Server=localhost;Database=BASE;Port=5432;User Id=USER;Password=SENHA"
}
}

## Criar migrations

dotnet ef migrations add BaseInicialPedidos -p ./src/Services/EF.Pedidos.Infra -s ./src/Presentation/EF.Api -c PedidoDbContext

## Update Database
No powershell, execute o seguinte comando na raiz do projeto:

```bash
./deploy/migrations.ps1
```

https://www.postman.com/acme-dev/workspace/fiap-5soat/overview