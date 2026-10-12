using BackendTechTest.Models;
using BackendTechTest.Models.ApiModels;

namespace BackendTechTest.Services;

public interface IPatientService
{
    Task<Patient?> GetByIdAsync(long id);
    Task<Patient> CreateAsync(PatientApiModel model);
    Task<Patient> UpdateAsync(long id, PatientApiModel model);
    Task<bool> DeleteAsync(long id);
}
