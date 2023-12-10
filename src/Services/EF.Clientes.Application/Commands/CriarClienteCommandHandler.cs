using EF.Clientes.Domain.Models;
using EF.Clientes.Domain.Repository;
using EF.Domain.Commons.Messages;
using FluentValidation.Results;
using MediatR;

namespace EF.Clientes.Application.Commands;

public class CriarClienteCommandHandler : CommandHandler,
    IRequestHandler<CriarClienteCommand, ValidationResult>
{
    private readonly IClienteRepository _clienteRepository;

    public CriarClienteCommandHandler(IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    public Task<ValidationResult> Handle(CriarClienteCommand request, CancellationToken cancellationToken)
    {
        var cliente = new Cliente(request.Cpf, request.PrimeiroNome, request.Sobrenome, request.Email);
        _clienteRepository.Criar(cliente);

        return PersistData(_clienteRepository.UnitOfWork);
    }
}