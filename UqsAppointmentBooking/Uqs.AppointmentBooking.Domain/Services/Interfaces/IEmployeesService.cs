using Uqs.AppointmentBooking.Domain.DomainObjects;

namespace Uqs.AppointmentBooking.Domain.Services.Interfaces;

public interface IEmployeesService
{
    Task<IEnumerable<Employee>> GetEmployees();
}
