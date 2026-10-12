using BackendTechTest.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BackendTechTest.Models.ApiModels
{
    public class AppointmentApiModel
    {
        [JsonPropertyName("patient")]
        public required long NhsNumber { get; set; }

        public required string Status { get; set; }

        public DateTimeOffset Time { get; set; }

        public required string Duration { get; set; }

        public required string Clinician { get; set; }

        public required string Department { get; set; }

        public required string Postcode { get; set; }
    }
}
