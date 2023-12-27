$commands = @(
    "dotnet ef database update -p ../src/Services/EF.Identidade.Infra -s ../src/Presentation/EF.Api -c IdentidadeDbContext",
    "dotnet ef database update -p ../src/Services/EF.Clientes.Infra -s ../src/Presentation/EF.Api -c ClienteDbContext",
    "dotnet ef database update -p ../src/Services/EF.Pedidos.Infra -s ../src/Presentation/EF.Api -c PedidoDbContext",
    "dotnet ef database update -p ../src/Services/EF.Carrinho.Infra -s ../src/Presentation/EF.Api -c CarrinhoDbContext",
    "dotnet ef database update -p ./src/Services/EF.Estoques.Infra -s ./src/Presentation/EF.Api -c EstoqueDbContext",
    "dotnet ef database update -p ./src/Services/EF.Cupons.Infra -s ./src/Presentation/EF.Api -c CupomDbContext"
)

foreach ($cmd in $commands)
{
    try
    {
        Write-Host "Executing: $cmd"
        Invoke-Expression $cmd
    }
    catch
    {
        Write-Host "Error when executing: $cmd"
        Write-Host "Error: $_"
    }
}

Write-Host "Finish."
