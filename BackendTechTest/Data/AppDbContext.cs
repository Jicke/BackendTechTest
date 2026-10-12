using Microsoft.EntityFrameworkCore;
using BackendTechTest.Models;

namespace BackendTechTest.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Patient> Patients => Set<Patient>();

    public DbSet<Appointment> Appointments => Set<Appointment>();
}
