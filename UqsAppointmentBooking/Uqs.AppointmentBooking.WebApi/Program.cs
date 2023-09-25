var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;

//services.AddDbContext<ApplicationContext>(options =>
//    options.UseSqlServer(
//        builder.Configuration.GetConnectionString("AppointmentBooking")
//    ));

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
