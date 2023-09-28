using Uqs.AppointmentBooking.Domain.DomainObjects;

namespace Uqs.AppointmentBooking.Domain.Services.Interfaces;

public interface IServicesService
{
    Task<Service?> GetServiceById(int id);
    Task<IEnumerable<Service>> GetActiveServices();
}
