﻿using EF.Cupons.Application.DTOs.Requests;
using EF.Domain.Commons.Messages;

namespace EF.Cupons.Application.Commands;

public class CriarCupomCommand : Command
{
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public string CodigoCupom { get; set; }
    public decimal PorcentagemDesconto { get; set; }
    public IReadOnlyCollection<AdicionarRemoverCupomProdutoDto> Produtos { get; set; }
}