namespace Uqs.Weather.Tests.Integration;

using System.Net.Http.Json;
using Xunit;

public class WeatherForecastTests
{
    private const int FORECAST_DAYS = 5;
    private const string BASE_ADDRESS = "https://localhost:7286";
    private const string API_URI = "/WeatherForecast/GetRealWeatherForecast";
    private record WeatherForecast(DateTime Date,
                                   int TemperatureC,
                                   int TemperatureF,
                                   string? Summary);

    // You have to run Uqs.Weather before running this test
    [Fact]
    public async Task GetRealWeatherForecast_Execute_GetNext5Days()
    {
        // Arrange
        var today = DateTime.Now.Date;
        DateTime[] next5Days = new DateTime[FORECAST_DAYS];
        for (int i = 0; i < FORECAST_DAYS; i++)
        {
            next5Days[i] = today.AddDays(i + 1);
        }

        HttpClient httpClient = new()
        {
            BaseAddress = new Uri(BASE_ADDRESS)
        };

        // Act
        var httpResponse = await httpClient.GetAsync(API_URI);

        // Assert
        var wfs = await httpResponse.Content.ReadFromJsonAsync<WeatherForecast[]>();
        for (int i = 0; i < FORECAST_DAYS; i++)
        {
            Assert.Equal(next5Days[i], wfs![i].Date.Date);
        }
    }
}
