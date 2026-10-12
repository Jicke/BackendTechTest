using FluentAssertions;
using BackendTechTest.Services.Helpers;
using Xunit;

namespace BackendTechTest.Tests.Services.Helpers;

public class NhsNumberHelperTests
{
    [Theory]
    [InlineData(1373645350)]
    [InlineData(1326391658)]
    [InlineData(6381935308)]
    public void ValidateNhsNumber_DoesNotThrow_ForValidNumber(long nhsNumber)
    {
        var action = () => NhsNumberHelper.ValidateNhsNumber(nhsNumber);

        action.Should().NotThrow();
    }

    [Theory]
    [InlineData(1234567890)]
    [InlineData(123456789)]
    [InlineData(12345678901)]
    public void ValidateNhsNumber_Throws_ForInvalidNumber(long nhsNumber)
    {
        var action = () => NhsNumberHelper.ValidateNhsNumber(nhsNumber);

        action.Should().Throw<ArgumentException>();
    }
}
