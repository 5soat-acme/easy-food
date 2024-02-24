using System.Net.Http.Headers;

namespace EF.Commons.Test.Extensions;

public static class HttpClientExtension
{
    public static void AddToken(this HttpClient client, string token)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}