using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BackendTechTest.Enums;

namespace BackendTechTest.Models;

public class Appointment
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public required long NhsNumber { get; set; }

    [ForeignKey(nameof(NhsNumber))]
    public Patient? Patient { get; set; }

    [Required]
    public AppointmentStatus Status { get; set; }

    [Required]
    public DateTimeOffset Time { get; set; }

    [Required]
    public required string Duration { get; set; }

    [Required]
    public required string Clinician { get; set; }

    [Required]
    public required string Department { get; set; }

    [Required]
    public required string Postcode { get; set; }
}
