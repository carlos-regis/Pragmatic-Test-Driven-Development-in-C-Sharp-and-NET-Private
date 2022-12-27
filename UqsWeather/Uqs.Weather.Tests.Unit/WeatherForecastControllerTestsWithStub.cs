using AdamTibi.OpenWeather;
using Microsoft.Extensions.Logging.Abstractions;
using Uqs.Weather.Controllers;
using Uqs.Weather.Tests.Unit.Stubs;
using Xunit;

namespace Uqs.Weather.Tests.Unit;

public class WeatherForecastControllerTestsWithStub
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
    public async Task GetRealWeatherForecast_NotInterestedInTodayWeather_WFStartsFromNextDay()
    {
        // Arrange
        const double nextDayTemp = 3.3;
        const double day5Temp = 7.7;
        var today = new DateTime(2022, 1, 1);
        var realWeatherTemps = new double[] { 2, nextDayTemp, 4, 5.5, 6, day5Temp, 8 };
        var clientStub = new ClientStub(today, realWeatherTemps);
        var controller = new WeatherForecastController(null!, clientStub, null!, null!);

        // Act
        IEnumerable<WeatherForecast> wfs = await controller.GetRealWeatherForecast();

        // Assert
        Assert.Equal(3, wfs.First().TemperatureC);
    }

    [Fact]
    public async Task GetRealForecast5DaysForecastStartingNextDay_WF5ThDayIsRealWeather6ThDay()
    {
        // Arrange
        const double nextDayTemp = 3.3;
        const double day5Temp = 7.7;
        var today = new DateTime(2022, 1, 1);
        var realWeatherTemps = new[] { 2, nextDayTemp, 4, 5.5, 6, day5Temp, 8 };
        var clientStub = new ClientStub(today, realWeatherTemps);
        var controller = new WeatherForecastController(null!, clientStub, null!, null!);

        // Act
        IEnumerable<WeatherForecast> wfs = await controller.GetRealWeatherForecast();

        // Assert
        Assert.Equal(8, wfs.Last().TemperatureC);
    }

    [Fact]
    public async Task GetRealWeatherForecast_ForecastingFor5DaysOnly_WFHas5Days()
    {
        // Arrange
        const double nextDayTemp = 3.3;
        const double day5Temp = 7.7;
        var today = new DateTime(2022, 1, 1);
        var realWeatherTemps = new[] { 2, nextDayTemp, 4, 5.5, 6, day5Temp, 8 };
        var clientStub = new ClientStub(today, realWeatherTemps);
        var controller = new WeatherForecastController(null!, clientStub, null!, null!);

        // Act
        IEnumerable<WeatherForecast> wfs = await controller.GetRealWeatherForecast();

        // Assert
        Assert.Equal(5, wfs.Count());
    }

    [Fact]
    public async Task GetRealWeatherForecast_WFDoesntConsiderDecimal_RealWeatherTempRoundedProperly()
    {
        // Arrange
        const double nextDayTemp = 3.3;
        const double day5Temp = 7.7;
        var today = new DateTime(2022, 1, 1);
        var realWeatherTemps = new[] { 2, nextDayTemp, 4, 5.5, 6, day5Temp, 8 };
        var clientStub = new ClientStub(today, realWeatherTemps);
        var controller = new WeatherForecastController(null!, clientStub, null!, null!);

        // Act
        IEnumerable<WeatherForecast> wfs = await controller.GetRealWeatherForecast();

        // Assert
        Assert.Equal(3, wfs.First().TemperatureC);
        Assert.Equal(8, wfs.Last().TemperatureC);
    }

    [Fact]
    public async Task GetRealWeatherForecast_TodayWeatherAnd6DaysForecastReceived_RealDateMatchesNextDay()
    {
        // Arrange
        const double nextDayTemp = 3.3;
        const double day5Temp = 7.7;
        var today = new DateTime(2022, 1, 1);
        var realWeatherTemps = new[] { 2, nextDayTemp, 4, 5.5, 6, day5Temp, 8 };
        var clientStub = new ClientStub(today, realWeatherTemps);
        var controller = new WeatherForecastController(null!, clientStub, null!, null!);

        // Act
        IEnumerable<WeatherForecast> wfs = await controller.GetRealWeatherForecast();

        // Assert
        Assert.Equal(new DateTime(2022, 1, 2), wfs.First().Date);
    }

    [Fact]
    public async Task GetRealWeatherForecast_TodayWeatherAnd6DaysForecastReceived_RealDateMatchesLastDay()
    {
        // Arrange
        const double nextDayTemp = 3.3;
        const double day5Temp = 7.7;
        var today = new DateTime(2022, 1, 1);
        var realWeatherTemps = new[] { 2, nextDayTemp, 4, 5.5, 6, day5Temp, 8 };
        var clientStub = new ClientStub(today, realWeatherTemps);
        var controller = new WeatherForecastController(null!, clientStub, null!, null!);

        // Act
        IEnumerable<WeatherForecast> wfs = await controller.GetRealWeatherForecast();

        // Assert
        Assert.Equal(new DateTime(2022, 1, 6), wfs.Last().Date);
    }

    [Fact]
    public async Task GetRealWeatherForecast_RequestsToOpenWeather_MetricUnitIsUsed()
    {
        // Arrange
        var realWeatherTemps = new double[] { 1, 2, 3, 4, 5, 6, 7 };
        var clientStub = new ClientStub(default, realWeatherTemps);
        var controller = new WeatherForecastController(null!, clientStub, null!, null!);

        // Act
        var _ = await controller.GetRealWeatherForecast();

        // Assert
        Assert.NotNull(clientStub.LastUnitSpy);
        Assert.Equal(Units.Metric, clientStub.LastUnitSpy!.Value);
    }

    [Theory]
    [InlineData("Freezing", -1)]
    [InlineData("Freezing", 0)]
    [InlineData("Bracing", 1)]
    [InlineData("Bracing", 4.4)]
    [InlineData("Chilly", 5)]
    [InlineData("Chilly", 9.4)]
    [InlineData("Cool", 10)]
    [InlineData("Cool", 14.4)]
    [InlineData("Mild", 15)]
    [InlineData("Mild", 19.4)]
    [InlineData("Warm", 20)]
    [InlineData("Warm", 24.4)]
    [InlineData("Balmy", 25)]
    [InlineData("Balmy", 29.4)]
    [InlineData("Hot", 30)]
    [InlineData("Hot", 34.4)]
    [InlineData("Sweltering", 35)]
    [InlineData("Sweltering", 39.4)]
    [InlineData("Scorching", 40)]
    [InlineData("Scorching", 44.4)]
    [InlineData("Scorching", 46)]
    public async Task GetRealWeatherForecast_Summary_MatchesTemp(string summary, double temp)
    {
        // Arrange
        var realWeatherTemps = new[] { 1, temp, 3, 4, 5, 6, 7 };
        var clientStub = new ClientStub(default, realWeatherTemps);
        var controller = new WeatherForecastController(null!, clientStub, null!, null!);

        // Act
        var wf = await controller.GetRealWeatherForecast();

        // Assert
        Assert.Equal(summary, wf.First().Summary);
    }
}
