namespace Uqs.Customer.Tests.Unit;

public class ProfileServiceTests
{
	[Fact]
	public void ChangeUsername_NullUsername_ArgumentNullException()
	{
        // Arrange
        var sut = new ProfileService();

        // Act
        var exception = Record.Exception(() => sut.ChangeUsername(null!));

        // Assert
        var ex = Assert.IsType<ArgumentNullException>(exception);
        Assert.Equal("username", ex.ParamName);
        Assert.StartsWith("Null", ex.Message);
    }
}