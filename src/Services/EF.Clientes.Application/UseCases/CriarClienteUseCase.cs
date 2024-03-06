using EF.Clientes.Application.DTOs;
using EF.Clientes.Application.UseCases.Interfaces;
using EF.Clientes.Domain.Models;
using EF.Clientes.Domain.Repository;
using EF.Core.Commons.Communication;
using EF.Core.Commons.UseCases;

namespace EF.Clientes.Application.UseCases;

public class CriarClienteUseCase : CommonUseCase, ICriarClienteUseCase
{
    private readonly IClienteRepository _clienteRepository;

    public CriarClienteUseCase(IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    public async Task<OperationResult<Guid>> Handle(CriarClienteDto clienteDto)
    {
        var cliente = new Cliente(clienteDto.Cpf, clienteDto.PrimeiroNome, clienteDto.Sobrenome, clienteDto.Email);
        await _clienteRepository.Criar(cliente);
        await PersistData(_clienteRepository.UnitOfWork);

        if (!ValidationResult.IsValid) return OperationResult<Guid>.Failure(ValidationResult);

        return OperationResult<Guid>.Success(cliente.Id);
    }
}