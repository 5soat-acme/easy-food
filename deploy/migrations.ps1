$commands = @(
    "dotnet ef database update -p ./src/Services/EF.Identidade.Infra -s ./src/Presentation/EF.Api -c IdentidadeDbContext",
    "dotnet ef database update -p ./src/Services/EF.Clientes.Infra -s ./src/Presentation/EF.Api -c ClienteDbContext",
    "dotnet ef database update -p ./src/Services/EF.Pedidos.Infra -s ./src/Presentation/EF.Api -c PedidoDbContext",
    "dotnet ef database update -p ./src/Services/EF.Carrinho.Infra -s ./src/Presentation/EF.Api -c CarrinhoDbContext",
    "dotnet ef database update -p ./src/Services/EF.Estoques.Infra -s ./src/Presentation/EF.Api -c EstoqueDbContext",
    "dotnet ef database update -p ./src/Services/EF.Cupons.Infra -s ./src/Presentation/EF.Api -c CupomDbContext"
)

$errorsList = @()

foreach ($cmd in $commands)
{
    try
    {
        Write-Host "Executing: $cmd"
        Invoke-Expression $cmd
    }
    catch
    {
        $errorMsg = "Error when executing: $cmd `nError: $_"
        Write-Host $errorMsg
        $errorsList += $errorMsg
    }
}

Write-Host "Finish."

if ($errorsList.Count -gt 0)
{
    Write-Host "Errors occurred during execution:"
    $errorsList | ForEach-Object { Write-Host $_ }
}