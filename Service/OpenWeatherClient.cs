using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

// https://www.youtube.com/watch?v=g-JGay_lnWI
public class OpenWeatherClient : IWeatherClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly JsonSerializerOptions _options;
    public OpenWeatherClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task<List<object>> GetDataAsync()
    {
        var httpClient = _httpClientFactory.CreateClient("weatherapi");
        List<object>? returnResult;

        using (var response = await httpClient.GetAsync("/posts", HttpCompletionOption.ResponseHeadersRead))
        {
            response.EnsureSuccessStatusCode();
            var stream = await response.Content.ReadAsStreamAsync();
            //var companies = await JsonSerializer.DeserializeAsync<List<CompanyDto>>(stream, _options);
            var result = await JsonSerializer.DeserializeAsync<List<object>>(stream, _options);
            returnResult = result;
        }

        return returnResult;
    }
}

  