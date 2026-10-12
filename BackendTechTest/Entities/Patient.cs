using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendTechTest.Models;

public class Patient
{
    [Key]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public required long NhsNumber { get; set; }

    [Required]
    public required string Name { get; set; }

    [Required]
    public DateOnly DateOfBirth { get; set; }

    [Required]
    public required string Postcode { get; set; }
}
