using Microsoft.EntityFrameworkCore;
using BackendTechTest.Data;
using BackendTechTest.Models;
using BackendTechTest.Models.ApiModels;
using BackendTechTest.Services.Helpers;
using BackendTechTest.Enums;

namespace BackendTechTest.Services;

public class AppointmentService(AppDbContext context) : IAppointmentService
{
    private readonly AppDbContext _context = context;

    public async Task<IEnumerable<Appointment>> GetByPatientIdAsync(long id) =>
        await _context.Appointments.Where(a => a.NhsNumber == id).ToListAsync();

    public async Task<Appointment?> GetByIdAsync(Guid id) =>
        await _context.Appointments.FirstOrDefaultAsync(a => a.Id == id);

    public async Task<Appointment> CreateAsync(AppointmentApiModel model)
    {
        PostcodeHelper.ValidatePostcode(model.Postcode);

        var appointment = new Appointment
        {
            NhsNumber = model.NhsNumber,
            Status = GetStatus(model.Status),
            Time = model.Time,
            Duration = model.Duration,
            Clinician = model.Clinician,
            Department = model.Department,
            Postcode = model.Postcode
        };

        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync();

        return appointment;
    }

    public async Task<Appointment> UpdateAsync(Guid id, AppointmentApiModel model)
    {
        PostcodeHelper.ValidatePostcode(model.Postcode);

        var appointment = await _context.Appointments.FindAsync(id);
        if (appointment is null)
        {
            return null;
        }

        if (appointment.Status == AppointmentStatus.Cancelled)
        {
            throw new InvalidOperationException("Cannot update a cancelled appointment.");
        }

        appointment.Status = GetStatus(model.Status);
        appointment.Time = model.Time;
        appointment.Duration = model.Duration;
        appointment.Clinician = model.Clinician;
        appointment.Department = model.Department;
        appointment.Postcode = model.Postcode;

        await _context.SaveChangesAsync();
        return appointment;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var appointment = await _context.Appointments.FindAsync(id);
        if (appointment is null)
        {
            return false;
        }

        _context.Appointments.Remove(appointment);
        await _context.SaveChangesAsync();
        return true;
    }

    private static AppointmentStatus GetStatus(string status)
    {
        return status switch
        {
            "active" => AppointmentStatus.Active,
            "cancelled" => AppointmentStatus.Cancelled,
            "attended" => AppointmentStatus.Attended,
            "missed" => AppointmentStatus.Missed,
            _ => throw new ArgumentException("Invalid appointment status", nameof(status))
        };
    }
}
