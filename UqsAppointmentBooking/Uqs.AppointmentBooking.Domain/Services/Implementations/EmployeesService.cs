using Microsoft.EntityFrameworkCore;
using Uqs.AppointmentBooking.Domain.Database;
using Uqs.AppointmentBooking.Domain.DomainObjects;
using Uqs.AppointmentBooking.Domain.Services.Interfaces;

namespace Uqs.AppointmentBooking.Domain.Services;

public class EmployeesService : IEmployeesService
{
    private readonly ApplicationContext _context;

    public EmployeesService(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Employee>> GetEmployees()
        => await _context.Employees!.ToArrayAsync();
}
