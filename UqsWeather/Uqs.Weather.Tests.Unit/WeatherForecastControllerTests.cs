using Microsoft.Extensions.Logging.Abstractions;
using Uqs.Weather.Controllers;
using Uqs.Weather.Tests.Unit.Stubs;
using Xunit;

namespace Uqs.Weather.Tests.Unit;

public class WeatherForecastControllerTests
{
    [Fact]
    public void ConvertCelsiusToFahrenheit_0Celsius_32Fahrenheit()
    {
        // Arrange
        const double EXPECTED = 32d;
        var logger = NullLogger<WeatherForecastController>.Instance;
        var wfController = new WeatherForecastController(logger, null!, null!, null!);

        // Act
        double actual = wfController.ConvertCelsiusToFahrenheit(0);

        // Assert
        Assert.Equal(EXPECTED, actual);
    }

    [Theory]
    [InlineData(-100, -148)]
    [InlineData(-10.1, 13.8)]
    [InlineData(10, 50)]
    [InlineData(100, 212)]
    [InlineData(0, 32)]
    public void ConvertCelsiusToFahrenheit_Celsius_CorrectFahrenheit(double celsius, double fahrenheit)
    {
        // Arrange
        var logger = NullLogger<WeatherForecastController>.Instance;
        var wfController = new WeatherForecastController(logger, null!, null!, null!);

        // Act
        double actual = wfController.ConvertCelsiusToFahrenheit(celsius);

        // Assert
        Assert.Equal(fahrenheit, actual, 1);
    }

    [Fact]
    public void GetRealForecast_NotInterestedInTodayWeather_WFStartsFromNextDay()
    {
        // Arrange
        const double nextDayTemp = 3.3;
        const double day5Temp = 7.7;
        var today = new DateTime(2022, 1, 1);
        var realWeatherTemps = new double[] { 2, nextDayTemp, 4, 5.5, 6, day5Temp, 8 };
        var clientStub = new ClientStub(today, realWeatherTemps);
        var controller = new WeatherForecastController(null!, clientStub, null!, null!);

        // Act
        IEnumerable<WeatherForecast> wfs = await controller.GetRealForecast();

        // Assert
        Assert.Equal(3, wfs.First().TemperatureC);

    }
}
