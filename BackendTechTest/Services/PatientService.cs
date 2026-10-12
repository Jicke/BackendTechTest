using Microsoft.EntityFrameworkCore;
using BackendTechTest.Data;
using BackendTechTest.Models;
using BackendTechTest.Models.ApiModels;
using BackendTechTest.Services.Helpers;

namespace BackendTechTest.Services;

public class PatientService(AppDbContext context) : IPatientService
{
    private readonly AppDbContext _context = context;

    public async Task<Patient?> GetByIdAsync(long id) =>
        await _context.Patients.FirstOrDefaultAsync(p => p.NhsNumber == id);

    public async Task<Patient> CreateAsync(PatientApiModel model)
    {
        NhsNumberHelper.ValidateNhsNumber(model.NhsNumber);
        PostcodeHelper.ValidatePostcode(model.Postcode);

        var patient = new Patient
        {
            NhsNumber = model.NhsNumber,
            Name = model.Name,
            DateOfBirth = model.DateOfBirth,
            Postcode = model.Postcode
        };

        _context.Patients.Add(patient);
        await _context.SaveChangesAsync();

        return patient;
    }

    public async Task<Patient> UpdateAsync(long id, PatientApiModel model)
    {
        var patient = await _context.Patients.FindAsync(id);
        if (patient is null)
        {
            return null;
        }

        patient.Name = model.Name;
        patient.DateOfBirth = model.DateOfBirth;
        patient.Postcode = model.Postcode;

        await _context.SaveChangesAsync();
        return patient;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var patient = await _context.Patients.FindAsync(id);
        if (patient is null)
        {
            return false;
        }

        _context.Patients.Remove(patient);
        await _context.SaveChangesAsync();
        return true;
    }
}
