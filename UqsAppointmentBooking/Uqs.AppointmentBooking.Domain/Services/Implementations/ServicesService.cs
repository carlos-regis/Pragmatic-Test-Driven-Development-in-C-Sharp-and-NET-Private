using Microsoft.EntityFrameworkCore;
using Uqs.AppointmentBooking.Domain.Database;
using Uqs.AppointmentBooking.Domain.DomainObjects;
using Uqs.AppointmentBooking.Domain.Services.Interfaces;

namespace Uqs.AppointmentBooking.Domain.Services.Implementations;

public class ServicesService : IServicesService
{
    private readonly ApplicationContext _context;

    public ServicesService(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Service>> GetActiveServices()
        => await _context.Services!.Where(x => x.IsActive).ToArrayAsync();

    public async Task<Service?> GetServiceById(int id)
        => await _context.Services!.SingleOrDefaultAsync(x => x.IsActive && x.Id == id);
}
