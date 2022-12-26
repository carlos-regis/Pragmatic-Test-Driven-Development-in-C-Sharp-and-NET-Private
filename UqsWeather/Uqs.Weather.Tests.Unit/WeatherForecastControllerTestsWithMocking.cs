using AdamTibi.OpenWeather;
using Uqs.Weather.Controllers;
using NSubstitute;
using Xunit;

namespace Uqs.Weather.Tests.Unit;

public class WeatherForecastControllerTestsWithMocking
{
    [Fact]
    public async Task GetRealWeatherForecast_RequestsToOpenWeather_MetricUnitIsUsed()
    {
        // Arrange
        var realWeatherTemps = new double[] { 1, 2, 3, 4, 5, 6, 7 };
        var today = default(DateTime);
        var clientMock = Substitute.For<IClient>();
        clientMock.OneCallAsync(Arg.Any<decimal>(), Arg.Any<decimal>(),
                                Arg.Any<IEnumerable<Excludes>>(), Arg.Any<Units>())
            .Returns(_ =>
            {
                const int DAYS = 7;
                OneCallResponse response = new()
                {
                    Daily = new Daily[DAYS]
                };
                for (int i = 0; i < DAYS; i++)
                {
                    response.Daily[i] = new Daily
                    {
                        Dt = today.AddDays(i),
                        Temp = new Temp
                        {
                            Day = realWeatherTemps.ElementAt(i)
                        }
                    };
                }
                return Task.FromResult(response);
            });
        var controller = new WeatherForecastController(null!, clientMock, null!, null!);

        // Act
        var _ = await controller.GetRealWeatherForecast();

        // Assert
        await clientMock.Received().OneCallAsync(
            Arg.Any<decimal>(),
            Arg.Any<decimal>(),
            Arg.Any<IEnumerable<Excludes>>(),
            Arg.Is<Units>(x => x == Units.Metric));
    }
}
