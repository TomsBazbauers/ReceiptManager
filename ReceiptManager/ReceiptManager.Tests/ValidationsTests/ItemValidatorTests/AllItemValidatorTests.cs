using FluentAssertions;
using ReceiptManager.Core.Models;
using ReceiptManager.Core.Validations;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ReceiptManager.Tests
{
    public class AllItemValidatorTests
    {
        private IEnumerable<IItemValidator> _sut;

        public AllItemValidatorTests()
        {
            _sut = new List<IItemValidator>()
            {
                new ItemCountValidator(),
                new ItemNameValidator(),
            };
        }

        [Fact]
        public void IsValid_InputValidAll_ReturnsTrue()
        {
            // Arrange
            var testList = new List<Item>()
            {
                new Item("Bose headphones"),
                new Item("Lenovo laptop"),
                new Item("Logitech mouse")
            };

            // Act
            var actual = _sut.All(validator => validator.IsValid(testList));

            // Assert
            actual.Should().BeTrue();
        }

        [Fact]
        public void IsValid_InputInvalidCount_ReturnsFalse()
        {
            // Arrange
            var testList = new List<Item>();

            // Act
            var actual = _sut.All(validator => validator.IsValid(testList));

            // Assert
            actual.Should().BeFalse();
        }

        [Theory]
        [InlineData("", false)]
        [InlineData("         ", false)]
        public void IsValid_InputInvalidName_ReturnsFalse(string testProductName, bool expected)
        {
            // Arrange
            var testList = new List<Item>()
            {
                new Item("Bose headphones"),
                new Item(testProductName),
                new Item("Logitech mouse")
            };

            // Act
            var actual = _sut.All(validator => validator.IsValid(testList));

            // Assert
            actual.Should().Be(expected);
        }
    }
}