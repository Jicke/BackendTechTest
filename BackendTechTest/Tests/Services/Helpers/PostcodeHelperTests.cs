using FluentAssertions;
using BackendTechTest.Services.Helpers;
using Xunit;

namespace BackendTechTest.Tests.Services.Helpers;

public class PostcodeHelperTests
{
    [Theory]
    [InlineData("LS21 1AB")]
    [InlineData("L1 1AA")]
    [InlineData("L1A 1AA")]
    public void ValidatePostcode_DoesNotThrow_ForValidPostcode(string postcode)
    {
        var action = () => PostcodeHelper.ValidatePostcode(postcode);

        action.Should().NotThrow();
    }

    [Theory]
    [InlineData("INVALIDPOSTCODE")]
    [InlineData("1W1A 1AA")]
    [InlineData("SW1A 1A")]
    [InlineData("SW1A AAA")]
    [InlineData("")]
    public void ValidatePostcode_Throws_ForInvalidPostcode(string postcode)
    {
        var action = () => PostcodeHelper.ValidatePostcode(postcode);

        action.Should().Throw<ArgumentException>();
    }
}
