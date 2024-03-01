// using System.Net;
// using System.Net.Http.Json;
// using EF.Api.Test.Config;
// using EF.Carrinho.Application.Gateways;
// using EF.Carrinho.Application.UseCases.Interfaces;
// using EF.Carrinho.Domain.Models;
// using EF.Carrinho.Domain.Repository;
// using EF.Commons.Test.Builders.Carrinho;
// using EF.Commons.Test.Builders.Carrinho.Dtos;
// using EF.Commons.Test.Builders.Carrinho.Models;
// using EF.Commons.Test.Extensions;
// using EF.Produtos.Domain.Models;
// using FluentAssertions;
// using Microsoft.AspNetCore.TestHost;
// using Microsoft.Extensions.DependencyInjection;
// using Moq;
// using Moq.AutoMock;
//
// namespace EF.Api.Test.Controllers;
//
// [Collection(nameof(IntegrationTestsFixtureCollection))]
// public class CarrinhoControllerTest
// {
//     private readonly IntegrationTestsFixture _testsFixture;
//
//     public CarrinhoControllerTest(IntegrationTestsFixture testsFixture)
//     {
//         _testsFixture = testsFixture;
//     }
//
//     [Fact(DisplayName = "Adicionar item no carrinho vazio")]
//     [Trait("Category", "API - Carrinho")]
//     public async Task AdicionarItem_CarrinhoVazio_DeveRetornarSucesso()
//     {
//         // Arrange
//         await _testsFixture.AcessarApi();
//         _testsFixture.Client.AddToken(_testsFixture.RespostaTokenAcesso!.Token);
//
//         var adicionarItemDto = new AdicionarItemDtoBuilder().Generate();
//         
//
//         var mocker = new AutoMocker();
//         mocker.GetMock<IProdutoService>().Setup(r => r.ObterItemPorProdutoId(It.IsAny<Guid>()))
//             .Returns(Task.FromResult(item));
//         
//         _testsFixture.ApiFactory.WithWebHostBuilder(builder =>
//         {
//             builder.ConfigureTestServices(services =>
//             {
//                 services.AddSingleton(mocker.GetMock<IProdutoService>().Object);
//             });
//         });
//
//         // Act
//         var response = await _testsFixture.Client.PostAsJsonAsync("api/carrinho", adicionarItemDto);
//
//         // Assert
//         response.IsSuccessStatusCode.Should().BeTrue("Item adicionado com sucesso");
//         mocker.GetMock<ICarrinhoRepository>().Verify(r => r.AdicionarItem(It.IsAny<Item>()), Times.Once);
//     }
// }