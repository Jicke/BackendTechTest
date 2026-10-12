using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using BackendTechTest.Enums;
using BackendTechTest.Models;
using BackendTechTest.Models.ApiModels;
using BackendTechTest.Services;
using Xunit;

namespace BackendTechTest.Tests.Services;

public class AppointmentServiceTests : TestContext
{
    private static AppointmentApiModel ValidModel(long nhsNumber = 1) => new()
    {
        NhsNumber = nhsNumber,
        Status = "active",
        Time = new DateTimeOffset(2026, 11, 1, 9, 30, 0, TimeSpan.Zero),
        Duration = "30m",
        Clinician = "Dr. Smith",
        Department = "GP",
        Postcode = "LS21 1AA"
    };

    private AppointmentService Service => new(Context);

    private async Task<Appointment> SeedAppointmentAsync(long nhsNumber = 1, AppointmentStatus status = AppointmentStatus.Active)
    {
        var appointment = new Appointment
        {
            NhsNumber = nhsNumber,
            Status = status,
            Time = new DateTimeOffset(2026, 11, 1, 9, 30, 0, TimeSpan.Zero),
            Duration = "30m",
            Clinician = "Dr. Smith",
            Department = "GP",
            Postcode = "LS21 1AA"
        };

        Context.Appointments.Add(appointment);
        await Context.SaveChangesAsync();

        return appointment;
    }

    [Fact]
    public async Task CreateAsync_AddsAppointment_WhenPostcodeIsValid()
    {
        var model = ValidModel();
        var result = await Service.CreateAsync(model);

        result.Id.Should().NotBe(Guid.Empty);
        result.NhsNumber.Should().Be(model.NhsNumber);
        result.Status.Should().Be(AppointmentStatus.Active);
        result.Duration.Should().Be(model.Duration);
        result.Clinician.Should().Be(model.Clinician);
        result.Department.Should().Be(model.Department);
        result.Time.Should().Be(model.Time);
        result.Postcode.Should().Be(model.Postcode);
        var entityCount = await Context.Appointments.CountAsync();
        entityCount.Should().Be(1);
    }

    [Fact]
    public async Task CreateAsync_Throws_WhenPostcodeIsInvalid()
    {
        var model = ValidModel();
        model.Postcode = "INVALIDPOSTCODE";

        var action = async () => await Service.CreateAsync(model);

        await action.Should().ThrowAsync<ArgumentException>();
        var entityCount = await Context.Appointments.CountAsync();
        entityCount.Should().Be(0);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsAppointment_WhenFound()
    {
        var seeded = await SeedAppointmentAsync();

        var found = await Service.GetByIdAsync(seeded.Id);

        found.Should().NotBeNull();
        found.Id.Should().Be(seeded.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
    {
        var found = await Service.GetByIdAsync(Guid.NewGuid());

        found.Should().BeNull();
    }

    [Fact]
    public async Task GetByPatientIdAsync_ReturnsOnlyMatchingAppointments()
    {
        await SeedAppointmentAsync(nhsNumber: 1);
        await SeedAppointmentAsync(nhsNumber: 1);
        await SeedAppointmentAsync(nhsNumber: 2);

        var results = await Service.GetByPatientIdAsync(1);

        results.Should().HaveCount(2);
        results.Should().OnlyContain(a => a.NhsNumber == 1);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesAppointment_WhenFound()
    {
        var seeded = await SeedAppointmentAsync();

        var updateModel = ValidModel();
        updateModel.Status = "cancelled";
        updateModel.Duration = "45m";
        updateModel.Clinician = "Dr. J Smith";
        updateModel.Department = "General Practitioner";
        updateModel.Time = new DateTimeOffset(2026, 11, 1, 10, 0, 0, TimeSpan.Zero);
        updateModel.Postcode = "LS21 1AB";

        var updated = await Service.UpdateAsync(seeded.Id, updateModel);

        updated.Should().NotBeNull();
        updated.Id.Should().Be(seeded.Id);
        updated.NhsNumber.Should().Be(updateModel.NhsNumber);
        updated.Status.Should().Be(AppointmentStatus.Cancelled);
        updated.Duration.Should().Be(updateModel.Duration);
        updated.Clinician.Should().Be(updateModel.Clinician);
        updated.Department.Should().Be(updateModel.Department);
        updated.Time.Should().Be(updateModel.Time);
        updated.Postcode.Should().Be(updateModel.Postcode);
    }

    [Fact]
    public async Task UpdateAsync_Throws_WhenPostcodeIsInvalid()
    {
        var seeded = await SeedAppointmentAsync();

        var updateModel = ValidModel();
        updateModel.Postcode = "NOTAPOSTCODE";

        var action = async () => await Service.UpdateAsync(seeded.Id, updateModel);

        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task UpdateAsync_Throws_WhenAppointmentIsCancelled()
    {
        var seeded = await SeedAppointmentAsync(status: AppointmentStatus.Cancelled);

        var updateModel = ValidModel();

        var action = async () => await Service.UpdateAsync(seeded.Id, updateModel);

        await action.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Cannot update a cancelled appointment.");
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNull_WhenNotFound()
    {
        var updated = await Service.UpdateAsync(Guid.NewGuid(), ValidModel());

        updated.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_RemovesAppointment_WhenFound()
    {
        var seeded = await SeedAppointmentAsync();

        var deleted = await Service.DeleteAsync(seeded.Id);

        deleted.Should().BeTrue();
        var entityCount = await Context.Appointments.CountAsync();
        entityCount.Should().Be(0);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalse_WhenNotFound()
    {
        var deleted = await Service.DeleteAsync(Guid.NewGuid());

        deleted.Should().BeFalse();
    }
}
