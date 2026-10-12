using Microsoft.AspNetCore.Mvc;
using BackendTechTest.Models.ApiModels;
using BackendTechTest.Services;

namespace BackendTechTest.Controllers;

[ApiController]
[Route("api/[controller]")]
// [Authorize] // Uncomment once authentication is configured to secure this controller.
public class AppointmentsController : ControllerBase    
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentsController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    [HttpGet("GetByPatientId/{id:long}")]
    public async Task<IActionResult> GetByPatientId(long id)
    {
        var appointments = await _appointmentService.GetByPatientIdAsync(id);
        return Ok(appointments);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var appointment = await _appointmentService.GetByIdAsync(id);
        return appointment is null ? NotFound() : Ok(appointment);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AppointmentApiModel model)
    {
        var appointment = await _appointmentService.CreateAsync(model);
        return Ok(appointment);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] AppointmentApiModel model)
    {
        var updated = await _appointmentService.UpdateAsync(id, model);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _appointmentService.DeleteAsync(id);
        return deleted ? Ok() : NotFound();
    }
}
