using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using BackendTechTest.Models;
using BackendTechTest.Models.ApiModels;
using BackendTechTest.Services;
using BackendTechTest.Tests;
using Xunit;

namespace BackendTechTest.Tests.Services;

public class PatientServiceTests : TestContext
{
    private static PatientApiModel ValidModel() => new()
    {
        NhsNumber = 9634522262,
        Name = "John Smith",
        DateOfBirth = new DateOnly(2000, 1, 1),
        Postcode = "LS21 1AA"
    };

    private PatientService Service => new(Context);

    private async Task<Patient> SeedPatientAsync()
    {
        var patient = new Patient
        {
            NhsNumber = 9634522262,
            Name = "John Smith",
            DateOfBirth = new DateOnly(2000, 1, 1),
            Postcode = "LS21 1AA"
        };

        Context.Patients.Add(patient);
        await Context.SaveChangesAsync();

        return patient;
    }

    [Fact]
    public async Task CreateAsync_AddsPatient_WhenNhsNumberIsValid()
    {
        var result = await Service.CreateAsync(ValidModel());

        var entityCount = await Context.Patients.CountAsync();
        entityCount.Should().Be(1);
        result.NhsNumber.Should().NotBe(0);
        result.Name.Should().Be("John Smith");
    }

    [Fact]
    public async Task CreateAsync_Throws_WhenNhsNumberIsInvalid()
    {
        var model = new PatientApiModel
        {
            NhsNumber = 1234567890,
            Name = "John Smith",
            DateOfBirth = new DateOnly(1990, 1, 1),
            Postcode = "LS21 1AA"
        };

        var action = async () => await Service.CreateAsync(model);

        await action.Should().ThrowAsync<ArgumentException>();
        var entityCount = await Context.Patients.CountAsync();
        entityCount.Should().Be(0);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsPatient_WhenFound()
    {
        var seeded = await SeedPatientAsync();

        var found = await Service.GetByIdAsync(seeded.NhsNumber);

        found.Should().NotBeNull();
        found.NhsNumber.Should().Be(seeded.NhsNumber);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
    {
        var found = await Service.GetByIdAsync(-1);

        found.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_UpdatesPatient_WhenFound()
    {
        var seeded = await SeedPatientAsync();

        var updated = await Service.UpdateAsync(seeded.NhsNumber, new PatientApiModel
        {
            NhsNumber = seeded.NhsNumber,
            Name = "John Smithy",
            DateOfBirth = new DateOnly(1990, 1, 1),
            Postcode = "LS21 1AB"
        });

        updated.Should().NotBeNull();
        updated.Name.Should().Be("John Smithy");
        updated.NhsNumber.Should().Be(seeded.NhsNumber);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNull_WhenNotFound()
    {
        var updated = await Service.UpdateAsync(-1, ValidModel());

        updated.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_RemovesPatient_WhenFound()
    {
        var seeded = await SeedPatientAsync();

        var deleted = await Service.DeleteAsync(seeded.NhsNumber);

        deleted.Should().BeTrue();
        var entityCount = await Context.Patients.CountAsync();
        entityCount.Should().Be(0);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalse_WhenNotFound()
    {
        var deleted = await Service.DeleteAsync(-1);

        deleted.Should().BeFalse();
    }
}
