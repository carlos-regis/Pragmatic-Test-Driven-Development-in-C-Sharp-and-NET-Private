using Uqs.AppointmentBooking.Domain.Services.Interfaces;

namespace Uqs.AppointmentBooking.Domain.Services;

public class NowService : INowService
{
    public DateTime Now => DateTime.Now;
}
