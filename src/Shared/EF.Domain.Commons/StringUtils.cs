namespace EF.Domain.Commons;

public static class StringUtils
{
    public static string SomenteNumeros(this string str, string input)
    {
        return new string(input.Where(char.IsDigit).ToArray());
    }
}