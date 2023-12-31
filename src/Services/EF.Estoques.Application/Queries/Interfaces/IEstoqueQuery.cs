﻿using EF.Estoques.Application.DTOs.Responses;

namespace EF.Estoques.Application.Queries.Interfaces;

public interface IEstoqueQuery
{
    public Task<EstoqueDto?> ObterEstoqueProduto(Guid produtoId, CancellationToken cancellationToken);
}