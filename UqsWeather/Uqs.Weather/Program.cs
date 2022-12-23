using AdamTibi.OpenWeather;
using Uqs.Weather;
using Uqs.Weather.Wrappers;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add services to the container.
services.AddSingleton<IClient>(_ => {
    bool isLoad = bool.Parse(builder.Configuration["LoadTest:IsActive"]);
    if (isLoad)
    { 
        return new ClientStub(); 
    }
    else
    {
        string apiKey = builder.Configuration["OpenWeather:Key"];
        HttpClient httpClient = new();

        return new Client(apiKey, httpClient);
    }
});

services.AddSingleton<INowWrapper>(_ => new NowWrapper());
services.AddTransient<IRandomWrapper>(_ => new RandomWrapper());

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
