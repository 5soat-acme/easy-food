#!/bin/bash

commands=(
    "dotnet ef database update -p ./src/Services/EF.Identidade.Infra -s ./src/Presentation/EF.Api -c IdentidadeDbContext"
    "dotnet ef database update -p ./src/Services/EF.Clientes.Infra -s ./src/Presentation/EF.Api -c ClienteDbContext"
    "dotnet ef database update -p ./src/Services/EF.Pedidos.Infra -s ./src/Presentation/EF.Api -c PedidoDbContext"
    "dotnet ef database update -p ./src/Services/EF.Carrinho.Infra -s ./src/Presentation/EF.Api -c CarrinhoDbContext"
    "dotnet ef database update -p ./src/Services/EF.Estoques.Infra -s ./src/Presentation/EF.Api -c EstoqueDbContext"
    "dotnet ef database update -p ./src/Services/EF.Cupons.Infra -s ./src/Presentation/EF.Api -c CupomDbContext"
    "dotnet ef database update -p ./src/Services/EF.Pagamentos.Infra -s ./src/Presentation/EF.Api -c PagamentoDbContext"
    "dotnet ef database update -p ./src/Services/EF.PreparoEntrega.Infra -s ./src/Presentation/EF.Api -c PreparoEntregaDbContext"
    "dotnet ef database update -p ./src/Services/EF.Produtos.Infra -s ./src/Presentation/EF.Api -c ProdutoDbContext"
)

errors=()
for cmd in "${commands[@]}"; do
    echo "Executing: $cmd"
    if ! eval $cmd; then
        errorMsg="Error when executing: $cmd"
        echo "$errorMsg"
        errors+=("$errorMsg")
    fi
done

echo "Finish."

if [ ${#errors[@]} -gt 0 ]; then
    echo "Errors occurred during execution:"
    for errorMsg in "${errors[@]}"; do
        echo "$errorMsg"
    done
fi
