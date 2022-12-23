using Microsoft.Extensions.Logging.Abstractions;
using Uqs.Weather.Controllers;

var logger = NullLogger<WeatherForecastController>
    .Instance;
var controller = new WeatherForecastController(logger, null!);

double f1 = controller.ConvertCelsiusToFahrenheit(-1.0);
if (f1 != 30.20d) throw new Exception("Invalid");

double f2 = controller.ConvertCelsiusToFahrenheit(1.2);
if (f2 != 34.16d) throw new Exception("Invalid");

Console.WriteLine("Test Passed");
