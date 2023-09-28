using Uqs.AppointmentBooking.Domain.Services.Implementations;
using Uqs.AppointmentBooking.Domain.Tests.Unit.Fakes;

namespace Uqs.AppointmentBooking.Domain.Tests.Unit.Services;

public class ServicesServiceTests : IDisposable
{
    private readonly ApplicationContextFakeBuilder _contextBuilder = new();
    private ServicesService? _sut;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing && _contextBuilder is not null)
        {
            _contextBuilder.Dispose();
        }
    }

    [Fact]
    public async Task GetActiveServices_NoServiceInTheSystem_NoServices()
    {
        // Arrange
        var ctx = _contextBuilder.Build();
        _sut = new ServicesService(ctx);

        // Act
        var actual = await _sut.GetActiveServices();

        // Assert
        Assert.True(!actual.Any());
    }

    [Fact]
    public async Task GetActiveServices_TwoActiveOneInactiveService_TwoServices()
    {
        // Arrange
        var ctx = _contextBuilder
            .WithSingleService(true)
            .WithSingleService(true)
            .WithSingleService(false)
            .Build();
        _sut = new ServicesService(ctx);
        var expected = 2;

        // Act
        var actual = await _sut.GetActiveServices();

        // Assert
        Assert.Equal(expected, actual.Count());
    }
}
