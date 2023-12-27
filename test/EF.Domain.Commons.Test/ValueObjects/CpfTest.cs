using EF.Domain.Commons.ValueObjects;
using FluentAssertions;

namespace EF.Domain.Commons.Test.ValueObjects;

public class CpfTest
{
    [Theory(DisplayName = "CPF válido com máscara")]
    [Trait("Category", "Domain.Commons.ValueObjects.Cpf")]
    [InlineData("907.339.944-07")]
    [InlineData("327.556.178-29")]
    [InlineData("331.318.157-01")]
    [InlineData("454.535.607-97")]
    [InlineData("707.397.625-08")]
    [InlineData("654.404.208-00")]
    [InlineData("713.386.961-65")]
    [InlineData("775.736.043-94")]
    [InlineData("171.746.268-58")]
    [InlineData("381.438.153-07")]
    [InlineData("668.335.436-74")]
    [InlineData("518.329.869-69")]
    [InlineData("890.405.367-67")]
    [InlineData("935.167.031-72")]
    [InlineData("314.747.903-05")]
    [InlineData("408.601.269-35")]
    [InlineData("844.947.225-37")]
    [InlineData("956.826.204-02")]
    [InlineData("253.202.265-51")]
    [InlineData("078.671.448-46")]
    public void Cpf_Validar_CpfValidoComMascara_DeveRetornarTrue(string numero)
    {
        // Arrange & Act
        var result = Cpf.Validar(numero);

        // Assert
        result.Should().BeTrue("o CPF é válido");
    }

    [Theory(DisplayName = "CPF válido sem máscara")]
    [Trait("Category", "Domain.Commons.ValueObjects.Cpf")]
    [InlineData("90733994407")]
    [InlineData("32755617829")]
    [InlineData("33131815701")]
    [InlineData("45453560797")]
    [InlineData("70739762508")]
    [InlineData("65440420800")]
    [InlineData("71338696165")]
    [InlineData("77573604394")]
    [InlineData("17174626858")]
    [InlineData("38143815307")]
    [InlineData("66833543674")]
    [InlineData("51832986969")]
    [InlineData("89040536767")]
    [InlineData("93516703172")]
    [InlineData("31474790305")]
    [InlineData("40860126935")]
    [InlineData("84494722537")]
    [InlineData("95682620402")]
    [InlineData("25320226551")]
    [InlineData("07867144846")]
    public void Cpf_Validar_CpfValidoSemMascara_DeveRetornarTrue(string numero)
    {
        // Arrange & Act
        var result = Cpf.Validar(numero);

        // Assert
        result.Should().BeTrue("o CPF é válido");
    }

    [Theory(DisplayName = "CPF inválido com máscara")]
    [Trait("Category", "Domain.Commons.ValueObjects.Cpf")]
    [InlineData("617.942.889-54")]
    [InlineData("593.766.398-82")]
    [InlineData("168.560.388-19")]
    [InlineData("040.210.654-07")]
    [InlineData("095.821.876-93")]
    [InlineData("861.443.355-04")]
    [InlineData("012.562.356-03")]
    [InlineData("474.365.402-02")]
    [InlineData("338.649.696-07")]
    [InlineData("891.966.476-84")]
    [InlineData("962.991.988-17")]
    [InlineData("753.642.123-43")]
    [InlineData("072.043.342-65")]
    [InlineData("036.708.767-44")]
    [InlineData("555.497.232-62")]
    [InlineData("462.549.175-25")]
    [InlineData("116.354.587-42")]
    [InlineData("616.007.809-25")]
    [InlineData("048.153.753-46")]
    [InlineData("020.356.151-49")]
    public void Cpf_Validar_CpfInvalidoValidoComMascara_DeveRetornarFalse(string numero)
    {
        // Arrange & Act
        var result = Cpf.Validar(numero);

        // Assert
        result.Should().BeFalse("o CPF é inválido");
    }

    [Theory(DisplayName = "CPF inválido sem máscara")]
    [Trait("Category", "Domain.Commons.ValueObjects.Cpf")]
    [InlineData("61794288954")]
    [InlineData("59376639882")]
    [InlineData("16856038819")]
    [InlineData("04021065407")]
    [InlineData("09582187693")]
    [InlineData("86144335504")]
    [InlineData("01256235603")]
    [InlineData("47436540202")]
    [InlineData("33864969607")]
    [InlineData("89196647684")]
    [InlineData("96299198817")]
    [InlineData("75364212343")]
    [InlineData("07204334265")]
    [InlineData("03670876744")]
    [InlineData("55549723262")]
    [InlineData("46254917525")]
    [InlineData("11635458742")]
    [InlineData("61600780925")]
    [InlineData("04815375346")]
    [InlineData("02035615149")]
    public void Cpf_Validar_CpfInvalidoValidoSemMascara_DeveRetornarFalse(string numero)
    {
        // Arrange & Act
        var result = Cpf.Validar(numero);

        // Assert
        result.Should().BeFalse("o CPF é inválido");
    }
}