using EF.Clientes.Domain.Models;
using EF.Clientes.Domain.Repository;
using EF.Domain.Commons.Messages;
using MediatR;

namespace EF.Clientes.Application.Commands;

public class CriarClienteCommandHandler : CommandHandler,
    IRequestHandler<CriarClienteCommand, CommandResult>
{
    private readonly IClienteRepository _clienteRepository;

    public CriarClienteCommandHandler(IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    public async Task<CommandResult> Handle(CriarClienteCommand request, CancellationToken cancellationToken)
    {
        var cliente = new Cliente(request.Cpf, request.PrimeiroNome, request.Sobrenome, request.Email);
        await _clienteRepository.Criar(cliente);
        var result = await PersistData(_clienteRepository.UnitOfWork);
        return CommandResult.Create(result, cliente.Id);
    }
}