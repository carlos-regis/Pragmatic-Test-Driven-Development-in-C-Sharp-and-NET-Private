using Microsoft.EntityFrameworkCore;

namespace Uqs.AppointmentBooking.Domain.Database;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
    }
}
