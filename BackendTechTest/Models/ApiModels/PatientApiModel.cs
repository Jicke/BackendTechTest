using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BackendTechTest.Models.ApiModels
{
    public class PatientApiModel
    {
        [JsonPropertyName("nhs_number")]
        public required long NhsNumber { get; set; }

        public required string Name { get; set; }

        [JsonPropertyName("date_of_birth")]
        public DateOnly DateOfBirth { get; set; }

        public required string Postcode { get; set; }
    }
}
