using FluentAssertions;
using ReceiptManager.Core.Validations;
using System;
using Xunit;

namespace ReceiptManager.Tests
{
    public class RequestValidatorTests
    {
        private readonly IRequestValidator _sut;

        public RequestValidatorTests()
        {
            _sut = new RequestValidator();
        }

        [Theory]
        [InlineData("Dell laptop", true)]
        [InlineData("Razor 3000 MX-15 super laptop", true)]
        [InlineData(" x5 ", true)]
        public void IsValid_InputValid_ReturnsTrue(string testProductName, bool expected)
        {
            // Act
            var actual = _sut.IsValid(testProductName);

            // Assert
            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData("", false)]
        [InlineData("      ", false)]
        public void IsValid_InputInvalid_ReturnsFalse(string testProductName, bool expected)
        {
            // Act
            var actual = _sut.IsValid(testProductName);

            // Assert
            actual.Should().Be(expected);
        }

        [Fact]
        public void IsValidDate_InputValid_ReturnsTrue()
        {
            // Act
            var actual = _sut.IsValidDate(new DateTime(2022, 10, 10), new DateTime(2022, 11, 01));

            // Assert
            actual.Should().BeTrue();
        }

        [Fact]
        public void IsValidDate_InputInvalidSingle_ReturnsFalse()
        {
            // Act
            var actual = _sut.IsValidDate(new DateTime(2023, 10, 10), new DateTime(2022, 1, 01));

            // Assert
            actual.Should().BeFalse();
        }

        [Fact]
        public void IsValidDate_InputInvalidMultiple_ReturnsFalse()
        {
            // Act
            var actual = _sut.IsValidDate(new DateTime(2023, 10, 10), DateTime.MinValue);

            // Assert
            actual.Should().BeFalse();
        }
    }
}