namespace Uqs.Weather.Tests.Integration;

public class WeatherForecastTests
{
    private const string BASE_ADDRESS = "https://localhost:7218";
    private const string API_URI = "/WeatherForecast/GetRealWeatherForecast";

    private record WeatherForecast(DateTime Date, int TemperatureC, int TemperatureF, string? Summary);

    [Fact]
    public void Test1()
    {

    }
}