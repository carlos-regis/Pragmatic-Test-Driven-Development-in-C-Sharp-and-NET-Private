using Microsoft.AspNetCore.Mvc;
using AdamTibi.OpenWeather;
using Uqs.Weather.Wrappers;

namespace Uqs.Weather.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private const int FORECAST_DAYS = 5;
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IClient _client;
    private readonly INowWrapper _nowWrapper;
    private readonly IRandomWrapper _randomWrapper;
    
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild",
        "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    public WeatherForecastController(ILogger<WeatherForecastController> logger,
                                     IClient client,
                                     INowWrapper nowWrapper,
                                     IRandomWrapper randomWrapper)
    {
        _logger = logger;
        _client = client;
        _nowWrapper = nowWrapper;
        _randomWrapper = randomWrapper;
    }

    [HttpGet("ConvertCelsiusToFahrenheit")]
    public double ConvertCelsiusToFahrenheit(double temperatureC)
    {
        double temperatureF = temperatureC * (9d / 5d) + 32;

        _logger.LogInformation("Temperature conversion requested");

        return temperatureF;
    }

    [HttpGet("GetRealWeatherForecast")]
    public async Task<IEnumerable<WeatherForecast>> GetRealWeatherForecast()
    {
        const decimal GREENWICH_LAT = 51.4810m;
        const decimal GREENWICH_LON = 0.0052m;    
        OneCallResponse oneCallResponse = await _client.OneCallAsync
            (GREENWICH_LAT, GREENWICH_LON, new[]
            {
                Excludes.Current, Excludes.Minutely,
                Excludes.Hourly, Excludes.Alerts
            }, Units.Metric);

        WeatherForecast[] weatherForecasts = new WeatherForecast[FORECAST_DAYS];
        for (int i = 0; i < weatherForecasts.Length; i++)
        {
            var weatherForecast = weatherForecasts[i] = new WeatherForecast();
            weatherForecast.Date = oneCallResponse.Daily[i + 1].Dt;
            double forecastTemperature = oneCallResponse.Daily[i + 1].Temp.Day;
            weatherForecast.TemperatureC = (int)Math.Round(forecastTemperature);
            weatherForecast.Summary = MapFeelToTemp(weatherForecast.TemperatureC);
        }
        return weatherForecasts;
    }

    [HttpGet("GetRandomWeatherForecast")]
    public IEnumerable<WeatherForecast> GetRandomWeatherForecast()
    {
        WeatherForecast[] weatherForecasts = new WeatherForecast[FORECAST_DAYS];
        for (int i = 0; i < weatherForecasts.Length; i++)
        {
            var weatherForecast = weatherForecasts[i] = new WeatherForecast();
            weatherForecast.Date = _nowWrapper.Now.AddDays(i + 1);
            weatherForecast.TemperatureC = _randomWrapper.Next(-20, 55);
            weatherForecast.Summary = MapFeelToTemp(weatherForecast.TemperatureC);
        }
        return weatherForecasts;
    }

    private static string MapFeelToTemp(int temperatureC)
    {
        if (temperatureC <= 0)
        {
            return Summaries.First();
        }

        int summariesIndex = (temperatureC / 5) + 1;
        if (summariesIndex >= Summaries.Length)
        {
            return Summaries.Last();
        }

        return Summaries[summariesIndex];
    }
}
