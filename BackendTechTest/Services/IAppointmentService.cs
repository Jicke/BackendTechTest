using BackendTechTest.Models;
using BackendTechTest.Models.ApiModels;

namespace BackendTechTest.Services;

public interface IAppointmentService
{
    Task<IEnumerable<Appointment>> GetByPatientIdAsync(long id);
    Task<Appointment?> GetByIdAsync(Guid id);
    Task<Appointment> CreateAsync(AppointmentApiModel model);
    Task<Appointment> UpdateAsync(Guid id, AppointmentApiModel model);
    Task<bool> DeleteAsync(Guid id);
}
