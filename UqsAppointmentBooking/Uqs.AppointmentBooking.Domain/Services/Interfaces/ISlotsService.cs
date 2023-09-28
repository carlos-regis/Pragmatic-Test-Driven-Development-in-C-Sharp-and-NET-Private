using Uqs.AppointmentBooking.Domain.Report;

namespace Uqs.AppointmentBooking.Domain.Services.Interfaces;

public interface ISlotsService
{
    Task<Slots> GetAvailableSlotsForEmployee(int serviceId, int employeeId);
}
