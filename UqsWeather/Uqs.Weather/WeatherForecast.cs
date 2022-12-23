namespace Uqs.Weather;

public class WeatherForecast
{
    public DateTime Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC * (5d / 9d));

    public string? Summary { get; set; }
}
