using Microsoft.AspNetCore.Mvc;
using BackendTechTest.Models;
using BackendTechTest.Models.ApiModels;
using BackendTechTest.Services;

namespace BackendTechTest.Controllers;

[ApiController]
[Route("api/[controller]")]
// [Authorize] // Uncomment once authentication is configured to secure this controller.
public class PatientsController : ControllerBase
{
    private readonly IPatientService _patientService;

    public PatientsController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id)
    {
        var patient = await _patientService.GetByIdAsync(id);
        return patient is null ? NotFound() : Ok(patient);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PatientApiModel model)
    {
        var patient = await _patientService.CreateAsync(model);
        return Ok(patient);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, [FromBody] PatientApiModel model)
    {
        var updated = await _patientService.UpdateAsync(id, model);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var deleted = await _patientService.DeleteAsync(id);
        return deleted ? Ok() : NotFound();
    }
}
